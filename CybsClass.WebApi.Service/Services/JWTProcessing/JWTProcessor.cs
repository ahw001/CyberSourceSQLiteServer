using System;

namespace CybsClass.WebApi.Service.Services.JWTProcessing
{
    public static class JWTProcessor
    {
        public static string ExtractEncryptedPayload(string jwt)
        {
            var parts = jwt.Split('.');
            if (parts.Length != 5)
            {
                throw new ArgumentException("Invalid JWT/JWE token format.");
            }

            return parts[3]; // This is the encrypted payload (Ciphertext)
        }
    }

}
