//using Org.BouncyCastle.Asn1.Ocsp;
using CybsClass.Cybersource.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CybsClass.Cybersource.Transactions
{
    public static class CallCyberSourceBoarding
    {

        // Try with your own credentaials
        // Get Key ID, Secret Key and Merchant Id from EBC portal

        private static string? signatureMerchantID;
        private static string? merchantID;
        private static string? requestHost;
        private static string? isPortfolioCredential;

        public static async Task<JsonObject> CallCybsApiJsonJwtGET(string resource, bool boardingAPI)
        {
            // USED FOR AUTH TRANSACTIONS

            signatureMerchantID = BoardingCredentials.GetSignatureMerchantId();
            isPortfolioCredential = BoardingCredentials.GetIsPortfolioCredential();
            //merchantID = BoardingCredentials.GetMerchantID();
            requestHost = BoardingCredentials.GetRequestHost();
            TaskStatus responseCode;
            JsonObject? jsonObject = [];

            try
            {
                // HTTP GET request
                using (var client = new HttpClient())
                {
                    Console.WriteLine("\nGET call - CyberSource JWT API");

                    string jwtToken = BoardingCredentials.GenerateJwTGET("", "GET", boardingAPI);

                    Console.WriteLine(" -- RequestURL -- ");
                    Console.WriteLine("\tURL : " + "https://" + requestHost + resource);

                    client.DefaultRequestHeaders.Add("v-c-merchant-id", signatureMerchantID);

                    client.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json")); // ACCEPT header
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                    client.Timeout = TimeSpan.FromMilliseconds(50000);

                    // POST FOR CYBERSOURCE TRANSACTION ****************************

                    var response = await client.GetAsync("https://" + requestHost + resource);

                    // POST FOR CYBERSOURCE TRANSACTION ****************************

                    responseCode = (TaskStatus)response.StatusCode;
                    string responseContent = await response.Content.ReadAsStringAsync();

                    jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);

                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    var jsonString = JsonSerializer.Serialize(jsonObject, options);

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
