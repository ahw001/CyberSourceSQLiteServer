using System.Text.Json.Nodes;
using CybsClass.Cybersource.Transactions;

namespace CybsClass.WebApi.Service.Services.TokenProcessing
{
    public static class CallTransTokenInfo
    {
        public static async Task<JsonObject> RunAsyncTransTokenInfo(string id)
        {
            JsonObject transTokenInfo;
            string resource = $"/up/v1/payment-details/{id}";

            Console.WriteLine("\n************* CALLING FOR TRANSIENT TOKEN INFO *****\n");

            try
            {

                transTokenInfo = await CallCyberSource.CallCyberSourceApiGet(resource, false);

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR IN TRANS TOKEN RETRIEVAL");
                Console.WriteLine(e.Message);
                transTokenInfo = new JsonObject();
                transTokenInfo.Add("error: ", e.Message);
                return transTokenInfo;
            }

            return transTokenInfo;
        }
    }
}
