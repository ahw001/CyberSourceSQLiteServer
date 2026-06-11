using Microsoft.EntityFrameworkCore;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public static class DBCybsTokenServices
    {
        private static Dictionary<string, string> dbResult = new();

        public static async Task<Dictionary<string, string>> UpdateCustomerInstIdAsync(int customerId, B2cCustomerDto b2cCustomerDto)
        {
            using B2cNorthwindContext db = new();
            try 
            {
                var affected = await db.PaymentCardInfos
                    .Where(model => model.B2cCustomerId == customerId).ExecuteUpdateAsync(setters => setters
                        .SetProperty(m => m.CustomerInstrumentId, b2cCustomerDto.CustomerInstrumentId)
                        .SetProperty(m => m.MerchantCustomerId, b2cCustomerDto.MerchantCustomerID)
                    );
                dbResult.Add("Affected", affected.ToString());
            } catch (Exception ex) {
                dbResult.Add("Error", ex.Message);
            }

            return dbResult;
        }

        public static async Task<Dictionary<string, string>> UpdatePaymentCardInstId(int customerId, B2cCustomerDto b2cCustomerDto)
        {
            using B2cNorthwindContext db = new();
            try
            {
                var affected = await db.PaymentCardInfos
                    .Where(model => model.B2cCustomerId == customerId).ExecuteUpdateAsync(setters => setters
                        .SetProperty(m => m.InstrumentIdentifierId, b2cCustomerDto.InstrumentIdentifier)
                    );
                dbResult.Add("Affected", affected.ToString());
            }
            catch (Exception ex)
            {
                dbResult.Add("Error", ex.Message);
            }

            return dbResult;
        }

        public static async Task<Dictionary<string, string>> GetNetworkTokenCountById(int paymentCardId)
        { 
            Dictionary<string, string> dbResults = new();

            using B2cNorthwindContext db = new();
            try
            {
                var affected = await db.NetworkTokenInfos
                       .Where(nt => nt.PaymentCardId == paymentCardId)
                       .CountAsync();
                dbResults.Add("Network Token Count:", affected.ToString());
                return dbResults;
            }
            catch (Exception ex) 
            { 
                string error = ex.Message;
                dbResults.Add("Exception", error);
                return dbResults;
            }
        }
    }
}
