using Microsoft.EntityFrameworkCore;
using CybsClass.EntityModels;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Mappers;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public class DBPaymentCardServices
    {
        private static Dictionary<string, string> dbResults = new();
        public static async Task<int> GetPaymentCardCountAsync()
        {
            using B2cNorthwindContext db = new();
            return await db.PaymentCardInfos.CountAsync();
        }
        public static async Task<List<PaymentCardDto>> GetPaymentCardInfos()
        {
            try 
            {
                Console.WriteLine("Geting full list of Payment Cards ...");
                using B2cNorthwindContext db = new();
                var paymentCardInfos =  await db.PaymentCardInfos.ToListAsync();
                List<PaymentCardDto> paymentCardDtos = PaymentCardMapper.Map(paymentCardInfos)!;
                return paymentCardDtos;
            } 
            catch (Exception ex) 
            { 
                Console.WriteLine($"Error getting full list of Payment Cards: {ex.ToString()}");
                var paymentCardInfos = new List<PaymentCardDto>();
                PaymentCardDto paymentCardDto = new PaymentCardDto();
                paymentCardDto.Error = ex.ToString();
                paymentCardInfos.Add(paymentCardDto);
                return paymentCardInfos;
            }

        }

        public static async Task<PaymentCardDto?> GetPaymentCardInfoByUsingId(int paymentcardid)
        {
            try 
            {
                Console.WriteLine($"Geting Payment Card for: {paymentcardid}");
                using B2cNorthwindContext db = new();
                Task<PaymentCardInfo?> task = db.PaymentCardInfos.AsNoTracking()
                            .FirstOrDefaultAsync(model => model.PaymentCardId == paymentcardid);

                var paymentCardInfo = await task;
                if (paymentCardInfo == null)
                {
                    return null;
                }
                return PaymentCardMapper.Map(paymentCardInfo);
            } 
            catch (Exception ex) 
            {
                Console.WriteLine($"Error Payment Card for ID: {ex.ToString()}");
                var paymentCardDto = new PaymentCardDto();
                paymentCardDto.Error = ex.ToString(); 
                return paymentCardDto;
            }
        }
        public static async Task<Dictionary<string, string>> CreatePaymentCardInfo(PaymentCardDto paymentCardDto)
        {
            try 
            {
                dbResults.Clear();
                PaymentCardInfo paymentCardInfo = PaymentCardMapper.Map(paymentCardDto)!;
                using B2cNorthwindContext db = new();
                db.PaymentCardInfos.Add(paymentCardInfo);
                var affected = await db.SaveChangesAsync();
                var instertedId = paymentCardInfo.PaymentCardId;
                dbResults.Add("Affected", affected.ToString());
                dbResults.Add("PaymentCardId", instertedId.ToString());
                return dbResults;
            } 
            catch (Exception ex) 
            {
                Console.WriteLine($"Error Creating Payment Card - {ex.ToString()}");
                dbResults.Add("Error", ex.ToString());
                return dbResults;
            }

        }

        public static async Task<List<PaymentCardInfo>> GetAllPaymentCardInfoEntities()
        {
            try
            {
                Console.WriteLine("Getting full list of Payment Card entities ...");
                using B2cNorthwindContext db = new();
                return await db.PaymentCardInfos.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting full list of Payment Card entities: {ex}");
                return [];
            }
        }

        public static async Task<int> DeletePaymentCardInfo(int id)
        {
            try
            {
                Console.WriteLine($"Deleting Payment Card with ID {id} ...");
                using B2cNorthwindContext db = new();
                return await db.PaymentCardInfos
                    .Where(model => model.PaymentCardId == id)
                    .ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Payment Card with ID {id}: {ex}");
                return 0;
            }
        }

        public static async Task<int> UpdatePaymentCardInfo(int id, PaymentCardDto paymentCardDto)
        {
            try
            {
                PaymentCardInfo paymentCardInfo = PaymentCardMapper.Map(paymentCardDto)!;
                using B2cNorthwindContext db = new();
                var affected = await db.PaymentCardInfos
                .Where(model => model.PaymentCardId == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.PaymentCardId, paymentCardInfo.PaymentCardId)
                    .SetProperty(m => m.B2cCustomerId, paymentCardInfo.B2cCustomerId)
                    .SetProperty(m => m.AccountNumber, paymentCardInfo.AccountNumber)
                    .SetProperty(m => m.TokenValue, paymentCardInfo.TokenValue)
                    .SetProperty(m => m.ExpMonth, paymentCardInfo.ExpMonth)
                    .SetProperty(m => m.ExpYear, paymentCardInfo.ExpYear)
                    .SetProperty(m => m.Cvv, paymentCardInfo.Cvv)
                    .SetProperty(m => m.PaymentAccountReferenceNumber, paymentCardInfo.PaymentAccountReferenceNumber)
                    .SetProperty(m => m.TokenizedCardType, paymentCardInfo.TokenizedCardType)
                    .SetProperty(m => m.InstrumentidentifierNew, paymentCardInfo.InstrumentidentifierNew)
                    .SetProperty(m => m.InstrumentIdentifierId, paymentCardInfo.InstrumentIdentifierId)
                    .SetProperty(m => m.InstrumentIdentifierState, paymentCardInfo.InstrumentIdentifierState)
                    .SetProperty(m => m.PaymentInstrumentId, paymentCardInfo.PaymentInstrumentId)
                    .SetProperty(m => m.ResponseTransactionJson, paymentCardInfo.ResponseTransactionJson)
                    );
                return affected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Updating Payment Card - {ex.ToString()}");
                return 0;
            }
        }
    }
}

