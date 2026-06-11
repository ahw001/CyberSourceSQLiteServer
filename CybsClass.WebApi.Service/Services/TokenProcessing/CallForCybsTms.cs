using CybsClass.Cybersource.Authentication;
using CybsClass.Cybersource.Models;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.OutboundTransObjects;
using CybsClass.Cybersource.Transactions;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CybsClass.WebApi.Service.Services.TokenProcessing
{
    public class CallForCybsTms
    {
        private static B2cCustomerDto B2cCustomerDto = new();
        private static string? merchantID;
        private static string? requestHost;

        private static JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };



        public static async Task<JsonObject> RunAsyncEnrollableInstId(B2cCustomerDto b2cCustomerDto)
        {
            B2cCustomerDto = b2cCustomerDto;
            JsonObject jsonObject = new();
            string resource = "/tms/v1/instrumentidentifiers";

            try
            {

                var intstrumentTokenData = new InstrumentTokenData
                {
                    Type = "enrollable card",

                    Card = new FullCard
                    {
                        Number = B2cCustomerDto.AccountNumber,
                        ExpirationMonth = B2cCustomerDto.ExpMonth,
                        ExpirationYear = B2cCustomerDto.ExpYear
                    }

                };

                var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

                string jsonString = JsonSerializer.Serialize(intstrumentTokenData, options);

                JsonObject requestJson = JsonSerializer.Deserialize<JsonObject>(jsonString)!;

                //Convert product json string to JsonNode to allow for editing
                JsonNode? jsonTransactionNode = JsonNode.Parse(jsonString);

                //Create string for Http Client POST input
                string? jsonTransString = jsonTransactionNode?.ToString()!;

                string jsonTokenString = JsonSerializer.Serialize(requestJson, options);

                jsonObject = await CallCyberSource.CallCyberSourceApiJson(jsonTokenString, resource, false);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                string jsonString = $"{{ \"Exception\": \"{e}\" }}";
                JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
                JsonElement rootElement = jsonDocument.RootElement;
                jsonObject = JsonObject.Create(rootElement)!;
                return await Task.FromResult(jsonObject);
            }

            return jsonObject;

        }




        public static async Task<JsonObject> TokenRetrievals(FollowOnTransDto followOnTransDto)
        {
            JsonObject jsonObject = new();
            string resource = string.Empty;
            string id = followOnTransDto.TransactionId!;
            TaskStatus responseCode;

            merchantID = Credentials.GetMerchantID();
            //merchantKeyId = Credentials.GetMerchantKeyId();
            //merchantsecretKey = Credentials.GetMerchantsecretKey();
            requestHost = Credentials.GetRequestHost();

            FollowOnTransactions? transaction = followOnTransDto.CurrentTransactionType!;

            if (transaction == FollowOnTransactions.SHIPPING_ID_RETRIEVE)
            {
                resource = $"/tms/v2/customers/{id}/shipping-addresses";
            }

            try
            {
                // HTTP GET request
                using (var client = new HttpClient())
                {
                    JsonNode? jsonNode;

                    Console.WriteLine("\nSample 1: GET call - CyberSource Token Retrievals");
                    Console.WriteLine(" -- RequestURL -- ");
                    Console.WriteLine("\tURL : " + "https://" + requestHost + resource);

                    string jwtToken = Credentials.GenerateJWT("", "GET", false);
                    client.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json")); // ACCEPT header
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                    using (var r = await client.GetAsync(new Uri("https://" + requestHost + resource)))
                    {
                        string responsecontent = await r.Content.ReadAsStringAsync();
                        //Console.WriteLine("\n -- Response Message --\n\n" + responsecontent);
                        responseCode = (TaskStatus)r.StatusCode;

                        jsonNode = JsonSerializer.Deserialize<JsonNode>(responsecontent);

                        var jsonString = JsonSerializer.Serialize(jsonNode, options);

                        if (jsonString.Contains("error", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("TRANSACTION WAS AN ERROR");
                            jsonObject = JsonSerializer.Deserialize<JsonObject>(responsecontent)!;
                        }
                        else
                        {
                            jsonObject = JsonSerializer.Deserialize<JsonObject>(responsecontent)!;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                string jsonString = $"{{ \"Exception\": \"{e}\" }}";
                JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
                JsonElement rootElement = jsonDocument.RootElement;
                jsonObject = JsonObject.Create(rootElement)!;
                return jsonObject;
            }
            return jsonObject;
        }
    }
}

