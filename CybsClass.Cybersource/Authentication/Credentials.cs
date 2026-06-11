using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CybsClass.Cybersource.Authentication
{
    /// <summary>
    /// Winhost-friendly credentials:
    /// - Prefer PEM mode: Resource\keys\private.pem (+ password) + Resource\certs\public.cer (DER)
    /// - Fallback to PFX: AuthCredentialFile.RestP12JwtCredential with EphemeralKeySet
    /// - Creates RS256 JWT (Jose-JWT) with x5c + v-c-merchant-id headers
    /// - Provides HMAC HTTP Signature helpers
    /// </summary>
    public static class Credentials
    {
        // ===== Config-backed values (Initialize) =====
        public static string? merchantKeyId { get; private set; }
        public static string? merchantsecretKey { get; private set; } // base64 HMAC key for HTTP Signature
        public static string? requestHost { get; private set; }       // e.g. apitest.cybersource.com
        public static string? P12JwtCredential { get; private set; }  // filename/relative/absolute .p12
        public static string? MerchantId { get; private set; }
        public static string? SignatureMerchantId { get; private set; } // portfolio: derived from p12 name
        public static string? KeyPass { get; private set; }           // PFX or PEM passphrase
        public static string? WebServiceUrlAddress { get; private set; }
        public static string? IsPortfolioCredential { get; private set; }

        // Diagnostics
        public static string? ResolvedCertPath { get; private set; }
        public static string? ResolvedPemPrivatePath { get; private set; }
        public static string? ResolvedPemPublicPath { get; private set; }

        // ===== Init =====
        public static void Initialize(
            string p12JwtCredential,
            string isPortfolioCredential,
            string merchantID,
            string keyPass,
            string keyId,
            string sharedSecret,
            string baseUrlAddress)
        {
            P12JwtCredential = p12JwtCredential;
            IsPortfolioCredential = isPortfolioCredential ?? "false";
            MerchantId = merchantID;
            KeyPass = (keyPass ?? string.Empty).Trim();
            merchantKeyId = keyId;
            merchantsecretKey = sharedSecret;
            requestHost = baseUrlAddress;
            WebServiceUrlAddress = baseUrlAddress;

            // Derive SignatureMerchantId for portfolio mode (from P12 filename)
            if (string.Equals(IsPortfolioCredential, "true", StringComparison.OrdinalIgnoreCase))
            {
                var name = p12JwtCredential ?? string.Empty;
                SignatureMerchantId = name.EndsWith(".p12", StringComparison.OrdinalIgnoreCase)
                    ? Path.GetFileNameWithoutExtension(name)
                    : Path.GetFileName(name);
            }
            else
            {
                SignatureMerchantId = merchantID;
            }

            Console.WriteLine($"[Config] RestP12JwtCredential = {P12JwtCredential}");
            Console.WriteLine($"[Config] IsPortfolioCredential = {IsPortfolioCredential}");
            Console.WriteLine($"[Config] MerchantId = {MerchantId}");
            Console.WriteLine($"[Config] SignatureMerchantId = {SignatureMerchantId}");
            Console.WriteLine($"[Config] KeyPass length = {KeyPass?.Length ?? 0}");
            Console.WriteLine($"[Config] BaseUrlAddress = {requestHost}");
        }

        public static string GetMerchantID() => MerchantId ?? string.Empty;
        public static string GetRequestHost() => requestHost ?? string.Empty;
        public static string GetMerchantKeyId() => merchantKeyId ?? string.Empty;
        public static string GetMerchantSecretKey() => merchantsecretKey ?? string.Empty;
        public static string? GetSignatureMerchantId() => SignatureMerchantId;
        public static string GetIsPortfolioCredential() { return IsPortfolioCredential!; }
        public static string GetWebServiceUrlAddress() => WebServiceUrlAddress ?? requestHost ?? string.Empty;

        // ===== JWT (RS256 via Jose-JWT) =====
        public static string GenerateJWT(string request, string method, bool boardingAPI)
        {
            string token = "TOKEN_PLACEHOLDER";

            try
            {
                bool isPost = string.Equals(method, "POST", StringComparison.OrdinalIgnoreCase)
                          || string.Equals(method, "PATCH", StringComparison.OrdinalIgnoreCase);

                string jwtBody = isPost
                    ? "{\n" +
                      $"\"digest\":\"{Convert.ToBase64String(SHA256.HashData(Encoding.ASCII.GetBytes(request ?? string.Empty)))}\", " +
                      "\"digestAlgorithm\":\"SHA-256\", " +
                      $"\"iat\":\"{DateTime.UtcNow.ToString("r")}\"" +
                      "}"
                    : $"{{\"iat\":\"{DateTime.UtcNow.ToString("r")}\"}}";

                Console.WriteLine($"\tMethod : {method}");
                Console.WriteLine($"\n\tJWT BODY : {jwtBody}");

                // Load signing material (PEM preferred; else PFX Ephemeral)
                using var material = LoadSigningMaterialOrThrow(); // IDisposable wrapper
                var x5cBase64 = Convert.ToBase64String(material.PublicCert.RawData);

                // Determine which merchant ID to use based on API type and portfolio mode
                string merchantIdForJwt;
                
                if (boardingAPI)
                {
                    // Boarding API always uses Portfolio/Signature Merchant ID
                    merchantIdForJwt = SignatureMerchantId ?? MerchantId ?? string.Empty;
                    Console.WriteLine($"\t[JWT] Using Portfolio/Signature Merchant ID for Boarding API: {merchantIdForJwt}");
                }
                else if (string.Equals(IsPortfolioCredential, "true", StringComparison.OrdinalIgnoreCase))
                {
                    // Portfolio mode: Regular APIs use Transacting Merchant ID (from config)
                    merchantIdForJwt = MerchantId ?? string.Empty;
                    Console.WriteLine($"\t[JWT] Portfolio mode - Using Transacting Merchant ID: {merchantIdForJwt}");
                    Console.WriteLine($"\t[JWT] Portfolio ID (from P12 filename): {SignatureMerchantId}");
                }
                else
                {
                    // Non-portfolio mode: Use the configured Merchant ID
                    merchantIdForJwt = MerchantId ?? string.Empty;
                    Console.WriteLine($"\t[JWT] Using Merchant ID: {merchantIdForJwt}");
                }

                var cybsHeaders = new Dictionary<string, object>
                {
                    { "v-c-merchant-id", merchantIdForJwt },
                    { "x5c", new List<string> { x5cBase64 } }
                };

                token = Jose.JWT.Encode(jwtBody, material.PrivateKey, Jose.JwsAlgorithm.RS256, cybsHeaders);

                try
                {
                    File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "jwsToken.txt"), token);
                }
                catch { /* ignore */ }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR : " + ex);
            }

            return token;
        }

        // ===== HTTP Signature (HMAC-SHA256) =====
        public static string GenerateDigest(string request)
        {
            try
            {
                var payloadBytes = SHA256.HashData(Encoding.UTF8.GetBytes(request ?? string.Empty));
                return "SHA-256=" + Convert.ToBase64String(payloadBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR : " + ex);
                return "SHA-256=" + Convert.ToBase64String(Array.Empty<byte>());
            }
        }

        public static StringBuilder GenerateSignature(string request, string digest, string keyid, string gmtDateTime, string method, string resource)
        {
            var signatureHeaderValue = new StringBuilder();
            const string algorithm = "HmacSHA256";
            const string postHdrs = "host date request-target digest v-c-merchant-id";
            const string getHdrs = "host date request-target v-c-merchant-id";

            try
            {
                bool isPost = string.Equals(method, "post", StringComparison.OrdinalIgnoreCase);

                var sb = new StringBuilder();
                sb.Append('\n');
                sb.Append("host: ").Append(requestHost).Append('\n');
                sb.Append("date: ").Append(gmtDateTime).Append('\n');
                sb.Append("request-target: ").Append(method).Append(' ').Append(resource);

                if (isPost)
                    sb.Append('\n').Append("digest: ").Append(digest);

                sb.Append('\n').Append("v-c-merchant-id: ");
                if (string.Equals(IsPortfolioCredential, "false", StringComparison.OrdinalIgnoreCase))
                    sb.Append(MerchantId);
                else
                    sb.Append(SignatureMerchantId);

                if (sb.Length > 0 && sb[0] == '\n')
                    sb.Remove(0, 1);

                byte[] signingBytes = Encoding.UTF8.GetBytes(sb.ToString());
                byte[] decodedKey = Convert.FromBase64String(merchantsecretKey!);

                using var hmac = new HMACSHA256(decodedKey);
                string sig = Convert.ToBase64String(hmac.ComputeHash(signingBytes));

                signatureHeaderValue
                    .Append("keyid=\"").Append(merchantKeyId).Append('"')
                    .Append(", algorithm=\"").Append(algorithm).Append('"')
                    .Append(", headers=\"").Append(isPost ? postHdrs : getHdrs).Append('"')
                    .Append(", signature=\"").Append(sig).Append('"');
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR : " + ex);
            }

            return signatureHeaderValue;
        }

        public static string SignJwsPayload(string jwsPayloadJson)
        {
            using var material = LoadSigningMaterialOrThrow();

            // New MLE JWT format (required Feb 2026+): kid = signing cert Subject serialNumber, typ = JWT.
            // kid is the same serial extraction used for the SJC cert kid in MleCredentials.
            string kid = MleCredentials.ExtractSerialFromSubject(material.PublicCert.Subject);
            Console.WriteLine($"[MLE] Signing cert Subject={material.PublicCert.Subject}, kid={kid}");

            var headers = new Dictionary<string, object>
            {
                { "kid", kid },
                { "typ", "JWT" }
            };

            var token = Jose.JWT.Encode(jwsPayloadJson, material.PrivateKey, Jose.JwsAlgorithm.RS256, headers);
            Console.WriteLine($"[MLE] Bearer (first 80): {token[..Math.Min(80, token.Length)]}");
            return token;
        }

        public static string ByteToString(byte[] buff)
        {
            var sb = new StringBuilder(buff.Length * 2);
            for (int i = 0; i < buff.Length; i++)
                sb.Append(buff[i].ToString("X2"));
            return sb.ToString();
        }

        // ===== Signing material loader (PEM preferred; PFX fallback) =====

        private sealed class SigningMaterial : IDisposable
        {
            public X509Certificate2 PublicCert { get; }
            public RSA PrivateKey { get; }

            public SigningMaterial(X509Certificate2 pub, RSA key)
            {
                PublicCert = pub;
                PrivateKey = key;
            }

            public void Dispose()
            {
                PrivateKey?.Dispose();
                PublicCert?.Dispose();
            }
        }

        private static SigningMaterial LoadSigningMaterialOrThrow()
        {
            // Try PEM mode first (Winhost friendly)
            var pem = TryLoadFromPem();
            if (pem is not null)
            {
                Console.WriteLine("[PEM] Using private.pem + public.cer");
                return pem;
            }

            // Fallback to PFX with EphemeralKeySet (no MachineKeySet on shared hosting)
            return LoadFromPfxEphemeral();
        }

        private static SigningMaterial? TryLoadFromPem()
        {
            // Resolve paths:
            var baseRes = Path.Combine(AppContext.BaseDirectory, "Resource");
            var privEnv = Environment.GetEnvironmentVariable("AuthCredentialFile__PrivatePemPath");
            var pubEnv = Environment.GetEnvironmentVariable("AuthCredentialFile__PublicCertPath");

            var privPath = ResolvePreferredPath(
                baseRes,
                privEnv,
                Path.Combine("restkeys", "private.pem"));

            var pubPath = ResolvePreferredPath(
                baseRes,
                pubEnv,
                Path.Combine("restkeys", "public.cer"));

            ResolvedPemPrivatePath = privPath;
            ResolvedPemPublicPath = pubPath;

            Console.WriteLine($"[PEM] PrivatePath={privPath} Exists={File.Exists(privPath)}");
            Console.WriteLine($"[PEM] PublicPath={pubPath} Exists={File.Exists(pubPath)}");

            if (!File.Exists(privPath) || !File.Exists(pubPath))
            {
                // Not using PEM mode
                return null;
            }

            try
            {
                // Public cert (for x5c)
                var pubCert = new X509Certificate2(pubPath);

                // Verify the PEM certificate identity matches the configured P12 credential.
                // The cert CN should match the P12 filename (without extension).
                var expectedName = !string.IsNullOrEmpty(P12JwtCredential)
                    ? Path.GetFileNameWithoutExtension(P12JwtCredential)
                    : null;

                if (!string.IsNullOrEmpty(expectedName))
                {
                    var certCn = pubCert.GetNameInfo(X509NameType.SimpleName, false) ?? string.Empty;
                    Console.WriteLine($"[PEM] Certificate CN={certCn}, Expected={expectedName}");

                    if (!string.Equals(certCn, expectedName, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"[PEM] WARNING: PEM certificate identity mismatch! " +
                            $"Certificate CN '{certCn}' does not match configured credential '{expectedName}'. " +
                            $"Falling back to P12 file. Replace the PEM/CER files in Resource/restkeys/ " +
                            $"with ones exported from {P12JwtCredential}.");
                        pubCert.Dispose();
                        return null;
                    }
                }

                // Private key (RSA) – encrypted or unencrypted PKCS#8
                string pemText = File.ReadAllText(privPath);
                var pass = (KeyPass ?? string.Empty).Trim();

                RSA rsa = RSA.Create();
                try
                {
                    if (!string.IsNullOrEmpty(pass))
                        rsa.ImportFromEncryptedPem(pemText, pass);
                    else
                        rsa.ImportFromPem(pemText);
                }
                catch
                {
                    // Some editors add UTF-8 BOM; try trimming
                    pemText = pemText.Trim();
                    if (!string.IsNullOrEmpty(pass))
                        rsa.ImportFromEncryptedPem(pemText, pass);
                    else
                        rsa.ImportFromPem(pemText);
                }

                // Quick sanity
                byte[] test = rsa.ExportRSAPublicKey(); // will throw if invalid
                Console.WriteLine($"[PEM] RSA public key bytes = {test.Length}");

                return new SigningMaterial(pubCert, rsa);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[PEM] Failed to load PEM materials: " + ex.Message);
                return null;
            }
        }

        private static SigningMaterial LoadFromPfxEphemeral()
        {
            if (string.IsNullOrWhiteSpace(P12JwtCredential))
                throw new InvalidOperationException("P12JwtCredential is not configured.");

            var baseDir = Path.Combine(AppContext.BaseDirectory, "Resource");
            var configured = P12JwtCredential!;
            var certPath = ResolveCertPath(baseDir, configured);
            ResolvedCertPath = certPath;

            var pass = (KeyPass ?? string.Empty).Trim();

            Console.WriteLine($"[PFX] Path={certPath} Exists={File.Exists(certPath)}");
            if (!File.Exists(certPath))
                throw new FileNotFoundException("P12 file not found.", certPath);

            byte[] pfx = File.ReadAllBytes(certPath);
            Console.WriteLine($"[PFX] Length={pfx.LongLength} SHA256={Convert.ToHexString(SHA256.HashData(pfx))}");
            Console.WriteLine($"[PFX] PassLen={pass.Length}");

            // Ephemeral only (shared hosting safe)
            var cert = new X509Certificate2(certPath, pass, X509KeyStorageFlags.EphemeralKeySet);
            if (!cert.HasPrivateKey)
            {
                cert.Dispose();
                throw new CryptographicException("Loaded PFX has no private key.");
            }

            var rsa = cert.GetRSAPrivateKey()
                      ?? throw new InvalidOperationException("PFX did not contain an RSA private key.");
            Console.WriteLine("[PFX] Loaded via FILE PATH + EphemeralKeySet");

            return new SigningMaterial(cert, rsa);
        }

        private static string ResolvePreferredPath(string baseResourceDir, string? envOverride, string relativeDefault)
        {
            if (!string.IsNullOrWhiteSpace(envOverride))
            {
                // Absolute?
                if (Path.IsPathRooted(envOverride))
                    return envOverride;

                // Treat as relative under Resource
                return Path.Combine(baseResourceDir, envOverride);
            }

            // Default under Resource
            return Path.Combine(baseResourceDir, relativeDefault);
        }

        private static string ResolveCertPath(string baseDir, string configured)
        {
            if (Path.IsPathRooted(configured))
                return configured;

            if (configured.Contains(Path.DirectorySeparatorChar) || configured.Contains('/'))
                return Path.Combine(baseDir, configured);

            return Path.Combine(baseDir, "certs", configured);
        }
    }
}
