using Microsoft.EntityFrameworkCore;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.EntityModels;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public static class PersistTokenizeData
    {
        public static async Task<Dictionary<string, string>> PersistTokenize(int b2cCustomerId, JsonObject jsonObject)
        {
            var dbResults = new Dictionary<string, string>();
            try
            {
                Console.WriteLine($"[PersistTokenize] Persisting tokenize token IDs for B2cCustomerId: {b2cCustomerId}");

                var response = JsonSerializer.Deserialize<TokenizeApiResponse>(jsonObject.ToString());

                if (response?.TokenInformation is null)
                {
                    dbResults.Add("Warning", "Tokenize response contained no tokenInformation — DB update skipped.");
                    Console.WriteLine("[PersistTokenize] No tokenInformation in response — skipping DB update.");
                    return dbResults;
                }

                using B2cNorthwindContext db = new();
                var affected = await db.PaymentCardInfos
                    .Where(m => m.B2cCustomerId == b2cCustomerId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(m => m.InstrumentIdentifierId,
                            response.TokenInformation.InstrumentIdentifier != null
                                ? response.TokenInformation.InstrumentIdentifier.Id
                                : null)
                        .SetProperty(m => m.PaymentInstrumentId,
                            response.TokenInformation.PaymentInstrument != null
                                ? response.TokenInformation.PaymentInstrument.Id
                                : null)
                        .SetProperty(m => m.CustomerInstrumentId,
                            response.TokenInformation.Customer != null
                                ? response.TokenInformation.Customer.Id
                                : null)
                    );

                Console.WriteLine($"[PersistTokenize] PaymentCardInfos rows updated: {affected}");
                dbResults.Add("Tokenize token IDs persisted", affected.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PersistTokenize] Exception: {ex.Message}");
                dbResults.Add("Exception in PersistTokenize", ex.Message);
            }
            return dbResults;
        }
    }
}
