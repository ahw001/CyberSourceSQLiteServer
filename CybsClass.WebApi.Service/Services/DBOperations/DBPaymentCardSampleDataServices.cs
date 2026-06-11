using Microsoft.EntityFrameworkCore;
using CybsClass.EntityModels;
using CybsClass.Cybersource.Models.DTOs;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public static class DBPaymentCardSampleDataServices
    {
        public static async Task<List<PaymentCardSampleDatumDto>> GetNtCardsAsync(B2cNorthwindContext context)
        {
            try
            {
                Console.WriteLine("[DBPaymentCardSampleDataServices] Fetching NT sample cards ...");
                var entities = await context.PaymentCardSampleData
                    .Where(c => c.Nt)
                    .ToListAsync();

                return entities.Select(e => new PaymentCardSampleDatumDto
                {
                    SamplePaymentCardId = e.SamplePaymentCardId,
                    CardBrand = e.CardBrand,
                    AccountNumber = e.AccountNumber,
                    ExpMonth = e.ExpMonth,
                    ExpYear = e.ExpYear,
                    Cvv = e.Cvv,
                    NtScenario = e.NtScenario
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBPaymentCardSampleDataServices] Error fetching NT cards: {ex.Message}");
                return new List<PaymentCardSampleDatumDto>
                {
                    new() { Error = new() { Message = ex.Message } }
                };
            }
        }
    }
}
