using Microsoft.EntityFrameworkCore.ChangeTracking;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Json;
using CybsClass.Cybersource.Transactions;
using CybsClass.WebApi.Service.Services.TokenProcessing;
using CybsClass.EntityModels;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public static class PersistTransTokenData
    {
        private static B2cCustomer b2CCustomer = new B2cCustomer();
        private static Dictionary<string, object> dbResults = new();
        private static FollowOnTransJson followOnTransJson = new FollowOnTransJson();
        static JsonObject? jsonObject = [];


        
        public static async Task<Dictionary<string, object>> InsertCustomers(CtxPaymentDto ctxPaymentDto, JsonNode authTransNode)
        {
              
            dbResults = new();
            
            Console.WriteLine("Inserting transient token data ...");
            try
            {
                jsonObject = await CallTransTokenInfo.RunAsyncTransTokenInfo(ctxPaymentDto!.TokenInformation!.TransientTokenJwt!);

                if (jsonObject is null)
                {
                    dbResults = new();
                    dbResults.Add("Exception", "Call for Transient Token Info Failed");
                    return dbResults;
                }
                else
                {
                    followOnTransJson = JsonSerializer.Deserialize<FollowOnTransJson>(jsonObject!)!;
                }

                using B2cNorthwindContext db = new();

                B2cCustomer c = new();

                c.FirstName = ctxPaymentDto.OrderInformation!.BillTo!.FirstName! ?? "null";
                c.LastName = ctxPaymentDto.OrderInformation!.BillTo!.LastName! ?? "null";
                c.Phone = ctxPaymentDto.OrderInformation!.BillTo!.PhoneNumber! ?? "null";
                c.Email = ctxPaymentDto.OrderInformation!.BillTo!.Email! ?? "null";
                c.Address1 = ctxPaymentDto.OrderInformation!.BillTo!.Address1! ?? "null";
                //c.Address2 = Address2;
                c.City = ctxPaymentDto.OrderInformation!.BillTo!.AdministrativeArea! ?? "null";
                c.Region = ctxPaymentDto.OrderInformation!.BillTo!.AdministrativeArea! ?? "null";
                c.PostalCode = ctxPaymentDto.OrderInformation!.BillTo!.PostalCode! ?? "null";
                c.Country = ctxPaymentDto.OrderInformation!.BillTo!.Country! ?? "null";


                var paymentCardInfo = new PaymentCardInfo();

                paymentCardInfo.AccountNumber = followOnTransJson.PaymentInformation!.Card!.Number! ?? "null";
                paymentCardInfo.ExpMonth = followOnTransJson.PaymentInformation!.Card!.ExpirationMonth!.ToString() ?? "0";
                paymentCardInfo.ExpYear = followOnTransJson.PaymentInformation!.Card!.ExpirationYear!.ToString() ?? "0";


                c.PaymentCardInfos.Add(paymentCardInfo);

                EntityEntry<B2cCustomer> entity = db.B2cCustomers.Add(c);

                int affected0 = await db.SaveChangesAsync();

                Console.WriteLine($"Customer Addition State:  {entity.State}");
                dbResults.Add("B2cCustomerId", c.B2cCustomerId);

                return dbResults;
            }
            catch (Exception ex)
            {
                dbResults = new();
                dbResults.Add("Exception", ex.Message);
                Console.WriteLine($"Exception: {ex.Message}");
                return dbResults;
            }
        }
    }
}
