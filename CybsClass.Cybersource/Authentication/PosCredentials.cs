using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CybsClass.Cybersource.Authentication
{
    public static class PosCredentials
    {
        public static string? merchantKeyId { get; set; }
        public static string? merchantsecretKey { get; set; }

        public static string? acceptanceMid { get; set; }
        public static string? acceptanceSecretKey { get; set; }
        public static string? posMerchantKeyId { get; set; }
        public static string? posMerchantsecretKey { get; set; }
        public static string? requestHost { get; private set; }
        public static string? posRequestHost { get; private set; }
        public static string? P12JwtCredential { get; private set; }
        public static string? MerchantId { get; private set; }
        public static string? SignatureMerchantId { get; private set; }
        public static string? KeyPass { get; private set; }
        public static string? WebServiceUrlAddress { get; private set; }
        public static string? IsPortfolioCredential { get; private set; }

        public static string? TerminalSerialNumber { get; private set; }

        public static string GetMerchantID() { return MerchantId!; }

        public static string GetRequestHost() { return requestHost!; }

        public static string GetAcceptanceMid() { return acceptanceMid!; }
        public static string GetAcceptanceSecretKey() { return acceptanceSecretKey!; }

        public static string GetPosRequestHost() { return posRequestHost!; }

        public static string GetMerchantKeyId() { return merchantKeyId!; }
        public static string GetMerchantSecretKey() { return merchantsecretKey!; }

        public static string GetPosMerchantKeyId() { return posMerchantKeyId!; }
        public static string GetPosMerchantSecretKey() { return posMerchantsecretKey!; }

        public static string GetIsPortfolioCredential() { return IsPortfolioCredential!; }

        public static string? GetSignatureMerchantId() { return SignatureMerchantId; }

        public static string? GetAcceptanceDeviceSerialNumber() { return TerminalSerialNumber; }

        public static void Initialize(string p12JwtCredential, string isPortfolioCredential, string merchantID, string keyPass, string keyId, 
            string sharedSecret, string baseUrlAddress, 
            string basePosUrlAddress, string acceptanceConfigMid, string accpetanceSecret, string accepanceSerialNumber)
        {
            P12JwtCredential = p12JwtCredential;
            posMerchantKeyId = keyId;
            posMerchantsecretKey = sharedSecret;
            requestHost = baseUrlAddress;
            posRequestHost = basePosUrlAddress;
            acceptanceMid = acceptanceConfigMid;
            acceptanceSecretKey = accpetanceSecret;
            IsPortfolioCredential = isPortfolioCredential ?? null;
            TerminalSerialNumber = accepanceSerialNumber;

            KeyPass = keyPass;

            string fileName = p12JwtCredential;
            string extensionToRemove = ".p12";

            if (merchantID is not null && IsPortfolioCredential == "false")
            {
                MerchantId = merchantID;
                SignatureMerchantId = merchantID;
            }
            else if (merchantID is not null && IsPortfolioCredential == "true")
            {
                MerchantId = merchantID;
                SignatureMerchantId = fileName[..^extensionToRemove.Length];
                Console.WriteLine("P12 Merchant Id From appsettings.json: " + SignatureMerchantId);
            }
        
        }

        /// <summary>
        /// This method demonstrates the creation of the JWT Authentication credential
        /// Takes Request Paylaod and Http method(GET/POST) as input.
        /// </summary>
        /// <param name="request">Value from which to generate JWT</param>
        /// <param name="method">The HTTP Verb that is needed for generating the credential</param>
        /// <returns>String containing the JWT Authentication credential</returns>
        public static string GenerateJWT(string request, string method, bool boardingAPI)
        {
            string digest;
            string token = "TOKEN_PLACEHOLDER";
            var cybsHeaders = new Dictionary<string, object>();

            try
            {
                cybsHeaders = new();

                // Generate the hash for the payload
                byte[] payloadBytes = SHA256.HashData(Encoding.ASCII.GetBytes(request));
                digest = Convert.ToBase64String(payloadBytes);

                Console.WriteLine("\tMethod : " + method);

                // Create the JWT payload (aka claimset / JWTBody)
                string jwtBody = "0";

                if (method.Equals("POST"))
                {
                    jwtBody = "{\n\"digest\":\"" + digest + "\", \"digestAlgorithm\":\"SHA-256\", \"iat\":\"" + DateTime.Now.ToUniversalTime().ToString("r") + "\"}";
                }
                else if (method.Equals("GET"))
                {
                    jwtBody = "{\"iat\":\"" + DateTime.Now.ToUniversalTime().ToString("r") + "\"}";
                }

                Console.WriteLine("\tJWT BODY : " + jwtBody);

                // P12 certificate public key is sent in the header and the private key is used to sign the token

                // In order to see the actual Path you have to use Path.GetFullPath()
                Console.WriteLine("Path to file: " + Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "../CybsClass.Cybersource/Resource")));


                X509Certificate2 x5Cert = new X509Certificate2(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "../CybsClass.Cybersource/Resource", P12JwtCredential!), KeyPass, X509KeyStorageFlags.MachineKeySet);

                // Extracting Public Key from .p12 file
                string x5cPublicKey = Convert.ToBase64String(x5Cert.RawData);

                // Extracting Private Key from .p12 file
                var privateKey = x5Cert.GetRSAPrivateKey();

                // Extracting serialNumber
                string serialNumber = null!;
                string serialNumberPrefix = "SERIALNUMBER=";

                string principal = x5Cert.Subject;

                int beg = principal.IndexOf(serialNumberPrefix);
                if (beg >= 0)
                {
                    int x5cBase64List = principal.IndexOf(",", beg);
                    if (x5cBase64List == -1)
                    {
                        x5cBase64List = principal.Length;
                    }

                    serialNumber = principal[serialNumberPrefix.Length..x5cBase64List];
                }

                // Create the JWT Header custom fields
                var x5cList = new List<string>()
                {
                    x5cPublicKey
                };

                if (boardingAPI)
                {
                    cybsHeaders = new Dictionary<string, object>()
                    {
                        { "v-c-merchant-id", SignatureMerchantId! },
                        { "x5c", x5cList }
                    };
                }
                else
                {
                    cybsHeaders = new Dictionary<string, object>()
                    {
                        { "v-c-merchant-id", MerchantId! },
                        { "x5c", x5cList }
                    };
                }


                // JWT token is Header plus the Body plus the Signature of the Header & Body
                // Here the Jose-JWT helper library (https://github.com/dvsekhvalnov/jose-jwt) is used create the JWT
                token = Jose.JWT.Encode(jwtBody, privateKey, Jose.JwsAlgorithm.RS256, cybsHeaders);

                // Writing Generated Token to file.
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"jwsToken.txt"), token);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR : " + ex.ToString());
            }

            return token;
        }

        public static string GenerateDigest(string request)
        {
            string digest = "DIGEST_PLACEHOLDER";

            try
            {
                // Generate the Digest
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] payloadBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(request));
                    digest = Convert.ToBase64String(payloadBytes);
                    digest = "SHA-256=" + digest;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR : " + ex.ToString());
            }

            return digest;
        }

        public static StringBuilder GenerateSignature(string request, string digest, string keyid, string gmtDateTime, string method, string resource)
        {
            StringBuilder signatureHeaderValue = new StringBuilder();
            string algorithm = "HmacSHA256";
            string postHeaders = "host date request-target digest v-c-merchant-id";
            string getHeaders = "host date request-target v-c-merchant-id";
            string url = "https://" + posRequestHost + resource;
            string getRequestTarget = method + " " + resource;
            string postRequestTarget = method + " " + resource;

            try
            {
                // Generate the Signature
                StringBuilder signatureString = new StringBuilder();
                signatureString.Append('\n');
                signatureString.Append("host");
                signatureString.Append(": ");
                signatureString.Append(posRequestHost);
                signatureString.Append('\n');
                signatureString.Append("date");
                signatureString.Append(": ");
                signatureString.Append(gmtDateTime);
                signatureString.Append('\n');
                signatureString.Append("request-target");
                signatureString.Append(": ");

                if (method.Equals("post"))
                {
                    signatureString.Append(postRequestTarget);
                    signatureString.Append('\n');
                    signatureString.Append("digest");
                    signatureString.Append(": ");
                    signatureString.Append(digest);
                }
                else
                {
                    signatureString.Append(getRequestTarget);
                }

                signatureString.Append('\n');
                signatureString.Append("v-c-merchant-id");
                signatureString.Append(": ");
                signatureString.Append(MerchantId);

                /*
                if (IsPortfolioCredential == "false")
                { 
                    signatureString.Append(MerchantId);
                }
                else
                {
                    signatureString.Append(SignatureMerchantId);
                }
                */

                signatureString.Remove(0, 1);

                byte[] signatureByteString = Encoding.UTF8.GetBytes(signatureString.ToString());

                byte[] decodedKey = Convert.FromBase64String(posMerchantsecretKey!);

                HMACSHA256 aKeyId = new HMACSHA256(decodedKey);

                byte[] hashmessage = aKeyId.ComputeHash(signatureByteString);
                string base64EncodedSignature = Convert.ToBase64String(hashmessage);

                signatureHeaderValue.Append("keyid=\"" + posMerchantKeyId + "\"");
                signatureHeaderValue.Append(", algorithm=\"" + algorithm + "\"");

                if (method.Equals("post"))
                {
                    signatureHeaderValue.Append(", headers=\"" + postHeaders + "\"");
                }
                else if (method.Equals("get"))
                {
                    signatureHeaderValue.Append(", headers=\"" + getHeaders + "\"");
                }

                signatureHeaderValue.Append(", signature=\"" + base64EncodedSignature + "\"");

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR : " + ex.ToString());
            }

            return signatureHeaderValue;
        }

        // Converts byte to encrypted string
        public static string ByteToString(byte[] buff)
        {
            string sbinary = string.Empty;

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }

            return sbinary;
        }

    }
}
