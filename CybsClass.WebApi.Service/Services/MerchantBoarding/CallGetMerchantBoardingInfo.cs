using System.Text.Json.Nodes;
using CybsClass.Cybersource.Transactions;

namespace CybsClass.WebApi.Service.Services.MerchantBoarding
{
    public static class CallGetMerchantBoardingInfo
    {
        public static async Task<JsonObject> RunAsyncGetMerchantInfo(string mid, bool boardingAPI)
        {
            JsonObject transTokenInfo;
        
            string resource = $"/oms/v1/organizations/{mid}";

            try
            {
                Console.WriteLine("\n************* CALLING FOR GET MERCHANT BOARDING INFO *****\n");

                transTokenInfo = await CallCyberSourceBoarding.CallCybsApiJsonJwtGET(resource, boardingAPI);

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR IN GET MERCHANT BOARDING INFO");
                Console.WriteLine(e.Message);
                transTokenInfo = new JsonObject();
                transTokenInfo.Add("error: ", e.Message);
                return transTokenInfo;
            }

            return transTokenInfo;
        }
    }
}
