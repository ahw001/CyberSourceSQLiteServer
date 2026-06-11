using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CybsClass.Cybersource.Authentication
{
    public static class MleJwtHelper
    {
        public static string EncryptPayloadToJwe(string jsonPayload)
        {
            var publicKey = MleCredentials.SjcPublicKey
                ?? throw new InvalidOperationException("MleCredentials not initialized — SjcPublicKey is null.");

            // JWE header requires kid, cty, and iat (epoch seconds) per CyberSource SDK behaviour.
            var jweHeaders = new Dictionary<string, object>
            {
                { "kid", MleCredentials.SjcKid ?? string.Empty },
                { "cty", "JWT" }, // <--------------- Removed cty because this indicates the response should be parsed as a JWT, but the response MAY NOT BE a JWT (it may be a JWE or plaintext JSON depending on the endpoint and CyberSource's implementation). The cty header is optional per RFC7516, so we can omit it to avoid confusion.
                { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
            };

            // Must pass byte[] — the String overload in jose-jwt 5.3.0 still routes through
            // JsonMapper.Serialize() which JSON-encodes the string (adds outer quotes + escapes).
            // Passing byte[] hits the payload-is-byte[] fast-path that uses raw bytes directly.
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(jsonPayload);
            return Jose.JWT.EncodeBytes(
                plaintextBytes,
                publicKey,
                Jose.JweAlgorithm.RSA_OAEP_256,
                Jose.JweEncryption.A256GCM,
                null,
                jweHeaders);
        }

        public static (string bearerToken, string encryptedBody) GenerateMleJwt(
            string jsonPayload, string method, string resource, bool isResponseMle = true)
        {
            string jweCompact = EncryptPayloadToJwe(jsonPayload);

            string encryptedBody = JsonSerializer.Serialize(new { encryptedRequest = jweCompact });

            string digest = Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(encryptedBody)));

            var now = DateTimeOffset.UtcNow;

            // iss must equal the identity of the signing cert — i.e. the portfolio org ID when
            // using a portfolio P12, or the merchant ID when using a direct merchant P12.
            // CyberSource validates iss against the cert owner looked up via the kid in the JWT header;
            // a mismatch between iss and cert owner returns 401 UNAUTHORIZED_USER.
            //
            // v-c-merchant-id is separate: it tells CyberSource which *transacting* submerchant to
            // act on behalf of, matching the same header sent in the HTTP request.
            string issuer = Credentials.GetSignatureMerchantId() ?? Credentials.GetMerchantID();
            string transactingMid = Credentials.GetMerchantID();

            string jwsPayloadJson = BuildJwsPayloadJson(
                digest,
                now.ToUnixTimeSeconds(),
                now.AddSeconds(120).ToUnixTimeSeconds(),
                Guid.NewGuid().ToString(),
                Credentials.GetRequestHost(),
                method,
                resource,
                issuer,
                transactingMid,
                MleCredentials.ResponseMleKid ?? string.Empty,
                isResponseMle);

            string bearerToken = Credentials.SignJwsPayload(jwsPayloadJson);
            return (bearerToken, encryptedBody);
        }

        private static string BuildJwsPayloadJson(
            string digest, long iat, long exp, string jti,
            string requestHost, string method, string resource,
            string issuer, string merchantId, string responseMleKid, bool isResponseMle)
        {
            using var ms = new MemoryStream();
            using var writer = new Utf8JsonWriter(ms);
            writer.WriteStartObject();
            writer.WriteString("digest", digest);
            writer.WriteString("digest-algorithm", "SHA-256");
            writer.WriteNumber("iat", iat);
            writer.WriteNumber("exp", exp);
            writer.WriteString("iss", issuer);         // must match the signing cert owner (portfolio org or merchant)
            writer.WriteString("jti", jti);
            writer.WriteString("request-host", requestHost);
            writer.WriteString("request-method", method.ToLowerInvariant());
            writer.WriteString("request-resource-path", resource);
            writer.WriteString("v-c-jwt-version", "2");
            writer.WriteString("v-c-merchant-id", merchantId); // transacting submerchant (may differ from iss in portfolio mode)
            if (isResponseMle)
                writer.WriteString("v-c-response-mle-kid", responseMleKid);
            writer.WriteEndObject();
            writer.Flush();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
