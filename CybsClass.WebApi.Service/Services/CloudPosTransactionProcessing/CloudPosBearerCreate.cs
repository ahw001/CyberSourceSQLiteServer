using CybsClass.Cybersource.Authentication;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Transactions;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;



namespace CybsClass.WebApi.Service.Services.CloudPosTransactionProcessing
{
    public static class CloudPosBearerCreate
    {

        public static async Task<JsonObject> CallForBearerToken()
        {

            JsonObject jsonObject = new JsonObject();
            string resource = "/login";

            var bearerCreate = new BearerCreate
            {
                Id = PosCredentials.GetAcceptanceMid(),
                Secret = PosCredentials.GetAcceptanceSecretKey(),
            };

            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
            string jsonString = JsonSerializer.Serialize(bearerCreate, options);

            Console.WriteLine("\n************* CALLING FOR CLOUD MODE BEARER TOKEN *****\n");

            jsonObject = await CallCyberSourcePos.CallCyberSourceSecretPosApiJson(jsonString, resource);

            return await Task.FromResult(jsonObject);
        }

    }
}
