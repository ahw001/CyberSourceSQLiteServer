using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using Jose;
using System.Text.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CybsClass.Cybersource.Authentication;


namespace CybsClass.Cybersource.Transactions
{
    public static class JoseJwtDecrypt
    {
        // Decrypts a JWE compact token by reading the kid from its header and looking up
        // the matching private key in MleCredentials.GetDecryptionKey. Throws if kid is
        // unknown or decryption fails, so callers always receive a clear ErrorObject.
        public static string DecryptJweFromRegistry(string jweCompact)
        {
            string kid = ExtractKidFromJweHeader(jweCompact);
            Console.WriteLine($"[JWE] DecryptJweFromRegistry: kid='{kid}'");

            var key = MleCredentials.GetDecryptionKey(kid);
            if (key == null)
                throw new InvalidOperationException(
                    $"No decryption key registered for kid '{kid}'. " +
                    "Ensure ResponseMleKid in appsettings.json matches the certificate serial number.");

            string result = DecryptJweWithKey(jweCompact, key);
            if (string.IsNullOrEmpty(result))
                throw new InvalidOperationException(
                    $"JWE decryption returned empty for kid '{kid}'. " +
                    "The private key does not match the public key used to encrypt this response.");

            return result;
        }

        private static string ExtractKidFromJweHeader(string jweCompact)
        {
            try
            {
                var dot = jweCompact.IndexOf('.');
                if (dot < 0) return string.Empty;
                var headerB64 = jweCompact[..dot];
                var pad = (4 - headerB64.Length % 4) % 4;
                var b64 = headerB64.Replace('-', '+').Replace('_', '/') + new string('=', pad);
                var headerJson = Encoding.UTF8.GetString(Convert.FromBase64String(b64));
                using var doc = JsonDocument.Parse(headerJson);
                if (doc.RootElement.TryGetProperty("kid", out var kidEl))
                    return kidEl.GetString() ?? string.Empty;
            }
            catch { }
            return string.Empty;
        }


        public static async Task<string> DecryptJwt(string jweTokenBase64Url)
        {

            string keyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "../CybsClass.Cybersource/Resource/Keys");

            string rsaPrivateKey = string.Empty;

            try 
            {
                rsaPrivateKey = await File.ReadAllTextAsync($"{keyPath}/private.pem");
            } catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"READ PRIVATE ENCRYPTION KEY FILE FAILED: {ex}");
            }
            
            Console.WriteLine("privateKey loaded");

            Console.WriteLine("JWT JWE RSA-OAEP-256 AES GCM 256 encryption");
            // https://www.nuget.org/packages/jose-jwt/
            // https://github.com/dvsekhvalnov/jose-jwt



            Console.WriteLine("\n* * * decrypt the payload with recipient\'s private key * * *");


            string jweDecryptedPayload = jweRsaDecryptFromBase64UrlToken(rsaPrivateKey, jweTokenBase64Url);
            Console.WriteLine("jweDecryptedPayload: " + jweDecryptedPayload);

            return jweDecryptedPayload;


        }

        
        public static string jweRsaDecryptFromBase64UrlToken(string rsaPrivateKey, string jweTokenBase64Url)
        {
            RSA rsaAlg = RSA.Create();
            byte[] privateKeyByte = getRsaPrivateKeyEncodedFromPem(rsaPrivateKey);
            int _out;
            rsaAlg.ImportPkcs8PrivateKey(privateKeyByte, out _out);
            string json = "";
            try
            {
                json = Jose.JWT.Decode(jweTokenBase64Url, rsaAlg);
            }
            catch (Jose.EncryptionException)
            {
                Console.WriteLine("*** Error: payload corrupted or wrong private key ***");
                // throws: Jose.EncryptionException: Unable to decrypt content or authentication tag do not match.
            }
            return json;
        }


        static byte[] Base64Decoding(string input)
        {
            return Convert.FromBase64String(input);
        }

        public static string DecryptJweWithKey(string jweCompact, RSA privateKey)
        {
            try
            {
                return Jose.JWT.Decode(jweCompact, privateKey);
            }
            catch (Jose.EncryptionException)
            {
                Console.WriteLine("*** MLE DecryptJweWithKey: payload corrupted or wrong private key ***");
                return string.Empty;
            }
        }

        private static byte[] getRsaPrivateKeyEncodedFromPem(string rsaPrivateKeyPem)
        {
            string rsaPrivateKeyHeaderPem = "-----BEGIN PRIVATE KEY-----\r\n";
            string rsaPrivateKeyFooterPem = "-----END PRIVATE KEY-----";
            string rsaPrivateKeyDataPem = rsaPrivateKeyPem.Replace(rsaPrivateKeyHeaderPem, "").Replace(rsaPrivateKeyFooterPem, "").Replace("\n", "");
            return Base64Decoding(rsaPrivateKeyDataPem);
        }

    }

}
