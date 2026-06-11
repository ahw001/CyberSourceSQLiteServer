using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CybsClass.WebApi.Service.Services.TokenProcessing
{
    public static class CallForTokenizedCards
    {
        public static async Task<JsonObject> RunAsync(TokenizedCardNetworkRequestDto dto)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            Console.WriteLine($"\n[CallForTokenizedCards] INBOUND DTO:\n{JsonSerializer.Serialize(dto, options)}");

            try
            {
                var cardNode = new JsonObject
                {
                    ["number"] = dto.CardNumber,
                    ["expirationMonth"] = dto.ExpMonth,
                    ["expirationYear"] = dto.ExpYear
                };
                if (!string.IsNullOrWhiteSpace(dto.SecurityCode))
                    cardNode["securityCode"] = dto.SecurityCode;

                var requestBody = new JsonObject
                {
                    ["source"] = dto.Source,
                    ["card"] = cardNode
                };

                string jsonString = requestBody.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine($"\n[CallForTokenizedCards] REQUEST TO CYBERSOURCE:\n{jsonString}");

                var jsonObject = await CallCyberSource.CallCyberSourceApiJsonApplicationJose(jsonString, "/tms/v2/tokenized-cards");

                Console.WriteLine($"\n[CallForTokenizedCards] RESPONSE FROM CYBERSOURCE:\n{JsonSerializer.Serialize(jsonObject, options)}");

                return jsonObject;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                string exJson = $"{{\"Exception\":\"{e.Message}\"}}";
                JsonDocument doc = JsonDocument.Parse(exJson);
                return JsonObject.Create(doc.RootElement)!;
            }
        }
    }
}
