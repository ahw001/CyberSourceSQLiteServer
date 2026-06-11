//using Org.BouncyCastle.Asn1.Ocsp;
using CybsClass.Cybersource.Authentication;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CybsClass.Cybersource.Transactions
{
    public static class CallCyberSource
    {

        // Try with your own credentaials
        // Get Key ID, Secret Key and Merchant Id from EBC portal

        private static string? signatureMerchantID;
        private static string? merchantID;
        private static string? requestHost;
        private static string? isPortfolioCredential;

        public static async Task<string> CallCyberSourceCaptureContext(string request, string resource)
        {
            Console.WriteLine("\n\n------------------ IN CallCyberSourceCaptureContext Method ------------------\n\n");

            TaskStatus responseCode;
            string responseContent = string.Empty;
            JsonObject? jsonObject = [];

            requestHost = Credentials.GetRequestHost();
            merchantID = Credentials.GetMerchantID();
            isPortfolioCredential = Credentials.GetIsPortfolioCredential();

            // HTTP POST request
            using (var client = new HttpClient())
            {

                StringContent content = new StringContent(request);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); // content-type header

                /* Add Request Header :: "v-c-merchant-id" set value to Cybersource Merchant ID or v-c-merchant-id
                 * This ID can be found on EBC portal
                 */
                client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantID);

                /* Add Request Header :: "Date" The date and time that the message was originated from.
                 * "HTTP-date" format as defined by RFC7231.
                 */
                string gmtDateTime = DateTime.Now.ToUniversalTime().ToString("r");
                client.DefaultRequestHeaders.Add("Date", gmtDateTime);

                /* Add Request Header :: "Host"
                 * Sandbox Host: apitest.cybersource.com
                 * Production Host: api.cybersource.com
                 */
                client.DefaultRequestHeaders.Add("Host", requestHost);

                /* Add Request Header :: "Digest"
                 * Digest is SHA-256 hash of payload that is BASE64 encoded
                 */
                var digest = Credentials.GenerateDigest(request);
                client.DefaultRequestHeaders.Add("Digest", digest);

                /* Add Request Header :: "Signature"
                 * Signature header contains keyId, algorithm, headers and signature as paramters
                 * method getSignatureHeader() has more details
                 */
                StringBuilder signature = Credentials.GenerateSignature(request, digest, string.Empty, gmtDateTime, "post", resource);
                client.DefaultRequestHeaders.Add("Signature", signature.ToString());

                Console.WriteLine("\n\nPOST call - CyberSource Payments API - Authorize a Credit Card");
                Console.WriteLine(" -- RequestURL -- ");
                Console.WriteLine("\tURL : " + "https://" + requestHost + resource);
                Console.WriteLine("\tMethod : POST");
                Console.WriteLine("\n -- HTTP Headers -- ");
                Console.WriteLine("\tv-c-merchant-id : " + merchantID);
                Console.WriteLine("\tDate : " + gmtDateTime);
                Console.WriteLine("\tHost : " + requestHost);
                Console.WriteLine("\tDigest : " + digest);
                Console.WriteLine("\tSignature : " + signature.ToString());

                Console.WriteLine("\n -- Request Payload --\n\n" + request);

                // POST FOR CYBERSOURCE TRANSACTION ****************************

                var response = await client.PostAsync("https://" + requestHost + resource, content);

                // POST FOR CYBERSOURCE TRANSACTION ****************************

                responseCode = (TaskStatus)response.StatusCode;
                responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("\n -- Capture Context Response Payload --\n\n" + responseContent);
            }
            return responseContent;
        }

        public static async Task<string> CallCyberSourceAPIFlex(string request, string resource)
        {
            TaskStatus responseCode;
            string responseContent = string.Empty;
            requestHost = Credentials.GetRequestHost();
            JsonObject? jsonObject = [];

            // HTTP POST request
            using (var client = new HttpClient())
            {

                Console.WriteLine("\n\nPOST call - CyberSource Flex API - Get a Flex Token");
                Console.WriteLine(" -- RequestURL -- ");
                Console.WriteLine("\tURL : " + "https://" + requestHost + resource);

                client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantID);

                string gmtDateTime = DateTime.Now.ToUniversalTime().ToString("r");
                client.DefaultRequestHeaders.Add("Date", gmtDateTime);

                var jwtToken = Credentials.GenerateJWT(request, "POST", false);

                StringContent content = new StringContent(request);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); // Content-Type header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                Console.WriteLine("\n -- Request Payload --\n\n" + request);

                var response = await client.PostAsync("https://" + requestHost + resource, content);
                responseCode = (TaskStatus)response.StatusCode;
                responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("\n\n------------------ RAW RESPONSE CONTENT--------------");
                Console.WriteLine(responseContent);
                Console.WriteLine("------------------ RAW RESPONSE CONTENT--------------\n\n");

                jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);

                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }
            ;
                var jsonString = JsonSerializer.Serialize(jsonObject, options);

                Console.WriteLine("\n -- Response Payload --\n\n" + jsonString);

            }

            return responseContent;
        }

        public static async Task<string> CallCyberSourceNtApiJson(string request, string resource)
        {
            Console.WriteLine("\n\n------------------ IN CallCyberSourceNtApiJson Method ------------------\n\n");

            merchantID = Credentials.GetMerchantID();
            //merchantKeyId = Credentials.GetMerchantKeyId();
            //merchantsecretKey = Credentials.GetMerchantsecretKey();
            requestHost = Credentials.GetRequestHost();
            TaskStatus responseCode;
            //NtDecodeDecrypt ntDecodeDecrypt = new NtDecodeDecrypt();
            string responseContent = string.Empty;
            JsonObject? jsonObject = [];

            // HTTP POST request
            using (var client = new HttpClient())
            {
                string jwtToken = Credentials.GenerateJWT(request, "POST", false);

                StringContent content = new(request);

                Console.WriteLine("\n\nPOST call - CyberSource Token-Flex API Call");
                Console.WriteLine(" -- RequestURL -- ");
                Console.WriteLine("\tURL : " + "https://" + requestHost + resource);

                /* Add Request Header :: "v-c-merchant-id" set value to Cybersource Merchant ID or v-c-merchant-id
                * This ID can be found on EBC portal.
                */

                client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantID);

                // Signal CyberSource to encrypt the NT response using our MLE key.
                // The kid must belong to the same account that signed the JWT (portfolio or merchant).
                if (!string.IsNullOrEmpty(MleCredentials.ResponseMleKid))
                    client.DefaultRequestHeaders.Add("v-c-merchant-mle-kid", MleCredentials.ResponseMleKid);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); // Content-Type header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                Console.WriteLine("\n -- Request Payload --\n\n" + request);

                var response = await client.PostAsync("https://" + requestHost + resource, content);

                responseCode = (TaskStatus)response.StatusCode;
                responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("\n\n------------------ RAW RESPONSE CONTENT--------------");
                Console.WriteLine(responseContent);
                Console.WriteLine("------------------ RAW RESPONSE CONTENT--------------\n\n");

                jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);

                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                var jsonString = JsonSerializer.Serialize(jsonObject, options);

                Console.WriteLine("\n -- Response Payload --\n\n" + jsonString);

            }

            return responseContent;
        }

        public static async Task<JsonObject> CallCyberSourceApiJson(string request, string resource, bool boardingAPI)
        {
            Console.WriteLine("\n\n------------------ IN CallCyberSourceApiJson Method ------------------\n\n");

            // USED FOR AUTH TRANSACTIONS

            signatureMerchantID = Credentials.GetSignatureMerchantId();
            isPortfolioCredential = Credentials.GetIsPortfolioCredential();
            merchantID = Credentials.GetMerchantID();
            requestHost = Credentials.GetRequestHost();
            TaskStatus responseCode;
            JsonObject? jsonObject = [];

            try
            {
                // HTTP POST request
                using (var client = new HttpClient())
                {
                    string jwtToken = Credentials.GenerateJWT(request, "POST", boardingAPI);

                    StringContent content = new StringContent(request);

                    Console.WriteLine($"\n\nPOST call - CyberSource API - {resource}");
                    Console.WriteLine("\n -- RequestURL -- ");
                    Console.WriteLine("\n\tURL : " + "https://" + requestHost + resource);

                    // REMOVE THESE LINES - v-c-merchant-id is already in the JWT
                    if (isPortfolioCredential == "true" && boardingAPI)
                    {
                        client.DefaultRequestHeaders.Add("v-c-merchant-id", signatureMerchantID);
                        Console.WriteLine("\n\t** v-c-merchant ID: " + signatureMerchantID);
                    }
                    else
                    {
                            client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantID);
                            Console.WriteLine("\n\t** v-c-merchant ID: " + merchantID);
                    }

                    // Log which merchant ID is being used (for debugging)
                    string activeMerchantId = (isPortfolioCredential == "true" && boardingAPI) 
                        ? signatureMerchantID 
                        : merchantID;
                    Console.WriteLine($"\n\t** v-c-merchant-id (in JWT): {activeMerchantId}");

                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                    client.Timeout = TimeSpan.FromMilliseconds(70000);

                    Console.WriteLine("\n -- Request Payload --\n\n" + request);

                    var response = await client.PostAsync("https://" + requestHost + resource, content);

                    responseCode = (TaskStatus)response.StatusCode;
                    string responseContent = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("\n\n------------------ RAW RESPONSE CONTENT--------------");
                    Console.WriteLine(responseContent);
                    Console.WriteLine("------------------ RAW RESPONSE CONTENT--------------\n\n");

                    Console.WriteLine("---------- RESPONSE HEADERS ----------");
                    Console.WriteLine($"Status: {(int)response.StatusCode} {response.ReasonPhrase}");
                    var responseHeaders = new Dictionary<string, string>();
                    foreach (var header in response.Headers)
                    {
                        var value = string.Join(", ", header.Value);
                        responseHeaders[header.Key] = value;
                        Console.WriteLine($"  {header.Key}: {value}");
                    }
                    foreach (var header in response.Content.Headers)
                    {
                        var value = string.Join(", ", header.Value);
                        responseHeaders[header.Key] = value;
                        Console.WriteLine($"  {header.Key}: {value}");
                    }
                    Console.WriteLine("--------------------------------------");

                    jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);

                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                    var jsonString = JsonSerializer.Serialize(jsonObject, options);

                    Console.WriteLine("\n -- Response Payload --\n" + jsonString);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                jsonObject = new JsonObject();
                jsonObject["error"] = e.Message;
                return jsonObject;
            }

            return jsonObject!;
        }

        public static async Task<JsonObject> CallCyberSourceApiGet(string resource, bool boardingAPI)
        {
            Console.WriteLine("\n\n------------------ IN CallCyberSourceApiGet Method ------------------\n\n");

            signatureMerchantID = Credentials.GetSignatureMerchantId();
            isPortfolioCredential = Credentials.GetIsPortfolioCredential();
            merchantID = Credentials.GetMerchantID();
            requestHost = Credentials.GetRequestHost();
            TaskStatus responseCode;
            JsonObject? jsonObject = [];

            string request = "";


            // HTTP GET request
            using (var client = new HttpClient())
            {

                /* Add Request Header :: "v-c-merchant-id" set value to Cybersource Merchant ID or v-c-merchant-id
                * This ID can be found on EBC portal.
                */
                if (isPortfolioCredential == "true" && boardingAPI)
                {
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", signatureMerchantID);
                }
                else
                {
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantID);
                }

                /* Add Request Header :: "Date" The date and time that the message was originated from.
                 * "HTTP-date" format as defined by RFC7231.
                 */
                string gmtDateTime = DateTime.Now.ToUniversalTime().ToString("r");
                client.DefaultRequestHeaders.Add("Date", gmtDateTime);

                /* Add Request Header :: "Host"
                 * Sandbox Host: apitest.cybersource.com
                 * Production Host: api.cybersource.com
                 */
                client.DefaultRequestHeaders.Add("Host", requestHost);

                /* Add Request Header :: "Signature"
                 * Signature header contains keyId, algorithm, headers and signature as paramters
                 * method getSignatureHeader() has more details
                 */
                StringBuilder signature = Credentials.GenerateSignature(request, string.Empty, string.Empty, gmtDateTime, "get", resource);
                client.DefaultRequestHeaders.Add("Signature", signature.ToString());

                Console.WriteLine("\nGET call - CyberSource Transient Token Data Retrieval");
                Console.WriteLine(" -- RequestURL -- ");
                Console.WriteLine("\tURL : " + "https://" + requestHost + resource);
                Console.WriteLine("\tMethod : GET");
                Console.WriteLine(" -- HTTP Headers -- ");
                if (isPortfolioCredential == "true" && boardingAPI)
                {
                    Console.WriteLine("\tv-c-merchant-id : " + signatureMerchantID);
                }
                else
                {
                    Console.WriteLine("\tv-c-merchant-id : " + merchantID);
                }
                Console.WriteLine("\tDate : " + gmtDateTime);
                Console.WriteLine("\tHost : " + requestHost);
                Console.WriteLine("\tSignature : " + signature.ToString());

                Console.WriteLine("\n -- Request Payload --\n\n" + request);

                /*********** CALL GET API TO RETRIEVE TRANS TOKEN *******************/

                var response = await client.GetAsync(new Uri("https://" + requestHost + resource));

                /*********** CALL GET API TO RETRIEVE TRANS TOKEN *******************/

                responseCode = (TaskStatus)response.StatusCode;
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("\n\n------------------ RAW RESPONSE CONTENT--------------");
                Console.WriteLine(responseContent);
                Console.WriteLine("------------------ RAW RESPONSE CONTENT--------------\n\n");

                jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent)!;

                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                var jsonString = JsonSerializer.Serialize(jsonObject, options);

                Console.WriteLine("\n -- Response Payload --\n\n" + jsonString);

            }

            return jsonObject;
        }

        public static async Task<JsonObject> CallCyberSourceApiDelete(string resource, bool boardingAPI)
        {
            Console.WriteLine("\n\n------------------ IN CallCyberSourceApiDelete Method ------------------\n\n");

            signatureMerchantID = Credentials.GetSignatureMerchantId();
            isPortfolioCredential = Credentials.GetIsPortfolioCredential();
            merchantID = Credentials.GetMerchantID();
            requestHost = Credentials.GetRequestHost();
            JsonObject? jsonObject = [];

            try
            {
                using var client = new HttpClient();
                string jwtToken = Credentials.GenerateJWT(string.Empty, "DELETE", boardingAPI);

                Console.WriteLine($"\n\nDELETE call - CyberSource API - {resource}");
                Console.WriteLine("\n -- RequestURL -- ");
                Console.WriteLine("\n\tURL : " + "https://" + requestHost + resource);

                if (isPortfolioCredential == "true" && boardingAPI)
                {
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", signatureMerchantID);
                    Console.WriteLine("\n\t** v-c-merchant ID: " + signatureMerchantID);
                }
                else
                {
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantID);
                    Console.WriteLine("\n\t** v-c-merchant ID: " + merchantID);
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                client.Timeout = TimeSpan.FromMilliseconds(70000);

                var response = await client.DeleteAsync("https://" + requestHost + resource);

                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("\n\n------------------ RAW RESPONSE CONTENT--------------");
                Console.WriteLine(responseContent);
                Console.WriteLine("------------------ RAW RESPONSE CONTENT--------------\n\n");

                Console.WriteLine("---------- RESPONSE HEADERS ----------");
                Console.WriteLine($"Status: {(int)response.StatusCode} {response.ReasonPhrase}");
                foreach (var header in response.Headers)
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                foreach (var header in response.Content.Headers)
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                Console.WriteLine("--------------------------------------");

                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    jsonObject = new JsonObject();
                    jsonObject["status"] = (int)response.StatusCode;
                }
                else
                {
                    jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                    Console.WriteLine("\n -- Response Payload --\n" + JsonSerializer.Serialize(jsonObject, options));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                jsonObject = new JsonObject();
                jsonObject["error"] = e.Message;
                return jsonObject;
            }

            return jsonObject!;
        }

        public static async Task<JsonObject> CallCyberSourceApiJsonPatch(string request, string resource, bool boardingAPI)
        {
            Console.WriteLine("\n\n------------------ IN CallCyberSourceApiJsonPatch Method ------------------\n\n");

            signatureMerchantID = Credentials.GetSignatureMerchantId();
            isPortfolioCredential = Credentials.GetIsPortfolioCredential();
            merchantID = Credentials.GetMerchantID();
            requestHost = Credentials.GetRequestHost();
            JsonObject? jsonObject = [];

            try
            {
                using var client = new HttpClient();
                string jwtToken = Credentials.GenerateJWT(request, "PATCH", boardingAPI);
                StringContent content = new StringContent(request);

                Console.WriteLine($"\n\nPATCH call - CyberSource API - {resource}");
                Console.WriteLine("\n -- RequestURL -- ");
                Console.WriteLine("\n\tURL : " + "https://" + requestHost + resource);

                if (isPortfolioCredential == "true" && boardingAPI)
                {
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", signatureMerchantID);
                    Console.WriteLine("\n\t** v-c-merchant ID: " + signatureMerchantID);
                }
                else
                {
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantID);
                    Console.WriteLine("\n\t** v-c-merchant ID: " + merchantID);
                }

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                client.Timeout = TimeSpan.FromMilliseconds(70000);

                Console.WriteLine("\n -- Request Payload --\n\n" + request);

                var response = await client.PatchAsync("https://" + requestHost + resource, content);

                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("\n\n------------------ RAW RESPONSE CONTENT--------------");
                Console.WriteLine(responseContent);
                Console.WriteLine("------------------ RAW RESPONSE CONTENT--------------\n\n");

                Console.WriteLine("---------- RESPONSE HEADERS ----------");
                Console.WriteLine($"Status: {(int)response.StatusCode} {response.ReasonPhrase}");
                foreach (var header in response.Headers)
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                foreach (var header in response.Content.Headers)
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                Console.WriteLine("--------------------------------------");

                jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);

                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                Console.WriteLine("\n -- Response Payload --\n" + JsonSerializer.Serialize(jsonObject, options));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                jsonObject = new JsonObject();
                jsonObject["error"] = e.Message;
                return jsonObject;
            }

            return jsonObject!;
        }

        public static async Task<JsonObject> CallCyberSourceApiJsonMle(string request, string resource, bool isResponseMle = true, bool boardingAPI = false)
        {

            Console.WriteLine("\n\n------------------ IN CallCyberSourceApiJsonMle Method ------------------\n\n");

            merchantID = Credentials.GetMerchantID();
            requestHost = Credentials.GetRequestHost();
            JsonObject? jsonObject = new JsonObject();

            try
            {
                var (bearerToken, encryptedBody) = MleJwtHelper.GenerateMleJwt(request, "POST", resource, isResponseMle);

                using var client = new HttpClient();
                var content = new StringContent(encryptedBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // v-c-merchant-id must always be the transacting MID, never the portfolio org ID.
                // Portfolio identity is conveyed via the x5c cert chain in the MLE bearer JWT —
                // sending the portfolio org ID in this header causes a 403 on NT-protected endpoints.
                // boardingAPI has no effect on the MLE header identity for this reason.
                client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantID);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                client.Timeout = TimeSpan.FromMilliseconds(70000);

                Console.WriteLine($"\n\nPOST (MLE) - CyberSource API - {resource}");
                Console.WriteLine($"\n -- RequestURL --\n\tURL: https://{requestHost}{resource}");

                Console.WriteLine($"\n -- JWT Payload --\n{DecodeJwtPayload(bearerToken)}");
                Console.WriteLine($"\n -- JSON Web Signature --\n{bearerToken}");

                Console.WriteLine("\n---------- REQUEST HEADERS ----------");
                foreach (var header in client.DefaultRequestHeaders)
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                Console.WriteLine($"  Content-Type: application/json");
                Console.WriteLine("-------------------------------------");

                Console.WriteLine($"\n -- Plaintext Request Payload --\n{request}");
                Console.WriteLine($"\n -- Encrypted Body (length: {encryptedBody.Length}) --\n{encryptedBody}");

                var response = await client.PostAsync($"https://{requestHost}{resource}", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("\n\n------------------ RAW RESPONSE CONTENT--------------");
                Console.WriteLine(responseContent);
                Console.WriteLine("------------------ RAW RESPONSE CONTENT--------------\n\n");

                Console.WriteLine("---------- RESPONSE HEADERS ----------");
                Console.WriteLine($"Status: {(int)response.StatusCode} {response.ReasonPhrase}");
                foreach (var header in response.Headers)
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                foreach (var header in response.Content.Headers)
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                Console.WriteLine("--------------------------------------");

                if (isResponseMle)
                {
                    using var doc = JsonDocument.Parse(responseContent);
                    if (doc.RootElement.TryGetProperty("encryptedResponse", out var encResp))
                    {
                        string jweCompact = encResp.GetString() ?? string.Empty;
                        Console.WriteLine($"[MLE] Decrypting encryptedResponse (length: {jweCompact.Length})");

                        var mleKey = MleCredentials.ResponseMlePrivateKey
                            ?? throw new InvalidOperationException("ResponseMlePrivateKey not initialized.");

                        string plainText = JoseJwtDecrypt.DecryptJweWithKey(jweCompact, mleKey);

                        Console.WriteLine($"\n -- Decrypted Response Payload --\n{plainText}");
                        jsonObject = JsonSerializer.Deserialize<JsonObject>(plainText);
                    }
                    else
                    {
                        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                        jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);
                        Console.WriteLine($"\n -- Response Payload (unencrypted) --\n{JsonSerializer.Serialize(jsonObject, options)}");
                    }
                }
                else
                {
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                    jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);
                    Console.WriteLine($"\n -- Response Payload (unencrypted) --\n{JsonSerializer.Serialize(jsonObject, options)}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                jsonObject = new JsonObject();
                jsonObject["error"] = e.Message;
                return jsonObject;
            }

            return jsonObject!;
        }

        public static async Task<JsonObject> CallCyberSourceApiJsonLegacyMle(string request, string resource, bool boardingAPI = false)
        {

            Console.WriteLine("\n\n------------------ IN CallCyberSourceApiJsonLegacyMle Method ------------------\n\n");

            // Legacy MLE: plain JSON request body + v-c-merchant-mle-kid header.
            // CyberSource encrypts the response as a raw JWE BLOB (not JSON-wrapped).
            // Uses standard JWT bearer auth (not MLE JWS), so boardingAPI flows through normally.
            signatureMerchantID = Credentials.GetSignatureMerchantId();
            isPortfolioCredential = Credentials.GetIsPortfolioCredential();
            merchantID = Credentials.GetMerchantID();
            requestHost = Credentials.GetRequestHost();
            JsonObject? jsonObject = new JsonObject();

            try
            {
                string jwtToken = Credentials.GenerateJWT(request, "POST", boardingAPI);

                using var client = new HttpClient();
                var content = new StringContent(request);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                if (isPortfolioCredential == "true" && boardingAPI)
                {
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", signatureMerchantID);
                    Console.WriteLine($"\n\t** v-c-merchant-id (boarding): {signatureMerchantID}");
                }
                else
                {
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantID);
                    Console.WriteLine($"\n\t** v-c-merchant-id: {merchantID}");
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                client.DefaultRequestHeaders.Add("v-c-merchant-mle-kid", MleCredentials.ResponseMleKid ?? string.Empty);
                client.Timeout = TimeSpan.FromMilliseconds(70000);

                Console.WriteLine($"\n\nPOST (Legacy MLE) - CyberSource API - {resource}");
                Console.WriteLine($"\n -- RequestURL --\n\tURL: https://{requestHost}{resource}");
                Console.WriteLine($"\n -- v-c-merchant-mle-kid: {MleCredentials.ResponseMleKid}");

                Console.WriteLine("\n---------- REQUEST HEADERS ----------");
                foreach (var header in client.DefaultRequestHeaders)
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                Console.WriteLine($"  Content-Type: application/json");
                Console.WriteLine("-------------------------------------");

                Console.WriteLine($"\n -- Request Payload --\n{request}");

                var response = await client.PostAsync($"https://{requestHost}{resource}", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("\n\n------------------ RAW RESPONSE CONTENT--------------");
                Console.WriteLine(responseContent);
                Console.WriteLine("------------------ RAW RESPONSE CONTENT--------------\n\n");

                Console.WriteLine("---------- RESPONSE HEADERS ----------");
                Console.WriteLine($"Status: {(int)response.StatusCode} {response.ReasonPhrase}");
                foreach (var header in response.Headers)
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                foreach (var header in response.Content.Headers)
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                Console.WriteLine("--------------------------------------");

                if (IsJweCompactToken(responseContent))
                {
                    Console.WriteLine($"[Legacy MLE] Decrypting JWE BLOB response (length: {responseContent.Length})");
                    var mleKey = MleCredentials.LegacyMlePrivateKey
                        ?? throw new InvalidOperationException("LegacyMlePrivateKey not initialized — check LegacyMlePrivateKeyPath in appsettings.json.");
                    string plainText = JoseJwtDecrypt.DecryptJweWithKey(responseContent.Trim(), mleKey);
                    Console.WriteLine($"\n -- Decrypted Legacy MLE Response --\n{plainText}");
                    jsonObject = JsonSerializer.Deserialize<JsonObject>(plainText);
                }
                else
                {
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                    jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);
                    Console.WriteLine($"\n -- Response Payload (unencrypted) --\n{JsonSerializer.Serialize(jsonObject, options)}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                jsonObject = new JsonObject();
                jsonObject["error"] = e.Message;
                return jsonObject;
            }

            return jsonObject!;
        }

        public static async Task<JsonObject> CallCyberSourceApiJsonApplicationJose(string request, string resource, bool boardingAPI = false)
        {

            Console.WriteLine("\n\n------------------ IN CallCyberSourceApiJsonApplicationJose Method ------------------\n\n");

            // Accept: application/jose flow — plain JSON request with standard JWT auth.
            // The Accept header tells CyberSource to return the response as a JWE BLOB containing
            // the full unmasked network token data. v-c-merchant-mle-kid identifies which public key
            // to use when encrypting the response.
            // Uses standard JWT bearer auth, so boardingAPI flows through normally.
            signatureMerchantID = Credentials.GetSignatureMerchantId();
            isPortfolioCredential = Credentials.GetIsPortfolioCredential();
            merchantID = Credentials.GetMerchantID();
            requestHost = Credentials.GetRequestHost();
            JsonObject? jsonObject = new JsonObject();

            try
            {
                string jwtToken = Credentials.GenerateJWT(request, "POST", boardingAPI);

                using var client = new HttpClient();
                var content = new StringContent(request);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                if (isPortfolioCredential == "true" && boardingAPI)
                {
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", signatureMerchantID);
                    Console.WriteLine($"\n\t** v-c-merchant-id (boarding): {signatureMerchantID}");
                }
                else
                {
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantID);
                    Console.WriteLine($"\n\t** v-c-merchant-id: {merchantID}");
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                client.DefaultRequestHeaders.Add("Accept", "application/jose");
                if (!string.IsNullOrEmpty(MleCredentials.ResponseMleKid))
                    client.DefaultRequestHeaders.Add("v-c-merchant-mle-kid", MleCredentials.ResponseMleKid);
                client.Timeout = TimeSpan.FromMilliseconds(70000);

                Console.WriteLine($"\n\nPOST (Accept: application/jose) - CyberSource API - {resource}");
                Console.WriteLine($"\n -- RequestURL --\n\tURL: https://{requestHost}{resource}");
                Console.WriteLine($"\n -- v-c-merchant-mle-kid: {MleCredentials.ResponseMleKid}");

                Console.WriteLine("\n---------- REQUEST HEADERS ----------");
                foreach (var header in client.DefaultRequestHeaders)
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                Console.WriteLine($"  Content-Type: application/json");
                Console.WriteLine("-------------------------------------");

                Console.WriteLine($"\n -- Request Payload --\n{request}");

                var response = await client.PostAsync($"https://{requestHost}{resource}", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("\n\n------------------ RAW RESPONSE CONTENT--------------");
                Console.WriteLine(responseContent);
                Console.WriteLine("------------------ RAW RESPONSE CONTENT--------------\n\n");

                Console.WriteLine("---------- RESPONSE HEADERS ----------");
                Console.WriteLine($"Status: {(int)response.StatusCode} {response.ReasonPhrase}");
                foreach (var header in response.Headers)
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                foreach (var header in response.Content.Headers)
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                Console.WriteLine("--------------------------------------");

                if (IsJweCompactToken(responseContent))
                {
                    Console.WriteLine($"[application/jose] Decrypting JWE BLOB response (length: {responseContent.Length})");
                    var mleKey = MleCredentials.LegacyMlePrivateKey
                        ?? throw new InvalidOperationException("LegacyMlePrivateKey not initialized — check LegacyMlePrivateKeyPath in appsettings.json.");
                    string plainText = JoseJwtDecrypt.DecryptJweWithKey(responseContent.Trim(), mleKey);
                    var joseOptions = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    Console.WriteLine($"\n -- Decrypted application/jose Response --\n{JsonSerializer.Serialize(JsonSerializer.Deserialize<JsonObject>(plainText), joseOptions)}");
                    jsonObject = JsonSerializer.Deserialize<JsonObject>(plainText);
                }
                else
                {
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                    jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);
                    Console.WriteLine($"\n -- Response Payload (unencrypted) --\n{JsonSerializer.Serialize(jsonObject, options)}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                jsonObject = new JsonObject();
                jsonObject["error"] = e.Message;
                return jsonObject;
            }

            return jsonObject!;
        }

        // Single consolidated sender for the two-axis encryption grid.
        // requestEncryption: "none" | "mle"
        // responseEncryption: "none" | "wrapped" | "blob"
        public static async Task<JsonObject> CallCyberSourceApiJsonMleGrid(
            string request, string resource,
            string requestEncryption, string responseEncryption,
            bool boardingAPI = false)
        {
            Console.WriteLine($"\n\n------------------ IN CallCyberSourceApiJsonMleGrid Method ------------------\n");
            Console.WriteLine($"RequestEncryption={requestEncryption}, ResponseEncryption={responseEncryption}, Resource={resource}, Boarding={boardingAPI}");

            if (requestEncryption == "none" && responseEncryption == "wrapped")
            {
                var unsupported = new JsonObject();
                var errObj = new JsonObject();
                errObj["error"] = "UnsupportedEncryptionCombination";
                errObj["message"] = "ResponseEncryption=wrapped with RequestEncryption=none is not implemented. Use mle+wrapped or none+blob instead.";
                unsupported["error"] = errObj;
                return unsupported;
            }

            signatureMerchantID = Credentials.GetSignatureMerchantId();
            isPortfolioCredential = Credentials.GetIsPortfolioCredential();
            merchantID = Credentials.GetMerchantID();
            requestHost = Credentials.GetRequestHost();
            JsonObject? jsonObject = new JsonObject();

            try
            {
                bool useMleBearer = requestEncryption == "mle";
                bool blobResponse = responseEncryption == "blob";
                bool wrappedResponse = responseEncryption == "wrapped";

                string bearerToken;
                string bodyToSend;

                if (useMleBearer)
                {
                    // MLE JWS bearer + encrypted body.
                    // isResponseMle=true only for wrapped — blob response is triggered by headers, not JWT claim.
                    var (mleBearer, encryptedBody) = MleJwtHelper.GenerateMleJwt(request, "POST", resource, isResponseMle: wrappedResponse);
                    bearerToken = mleBearer;
                    bodyToSend = encryptedBody;
                }
                else
                {
                    bearerToken = Credentials.GenerateJWT(request, "POST", boardingAPI);
                    bodyToSend = request;
                }

                using var client = new HttpClient();
                var content = new StringContent(bodyToSend);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // MLE bearer always uses transacting MID; standard JWT respects boardingAPI.
                if (useMleBearer)
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantID);
                else if (isPortfolioCredential == "true" && boardingAPI)
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", signatureMerchantID);
                else
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantID);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                if (blobResponse)
                {
                    // BLOB/Legacy MLE: Accept: application/jose signals we can handle the JWE compact response.
                    // v-c-merchant-mle-kid is NOT sent — that header belongs to request/response MLE only.
                    client.DefaultRequestHeaders.Add("Accept", "application/jose");
                }

                client.Timeout = TimeSpan.FromMilliseconds(70000);

                Console.WriteLine($"\n\nPOST (MleGrid) - CyberSource API - {resource}");
                Console.WriteLine($"\n -- RequestURL --\n\tURL: https://{requestHost}{resource}");
                Console.WriteLine("\n---------- REQUEST HEADERS ----------");
                foreach (var h in client.DefaultRequestHeaders)
                    Console.WriteLine($"  {h.Key}: {string.Join(", ", h.Value)}");
                Console.WriteLine($"  Content-Type: application/json");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine($"\n -- Request Body --\n{bodyToSend}");

                var response = await client.PostAsync($"https://{requestHost}{resource}", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("\n\n------------------ RAW RESPONSE CONTENT--------------");
                Console.WriteLine(responseContent);
                Console.WriteLine("------------------ RAW RESPONSE CONTENT--------------\n\n");
                Console.WriteLine("---------- RESPONSE HEADERS ----------");
                Console.WriteLine($"Status: {(int)response.StatusCode} {response.ReasonPhrase}");
                foreach (var h in response.Headers)
                    Console.WriteLine($"  {h.Key}: {string.Join(", ", h.Value)}");
                foreach (var h in response.Content.Headers)
                    Console.WriteLine($"  {h.Key}: {string.Join(", ", h.Value)}");
                Console.WriteLine("--------------------------------------");

                string? contentType = response.Content.Headers.ContentType?.MediaType;
                bool isActuallyBlob = contentType == "application/jose" || IsJweCompactToken(responseContent);

                var logOptions = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

                if (blobResponse && isActuallyBlob)
                {
                    // BLOB/Legacy MLE response is decrypted with LegacyMlePrivateKey (private.pem),
                    // not the request/response MLE key pair.
                    Console.WriteLine($"[MleGrid] Decrypting BLOB response with LegacyMlePrivateKey (length: {responseContent.Length})");
                    var blobKey = MleCredentials.LegacyMlePrivateKey
                        ?? throw new InvalidOperationException("LegacyMlePrivateKey not initialized — configure LegacyMlePrivateKeyPath in appsettings.json.");
                    string plainText = JoseJwtDecrypt.DecryptJweWithKey(responseContent.Trim(), blobKey);
                    if (string.IsNullOrEmpty(plainText))
                        throw new InvalidOperationException("JWE decryption returned empty — LegacyMlePrivateKey does not match the key used to encrypt this BLOB response.");
                    Console.WriteLine($"\n -- Decrypted BLOB Response --\n{plainText}");
                    jsonObject = JsonSerializer.Deserialize<JsonObject>(plainText);
                }
                else if (blobResponse && !isActuallyBlob)
                {
                    // CyberSource returned JSON (likely an error body) instead of a BLOB — pass through.
                    Console.WriteLine("[MleGrid] BLOB requested but response is JSON — passing through as-is.");
                    jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);
                    Console.WriteLine($"\n -- Response Payload --\n{JsonSerializer.Serialize(jsonObject, logOptions)}");
                }
                else if (wrappedResponse)
                {
                    using var doc = JsonDocument.Parse(responseContent);
                    if (doc.RootElement.TryGetProperty("encryptedResponse", out var encResp))
                    {
                        string jweCompact = encResp.GetString() ?? string.Empty;
                        Console.WriteLine($"[MleGrid] Decrypting encryptedResponse with ResponseMlePrivateKey (length: {jweCompact.Length})");
                        var wrappedKey = MleCredentials.ResponseMlePrivateKey
                            ?? throw new InvalidOperationException("ResponseMlePrivateKey not initialized — cannot decrypt wrapped response.");
                        string plainText = JoseJwtDecrypt.DecryptJweWithKey(jweCompact, wrappedKey);
                        if (string.IsNullOrEmpty(plainText))
                            throw new InvalidOperationException("JWE decryption returned empty — ResponseMlePrivateKey does not match the key used to encrypt the encryptedResponse.");
                        Console.WriteLine($"\n -- Decrypted Wrapped Response --\n{plainText}");
                        jsonObject = JsonSerializer.Deserialize<JsonObject>(plainText);
                    }
                    else
                    {
                        // encryptedResponse absent — surface as explicit error rather than silent fallback.
                        Console.WriteLine("[MleGrid] WARNING: encryptedResponse absent in wrapped-response mode.");
                        var errObj = new JsonObject();
                        errObj["error"] = "MissingEncryptedResponse";
                        errObj["message"] = "ResponseEncryption=wrapped was requested but the response does not contain encryptedResponse. Verify v-c-response-mle-kid configuration.";
                        jsonObject = new JsonObject();
                        jsonObject["error"] = errObj;
                    }
                }
                else
                {
                    jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);
                    Console.WriteLine($"\n -- Response Payload --\n{JsonSerializer.Serialize(jsonObject, logOptions)}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[MleGrid] ERROR: {e.Message}");
                jsonObject = new JsonObject();
                var errObj = new JsonObject();
                errObj["error"] = e.GetType().Name;
                errObj["message"] = e.Message;
                jsonObject["error"] = errObj;
            }

            return jsonObject!;
        }

        private static bool IsJweCompactToken(string value)
        {
            var trimmed = value.Trim();
            // JWE compact serialization has exactly 4 dots (5 Base64url parts).
            return trimmed.Count(c => c == '.') == 4 && !trimmed.StartsWith("{");
        }

        public static async Task<JsonObject> CallCyberSourceFollowOnJson(string request, string id, string transaction)
        {

            Console.WriteLine("\n\n------------------ IN CallCyberSourceFollowOnJson Method ------------------\n\n");

            // USED FOR CAPTURE, REVERSAL, VOID, CREDIT/RETURN

            signatureMerchantID = Credentials.GetSignatureMerchantId();
            isPortfolioCredential = Credentials.GetIsPortfolioCredential();
            merchantID = Credentials.GetMerchantID();
            requestHost = Credentials.GetRequestHost();
            bool boardingAPI = false;
            TaskStatus responseCode;
            JsonObject? jsonObject = [];

            string requestId = id;
            string resource = string.Empty;

            string jwtToken = Credentials.GenerateJWT(request, "POST", false);

            switch (transaction)
            {
                case "SHIPPING_ID_RETRIEVE":
                    resource = $"/tms/v2/customers/{id}/shipping-addresses";
                    break;
                case "CAPTURE":
                    resource = $"/pts/v2/payments/{id}/captures";
                    break;
                case "REVERSAL":
                    resource = $"/pts/v2/payments/{id}/reversals";
                    break;
                case "VOID_SALE":
                    resource = $"/pts/v2/payments/{id}/voids";
                    break;
                case "VOID_CAPTURE":
                    resource = $"/pts/v2/captures/{id}/voids";
                    break;
                case "VOID_CREDIT":
                    resource = $"/pts/v2/credits/{id}/voids";
                    break;
                case "VOID_REFUND":
                    resource = $"/pts/v2/refunds/{id}/voids";
                    break;
                case "REFUND_SALE":
                    resource = $"/pts/v2/payments/{id}/refunds";
                    break;
                case "REFUND_CAPTURE":
                    resource = $"/pts/v2/captures/{id}/refunds";
                    break;

            }

            try
            {
                // HTTP POST request
                using (var client = new HttpClient())
                {
                    JsonNode? jsonNode;
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

                    StringContent content = new StringContent(request);

                    Console.WriteLine("\n\nPOST call - CyberSource Follow On Transaction API Call");
                    Console.WriteLine(" -- RequestURL -- ");
                    Console.WriteLine("\tURL : " + "https://" + requestHost + resource);

                    if (isPortfolioCredential == "true" && boardingAPI)
                    {
                        client.DefaultRequestHeaders.Add("v-c-merchant-id", signatureMerchantID);
                        Console.WriteLine("\n\t** v-c-merchant ID: " + signatureMerchantID);
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantID);
                        Console.WriteLine("\n\t** v-c-merchant ID: " + merchantID);
                    }

                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); // Content-Type header
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                    //await Task.Delay(5000);

                    Console.WriteLine("\n -- Request Payload --\n" + request);

                    // POST FOR FOLLOW ON TRANSACTION ****************************

                    var response = await client.PostAsync("https://" + requestHost + resource, content);

                    // POST FOR FOLLOW ON TRANSACTION ****************************

                    responseCode = (TaskStatus)response.StatusCode;
                    string responsecontent = await response.Content.ReadAsStringAsync();

                    jsonNode = JsonSerializer.Deserialize<JsonNode>(responsecontent);

                    Console.WriteLine("\n\n------------------ RAW RESPONSE CONTENT--------------");
                    Console.WriteLine(responsecontent);
                    Console.WriteLine("------------------ RAW RESPONSE CONTENT--------------\n\n");


                    var jsonString = JsonSerializer.Serialize(jsonNode, options);

                    if (jsonString is not null)
                    {
                        Console.WriteLine("\n -- Response Payload --\n\n" + jsonString);
                    }


                    if ((jsonString is not null) && (jsonString.Contains("error", StringComparison.OrdinalIgnoreCase)))
                    {
                        Console.WriteLine("TRANSACTION WAS AN ERROR");
                    }

                    if (jsonString is not null)
                    {
                        JsonNode responseJsonNode = JsonNode.Parse(jsonString)!;
                        JsonNode idNode = responseJsonNode!["id"]!;

                        if (idNode == null)
                        {
                            Console.WriteLine("ID NODE IS NULL - TRANSACTION FAILED");
                        }
                        else
                        {
                            Console.WriteLine($"---------- JSON ID ={idNode.ToJsonString()} --------------");
                        }
                        jsonObject = JsonSerializer.Deserialize<JsonObject>(responsecontent)!;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR IN FOLLOW ON PROCESSING");
                Console.WriteLine(e.Message);
                JsonNode jsonNode = jsonObject;
                jsonNode["Exception"] = "ERROR IN FOLLOW ON PROCESSING" + e.Message;
                jsonObject = (JsonObject)jsonNode;
                return jsonObject;
            }
            return jsonObject;
        }

        private static string DecodeJwtPayload(string jwt)
        {
            try
            {
                var parts = jwt.Split('.');
                if (parts.Length < 2) return "(invalid JWT)";
                var seg = parts[1];
                var pad = (4 - seg.Length % 4) % 4;
                var b64 = seg.Replace('-', '+').Replace('_', '/') + new string('=', pad);
                return Encoding.UTF8.GetString(Convert.FromBase64String(b64));
            }
            catch { return "(decode error)"; }
        }
    }
}
