using Microsoft.EntityFrameworkCore;
using CybsClass.EntityModels;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public class DBPayerAuthCardSampleDatumServices
    {
        public static async Task<List<PayerAuthCardSampleDatum>> GetAllPayerAuthCardSampleData()
        {
            try
            {
                Console.WriteLine("[DBPayerAuthCardSampleDatumServices] Fetching all payer auth card sample data.");
                using B2cNorthwindContext db = new();
                return await db.PayerAuthCardSampleData.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBPayerAuthCardSampleDatumServices] Error fetching all payer auth card sample data: {ex}");
                return [];
            }
        }

        public static async Task<PayerAuthCardSampleDatum?> GetPayerAuthCardSampleDatumById(int id)
        {
            try
            {
                Console.WriteLine($"[DBPayerAuthCardSampleDatumServices] Fetching payer auth card sample datum with ID {id}.");
                using B2cNorthwindContext db = new();
                return await db.PayerAuthCardSampleData.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.SamplePayAuthPaymentCardId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBPayerAuthCardSampleDatumServices] Error fetching payer auth card sample datum with ID {id}: {ex}");
                return null;
            }
        }

        public static async Task<int> UpdatePayerAuthCardSampleDatum(int id, PayerAuthCardSampleDatum payerAuthCardSampleDatum)
        {
            try
            {
                Console.WriteLine($"[DBPayerAuthCardSampleDatumServices] Updating payer auth card sample datum with ID {id}.");
                using B2cNorthwindContext db = new();
                return await db.PayerAuthCardSampleData
                    .Where(model => model.SamplePayAuthPaymentCardId == id)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(m => m.SamplePayAuthPaymentCardId, payerAuthCardSampleDatum.SamplePayAuthPaymentCardId)
                        .SetProperty(m => m.CardBrand, payerAuthCardSampleDatum.CardBrand)
                        .SetProperty(m => m.AccountNumber, payerAuthCardSampleDatum.AccountNumber)
                        .SetProperty(m => m.ExpMonth, payerAuthCardSampleDatum.ExpMonth)
                        .SetProperty(m => m.ExpYear, payerAuthCardSampleDatum.ExpYear)
                        .SetProperty(m => m.Cvv, payerAuthCardSampleDatum.Cvv));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBPayerAuthCardSampleDatumServices] Error updating payer auth card sample datum with ID {id}: {ex}");
                return 0;
            }
        }

        public static async Task<PayerAuthCardSampleDatum?> CreatePayerAuthCardSampleDatum(PayerAuthCardSampleDatum payerAuthCardSampleDatum)
        {
            try
            {
                Console.WriteLine("[DBPayerAuthCardSampleDatumServices] Inserting new payer auth card sample datum.");
                using B2cNorthwindContext db = new();
                db.PayerAuthCardSampleData.Add(payerAuthCardSampleDatum);
                await db.SaveChangesAsync();
                Console.WriteLine($"[DBPayerAuthCardSampleDatumServices] Payer auth card sample datum created with ID {payerAuthCardSampleDatum.SamplePayAuthPaymentCardId}.");
                return payerAuthCardSampleDatum;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBPayerAuthCardSampleDatumServices] Error creating payer auth card sample datum: {ex}");
                return null;
            }
        }

        public static async Task<int> DeletePayerAuthCardSampleDatum(int id)
        {
            try
            {
                Console.WriteLine($"[DBPayerAuthCardSampleDatumServices] Deleting payer auth card sample datum with ID {id}.");
                using B2cNorthwindContext db = new();
                return await db.PayerAuthCardSampleData
                    .Where(model => model.SamplePayAuthPaymentCardId == id)
                    .ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBPayerAuthCardSampleDatumServices] Error deleting payer auth card sample datum with ID {id}: {ex}");
                return 0;
            }
        }
    }
}
