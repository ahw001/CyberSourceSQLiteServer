using Microsoft.EntityFrameworkCore;
using CybsClass.Cybersource.Models.Json;
using CybsClass.EntityModels;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public static class PersistCybsTokenData
    {
        private static Dictionary<string, string> dbResults = new Dictionary<string, string>();

        private static string statusNode = string.Empty;

        private static string customerInstId = string.Empty;

        public static async Task<Dictionary<string, string>> TokenDBOps(int customerId, JsonObject jsonObject)
        {
            try
            {
                dbResults = new();

                Console.WriteLine("Inserting token data ...");

                JsonDocument document = JsonDocument.Parse(jsonObject.ToString());

                if (document.RootElement.TryGetProperty("status", out JsonElement statusElement))
                {
                    statusNode = statusElement.GetString()!;
                    if (statusNode == "AUTHORIZED")
                    {
                        ZeroAuthRootToken zeroAuthRootToken = JsonSerializer.Deserialize<ZeroAuthRootToken>(jsonObject.ToString())!;
                        if (zeroAuthRootToken is not null && zeroAuthRootToken.Id is not null)
                        {
                            Console.WriteLine("Updating PaymentCardInfos table ...");
                            Console.WriteLine($"customerId: {customerId}");
                            Console.WriteLine($"ZeroAuthRootToken.Id: {zeroAuthRootToken.Id}");
                            using B2cNorthwindContext db = new();
                            var affected = await db.PaymentCardInfos
                                .Where(model => model.B2cCustomerId == customerId).ExecuteUpdateAsync(setters => setters
                                    .SetProperty(m => m.InstrumentIdentifierId, zeroAuthRootToken!.TokenInformation!.InstrumentIdentifier!.Id ?? null)
                                    .SetProperty(m => m.CustomerInstrumentId, zeroAuthRootToken!.TokenInformation!.Customer!.Id ?? null)
                                );
                            dbResults.Add("Affected: ZeroAuth InstID and CustInstID Token Added", affected.ToString());

                            dbResults = await PersistShippingInstAddress.InsertShippingInstAddress(zeroAuthRootToken);
                        }
                    }
                }

                if (document.RootElement.TryGetProperty("customer", out JsonElement customerElement))
                {
                    Console.WriteLine("Customer element is present.");

                    dbResults = new();

                    RootToken rootToken = JsonSerializer.Deserialize<RootToken>(customerElement.ToString())!;

                    using B2cNorthwindContext db = new();

                    if (rootToken is not null && rootToken.Id is not null && rootToken?.BuyerInformation?.MerchantCustomerID is not null)
                    {
                        try
                        {
                            customerInstId = rootToken.Id!;
                            Console.WriteLine("Updating PaymentCardInfos table ...");
                            Console.WriteLine($"customerId: {customerId}");
                            Console.WriteLine($"Customer Instrument ID - rootToken.Id: {rootToken.Id}");
                            Console.WriteLine($"Merchant Customer ID - rootToken.BuyerInformation.MerchantCustomerID: {rootToken.BuyerInformation.MerchantCustomerID}");
                            var affected = await db.PaymentCardInfos
                                .Where(model => model.B2cCustomerId == customerId).ExecuteUpdateAsync(setters => setters
                                    .SetProperty(m => m.CustomerInstrumentId, rootToken.Id)
                                    .SetProperty(m => m.MerchantCustomerId, rootToken.BuyerInformation.MerchantCustomerID)
                                );
                            dbResults.Add("Affected: Customer Token Added", affected.ToString());

                        }
                        catch (Exception ex)
                        {
                            dbResults.Add("Error in Customer Token Persistance", ex.Message);
                            Console.WriteLine($"Exception: {ex.Message}");

                        }
                    }
                }
                if (document.RootElement.TryGetProperty("shippingAddress", out JsonElement shippingAddress))
                {
                    Console.WriteLine("Shipping Address element is present.");
                    RootToken rootToken = JsonSerializer.Deserialize<RootToken>(shippingAddress.ToString())!;
                    if (rootToken is not null && rootToken.Id is not null && customerInstId != "")
                    {

                        dbResults = new();

                        rootToken = JsonSerializer.Deserialize<RootToken>(shippingAddress.ToString())!;
                        dbResults = await PersistShippingInstAddress.InsertShippingInstAddress(rootToken, customerInstId);

                    }
                    else
                    {
                        // Handle case where "Id" property exists but is null
                        Console.WriteLine("Id property exists but is null.");
                        dbResults.Add("Error in Shipping Token Persistance", "No Shipping Inst ID Found.");

                    }
                }
                else
                {
                    // Handle case where "Id" property does not exist
                    Console.WriteLine("Id property does not exist.");
                    dbResults.Add("Error in Token Persistance", "No Shipping Inst ID Found.");

                }
            }
            catch (Exception ex)
            {
                dbResults = new();
                dbResults.Add("Exception in Token Persistance", ex.Message);
                Console.WriteLine($"Exception: {ex.Message}");
                return dbResults;
            }
            return dbResults;
        }
    }
}





