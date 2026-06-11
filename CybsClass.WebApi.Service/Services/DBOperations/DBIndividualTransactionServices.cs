using Microsoft.EntityFrameworkCore;
using CybsClass.EntityModels;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Mappers;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public class DBIndividualTransactionServices
    {
        public static async Task<int> GetIndividualTransactionCountAsync()
        {
            using B2cNorthwindContext db = new();
            return await db.IndividualTransactions.CountAsync();
        }
        public static async Task<List<IndividualTransactionDto>> GetIndividualTransactions()
        {
            try
            {
                Console.WriteLine("Geting full list of Individual Transactions ...");
                using B2cNorthwindContext db = new();
                var individualTransactions = await db.IndividualTransactions.ToListAsync();
                List<IndividualTransactionDto> paymentCardDtos = IndividualTransactionMapper.Map(individualTransactions)!;
                return paymentCardDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting full list of Individual Transactions: {ex.ToString()}");
                var individualTransactions = new List<IndividualTransactionDto>();
                IndividualTransactionDto paymentCardDto = new IndividualTransactionDto();
                paymentCardDto.Error = ex.ToString();
                individualTransactions.Add(paymentCardDto);
                return individualTransactions;
            }

        }

        public static async Task<IndividualTransactionDto?> GetIndividualTransactionByUsingId(int transactionid)
        {
            try
            {
                Console.WriteLine($"Geting Individual Transaction for: {transactionid}");
                using B2cNorthwindContext db = new();
                Task<IndividualTransaction?> task = db.IndividualTransactions.AsNoTracking()
                            .FirstOrDefaultAsync(model => model.TransactionId == transactionid);

                var individualTransaction = await task;
                if (individualTransaction == null)
                {
                    return null;
                }
                return IndividualTransactionMapper.Map(individualTransaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Individual Transaction for ID: {ex.ToString()}");
                var paymentCardDto = new IndividualTransactionDto();
                paymentCardDto.Error = ex.ToString();
                return paymentCardDto;
            }
        }
        public static async Task<int> CreateIndividualTransaction(IndividualTransactionDto paymentCardDto)
        {
            try
            {
                IndividualTransaction individualTransaction = IndividualTransactionMapper.Map(paymentCardDto)!;
                using B2cNorthwindContext db = new();
                db.IndividualTransactions.Add(individualTransaction);
                var affected = await db.SaveChangesAsync();
                return affected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Creating Individual Transaction - {ex.ToString()}");
                return 0;
            }
        }
    }
}
