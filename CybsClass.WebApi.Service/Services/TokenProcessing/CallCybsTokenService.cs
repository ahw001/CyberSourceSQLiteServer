using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using System.Text.Json;
using System.Text.Json.Nodes;



namespace CybsClass.WebApi.Service.Services.TokenProcessing
{
    public static class CallCybsTokenService
    {
        static string jsonString = string.Empty;
        public static JsonObject jsonObject = new JsonObject();
        public static async Task<JsonObject> CallForCybsCombinedTokenService(B2cCustomerDto formDto)
        {
            try
            {
                jsonObject = await CallForCybsToken.RunAsyncJsonObject(formDto);

                return await Task.FromResult(jsonObject);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                jsonString = $"{{ \"Exception\": \"{e}\" }}";
                JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
                JsonElement rootElement = jsonDocument.RootElement;
                jsonObject = JsonObject.Create(rootElement)!;
                return await Task.FromResult(jsonObject);
            }

        }

        public static async Task<JsonObject> CallForCybsFlexTokenService(B2cCustomerDto formDto)
        {
            try
            {
                jsonObject = await CallForCybsToken.RunAsyncJsonObject(formDto);

                return await Task.FromResult(jsonObject);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                jsonString = $"{{ \"Exception\": \"{e}\" }}";
                JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
                JsonElement rootElement = jsonDocument.RootElement;
                jsonObject = JsonObject.Create(rootElement)!;
                return await Task.FromResult(jsonObject);
            }

        }

    }
}
