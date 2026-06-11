using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CybsClass.Cybersource.Authentication
{
    public static class MleCredentials
    {
        public static RSA? SjcPublicKey { get; private set; }
        public static RSA? ResponseMlePrivateKey { get; private set; }
        public static RSA? LegacyMlePrivateKey { get; private set; }
        public static string? ResponseMleKid { get; private set; }
        public static string? SjcKid { get; private set; }

        private static readonly Dictionary<string, RSA> _keyRegistry = new(StringComparer.OrdinalIgnoreCase);

        // Returns the private RSA key registered under the given kid, or null if not found.
        public static RSA? GetDecryptionKey(string kid) =>
            _keyRegistry.TryGetValue(kid, out var key) ? key : null;

        public static void Initialize(string sjcCertPath, string responseMleKeyPath, string responseMleKid,
            string? responseMleKeyPass = null, string? legacyMlePrivateKeyPath = null, string? legacyMleKid = null)
        {
            ResponseMleKid = responseMleKid;

            var baseRes = Path.Combine(AppContext.BaseDirectory, "Resource");

            var resolvedSjcPath = Path.IsPathRooted(sjcCertPath)
                ? sjcCertPath
                : Path.Combine(baseRes, sjcCertPath);

            var resolvedKeyPath = Path.IsPathRooted(responseMleKeyPath)
                ? responseMleKeyPath
                : Path.Combine(baseRes, responseMleKeyPath);

            Console.WriteLine($"[MLE] SjcCertPath={resolvedSjcPath} Exists={File.Exists(resolvedSjcPath)}");
            Console.WriteLine($"[MLE] ResponseMleKeyPath={resolvedKeyPath} Exists={File.Exists(resolvedKeyPath)}");
            Console.WriteLine($"[MLE] ResponseMleKid={responseMleKid}");

            try
            {
                var sjcCert = new X509Certificate2(resolvedSjcPath);
                SjcPublicKey = sjcCert.GetRSAPublicKey();
                SjcKid = ExtractSerialFromSubject(sjcCert.Subject);
                Console.WriteLine($"[MLE] SJC cert loaded: {sjcCert.Subject}, valid until {sjcCert.NotAfter:yyyy-MM-dd}, SjcKid={SjcKid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MLE] ERROR loading SJC cert: {ex.Message}");
            }

            try
            {
                string? certThumbprint = null;

                if (resolvedKeyPath.EndsWith(".p12", StringComparison.OrdinalIgnoreCase))
                {
                    var mleCert = new X509Certificate2(resolvedKeyPath, responseMleKeyPass ?? string.Empty,
                        X509KeyStorageFlags.EphemeralKeySet);
                    ResponseMlePrivateKey = mleCert.GetRSAPrivateKey();

                    // Auto-extract kid from cert if not provided in config
                    if (string.IsNullOrEmpty(ResponseMleKid))
                        ResponseMleKid = ExtractSerialFromSubject(mleCert.Subject);

                    // CyberSource writes the SHA-1 thumbprint of our cert into the JWE response kid header,
                    // not the serial-number kid we advertised — register under both so lookup succeeds.
                    certThumbprint = mleCert.Thumbprint;

                    Console.WriteLine($"[MLE] Response MLE P12 loaded: {mleCert.Subject}, KeySize={ResponseMlePrivateKey?.KeySize}, Kid={ResponseMleKid}, Thumbprint={certThumbprint}");
                }
                else
                {
                    string pem = File.ReadAllText(resolvedKeyPath);
                    var rsa = RSA.Create();
                    rsa.ImportFromPem(pem);
                    ResponseMlePrivateKey = rsa;
                    Console.WriteLine($"[MLE] Response MLE private key loaded. KeySize={rsa.KeySize}");
                }

                if (!string.IsNullOrEmpty(ResponseMleKid) && ResponseMlePrivateKey != null)
                {
                    _keyRegistry[ResponseMleKid] = ResponseMlePrivateKey;
                    Console.WriteLine($"[MLE] Key registry: registered ResponseMleKid='{ResponseMleKid}'");
                }
                if (!string.IsNullOrEmpty(certThumbprint) && ResponseMlePrivateKey != null)
                {
                    _keyRegistry[certThumbprint] = ResponseMlePrivateKey;
                    Console.WriteLine($"[MLE] Key registry: registered cert thumbprint='{certThumbprint}'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MLE] ERROR loading response MLE private key: {ex.Message}");
            }

            if (!string.IsNullOrEmpty(legacyMlePrivateKeyPath))
            {
                var resolvedLegacyPath = Path.IsPathRooted(legacyMlePrivateKeyPath)
                    ? legacyMlePrivateKeyPath
                    : Path.Combine(baseRes, legacyMlePrivateKeyPath);

                Console.WriteLine($"[MLE] LegacyMlePrivateKeyPath={resolvedLegacyPath} Exists={File.Exists(resolvedLegacyPath)}");

                try
                {
                    string pem = File.ReadAllText(resolvedLegacyPath);
                    var rsa = RSA.Create();
                    rsa.ImportFromPem(pem);
                    LegacyMlePrivateKey = rsa;
                    Console.WriteLine($"[MLE] Legacy MLE private key loaded. KeySize={rsa.KeySize}");

                    if (!string.IsNullOrEmpty(legacyMleKid))
                    {
                        _keyRegistry[legacyMleKid] = rsa;
                        Console.WriteLine($"[MLE] Key registry: registered LegacyMleKid='{legacyMleKid}'");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[MLE] ERROR loading legacy MLE private key: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("[MLE] LegacyMlePrivateKeyPath not configured — legacy MLE decryption unavailable.");
            }
        }

        // Extract the serialNumber attribute from an X.509 Subject DN string.
        // e.g. "SERIALNUMBER=1763424170787064972884, CN=CyberSource_SJC_US" → "1763424170787064972884"
        public static string ExtractSerialFromSubject(string subjectDn)
        {
            var upper = subjectDn.ToUpperInvariant();
            const string prefix = "SERIALNUMBER=";
            var i = upper.IndexOf(prefix, StringComparison.Ordinal);
            if (i < 0) return string.Empty;
            var start = i + prefix.Length;
            var end = upper.IndexOf(',', start);
            if (end < 0) end = upper.Length;
            return subjectDn.Substring(start, end - start).Trim();
        }
    }
}
