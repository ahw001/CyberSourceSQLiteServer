using System;
using System.Security.Cryptography;
using System.Text;

namespace CybsClass.WebApi.Service.Services.GooglePay
{


    // Helper for HKDF (minimal implementation)
    public static class HKDF
    {
        public static byte[] DeriveKey(byte[] ikm, byte[] salt, byte[] info, int length)
        {
            using var hmac = new HMACSHA256(salt ?? new byte[32]);
            byte[] prk = hmac.ComputeHash(ikm);

            byte[] t = Array.Empty<byte>();
            byte[] okm = new byte[length];
            byte counter = 1;
            int pos = 0;
            while (pos < length)
            {
                hmac.Key = prk;
                hmac.Initialize();

                byte[] input = new byte[t.Length + info.Length + 1];
                Buffer.BlockCopy(t, 0, input, 0, t.Length);
                Buffer.BlockCopy(info, 0, input, t.Length, info.Length);
                input[^1] = counter++;

                t = hmac.ComputeHash(input);
                int copyLen = Math.Min(t.Length, length - pos);
                Buffer.BlockCopy(t, 0, okm, pos, copyLen);
                pos += copyLen;
            }
            return okm;
        }
    }

    public class GooglePayDecryptor
    {
        public static string Decrypt(
            string merchantPrivateKeyPem,
            string ephemeralPublicKeyBase64,
            string encryptedMessageBase64,
            string tagBase64)
        {
            // 1. Load EC private key
            ECDiffieHellman merchantEcdh;
            using (var ec = ECDsa.Create())
            {
                ec.ImportFromPem(merchantPrivateKeyPem);
                // This hack lets us get the key in ECDiffieHellman (not ECDsa) form:
                merchantEcdh = ECDiffieHellman.Create();
                merchantEcdh.ImportParameters(ec.ExportParameters(true));
            }

            // 2. Import ephemeral public key
            var ephemeralPubKeyBytes = Convert.FromBase64String(ephemeralPublicKeyBase64);
            var ephemeralEcdh = ECDiffieHellman.Create();
            ephemeralEcdh.ImportSubjectPublicKeyInfo(ephemeralPubKeyBytes, out _);

            // 3. ECDH: Derive shared secret
            var sharedSecret = merchantEcdh.DeriveKeyFromHash(
                ephemeralEcdh.PublicKey,
                HashAlgorithmName.SHA256,
                null, null);

            // 4. HKDF to derive AES-256 key
            var info = Encoding.UTF8.GetBytes("Google");
            var salt = new byte[32]; // All zeros
            var aesKey = HKDF.DeriveKey(sharedSecret, salt, info, 32);

            // 5. AES-GCM decrypt
            var encrypted = Convert.FromBase64String(encryptedMessageBase64);
            var tag = Convert.FromBase64String(tagBase64);
            var cipherTextWithTag = new byte[encrypted.Length + tag.Length];
            Buffer.BlockCopy(encrypted, 0, cipherTextWithTag, 0, encrypted.Length);
            Buffer.BlockCopy(tag, 0, cipherTextWithTag, encrypted.Length, tag.Length);

            // Google Pay uses a 12-byte IV of all zeros
            var iv = new byte[12];

            var plaintext = new byte[encrypted.Length];
            using var aesGcm = new AesGcm(aesKey, tag.Length);
            aesGcm.Decrypt(
                nonce: iv,
                ciphertext: encrypted,
                tag: tag,
                plaintext: plaintext
            );

            return Encoding.UTF8.GetString(plaintext);
        }
    }
}

/*
 
 string decrypted = GooglePayDecryptor.Decrypt(
    merchantPrivateKeyPem: File.ReadAllText("your-private-key.pem"),
    ephemeralPublicKeyBase64: "BASE64_FROM_PAYLOAD",
    encryptedMessageBase64: "BASE64_FROM_PAYLOAD",
    tagBase64: "BASE64_FROM_PAYLOAD"
);

Console.WriteLine(decrypted);

 
 */
