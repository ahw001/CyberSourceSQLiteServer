using CybsClass.Cybersource.Authentication;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CybsClass.Cybersource.Transactions
{
    public static class CallCyberSourcePos
    {

        private static string? signatureMerchantID;
        private static string? posMerchantID;
        private static string? requestHost;
        private static string? posRequestHost;
        private static string? isPortfolioCredential;

        public static async Task<JsonObject> CallCyberSourceSecretPosApiJson(string request, string resource)
        {
            TaskStatus responseCode;
            string responseContent = string.Empty;
            JsonObject? jsonObject = [];

            posRequestHost = PosCredentials.GetPosRequestHost();
            posMerchantID = PosCredentials.GetMerchantID();
            isPortfolioCredential = PosCredentials.GetIsPortfolioCredential();

            if (string.IsNullOrWhiteSpace(posRequestHost))
            {
                throw new InvalidOperationException("POS request host is not configured. Ensure PosCredentials.Initialize is called with a valid basePosUrlAddress.");
            }

            if (string.IsNullOrWhiteSpace(posMerchantID))
            {
                throw new InvalidOperationException("POS merchant ID is not configured. Ensure PosCredentials.Initialize is called with a valid merchantID.");
            }

            // HTTP POST request
            using (var client = new HttpClient())
            {

                StringContent content = new StringContent(request);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); // content-type header

                /* Add Request Header :: "v-c-merchant-id" set value to Cybersource Merchant ID or v-c-merchant-id
                 * This ID can be found on EBC portal
                 */
                client.DefaultRequestHeaders.Add("v-c-merchant-id", posMerchantID);

                /* Add Request Header :: "Date" The date and time that the message was originated from.
                 * "HTTP-date" format as defined by RFC7231.
                 */
                string gmtDateTime = DateTime.Now.ToUniversalTime().ToString("r");
                client.DefaultRequestHeaders.Add("Date", gmtDateTime);

                /* Add Request Header :: "Host"
                 * Sandbox Host: apitest.cybersource.com
                 * Production Host: api.cybersource.com
                 */
                client.DefaultRequestHeaders.Add("Host", posRequestHost);

                /* Add Request Header :: "Digest"
                 * Digest is SHA-256 hash of payload that is BASE64 encoded
                 */
                var digest = Credentials.GenerateDigest(request);
                client.DefaultRequestHeaders.Add("Digest", digest);

                /* Add Request Header :: "Signature"
                 * Signature header contains keyId, algorithm, headers and signature as paramters
                 * method getSignatureHeader() has more details
                 */
                StringBuilder signature = PosCredentials.GenerateSignature(request, digest, string.Empty, gmtDateTime, "post", resource);
                client.DefaultRequestHeaders.Add("Signature", signature.ToString());

                Console.WriteLine("\n\nPOST call - CyberSource REST API - POS Operations");
                Console.WriteLine(" -- RequestURL -- ");
                Console.WriteLine("\tURL : " + "https://" + posRequestHost + resource);
                Console.WriteLine("\tMethod : POST");
                Console.WriteLine("\n -- HTTP Headers -- ");
                Console.WriteLine("\tv-c-merchant-id : " + posMerchantID);
                Console.WriteLine("\tDate : " + gmtDateTime);
                Console.WriteLine("\tHost : " + posRequestHost);
                Console.WriteLine("\tDigest : " + digest);
                Console.WriteLine("\tSignature : " + signature.ToString());

                Console.WriteLine(" -- Request Payload --\n" + request);

                var response = await client.PostAsync("https://" + posRequestHost + resource, content);
                responseCode = (TaskStatus)response.StatusCode;
                responseContent = await response.Content.ReadAsStringAsync();

                if (responseContent is not null)
                {
                    jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);

                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    var jsonString = JsonSerializer.Serialize(jsonObject, options);

                    Console.WriteLine("\n -- Response Payload --\n\n" + jsonString);
                }
                else
                {
                    responseContent = "No response content available - no bearer token created.";
                    jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);
                }

            }
            return jsonObject!;
        }

        public static async Task<JsonObject> CallCyberSourceJwtPosApiJson(string request, string resource, string bearerToken)
        {
            // USED FOR AUTH TRANSACTIONS

            isPortfolioCredential = Credentials.GetIsPortfolioCredential();
            posMerchantID = PosCredentials.GetMerchantID();
            requestHost = PosCredentials.GetPosRequestHost();
            TaskStatus responseCode;
            JsonObject? jsonObject = [];

            try
            {
                // HTTP POST request
                using (var client = new HttpClient())
                {
                    string jwtToken = bearerToken;

                    StringContent content = new StringContent(request);

                    Console.WriteLine($"\n\nCloud POS - POST call - CyberSource API - {resource}");
                    Console.WriteLine(" -- RequestURL -- ");
                    Console.WriteLine("\tURL : " + "https://" + requestHost + resource);

                    Console.WriteLine(" -- HTTP Headers -- \n");
                    Console.WriteLine("\tMediaTypeHeaderValue: application/json");
                    client.DefaultRequestHeaders.Add("v-c-merchant-id", posMerchantID);


                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); // Content-Type header
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                    //Console.WriteLine("\nAuthenticationHeaderValue : " + client.DefaultRequestHeaders.Authorization.ToString());

                    client.Timeout = TimeSpan.FromMilliseconds(70000);

                    Console.WriteLine("\n -- Request Payload --\n" + request);

                    // POST FOR CYBERSOURCE TRANSACTION ****************************

                    var response = await client.PostAsync("https://" + requestHost + resource, content);

                    // POST FOR CYBERSOURCE TRANSACTION ****************************

                    responseCode = (TaskStatus)response.StatusCode;
                    string responseContent = await response.Content.ReadAsStringAsync();

                    jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);

                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    var jsonString = JsonSerializer.Serialize(jsonObject, options);

                    
                    if (jsonString.Contains("error", StringComparison.OrdinalIgnoreCase) || jsonString.Contains("INVALID"))
                    {
                        Console.WriteLine("TRANSACTION WAS AN ERROR");
                        Console.WriteLine($"CLOUD POS RESPONSE:\n {jsonObject}");
                    }
                    else
                    {
                        Console.WriteLine("\n -- Response Payload --\n\n" + jsonString);
                    }

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

    }
}
