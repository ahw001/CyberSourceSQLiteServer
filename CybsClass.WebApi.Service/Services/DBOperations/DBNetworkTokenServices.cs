using Microsoft.EntityFrameworkCore;
using CybsClass.EntityModels;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Mappers;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public class DBNetworkTokenServices
    {
        public static async Task<int> GetNetworkTokenInfosCountAsync()
        {
            using B2cNorthwindContext db = new();
            return await db.NetworkTokenInfos.CountAsync();
        }
        public static async Task<List<NetworkTokenInfoDto>> GetNetworkTokens()
        {
            try
            {
                Console.WriteLine("Geting full list of Network Tokens ...");
                using B2cNorthwindContext db = new();
                var networkTokenInfos = await db.NetworkTokenInfos.ToListAsync();
                List<NetworkTokenInfoDto> NetworkTokenInfoDtos = NetworkTokenMapper.MapToDtos(networkTokenInfos)!;
                return NetworkTokenInfoDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting full list of Network Tokens: {ex.ToString()}");
                var networkTokenInfos = new List<NetworkTokenInfoDto>();
                NetworkTokenInfoDto NetworkTokenInfoDto = new NetworkTokenInfoDto();
                NetworkTokenInfoDto.Error = ex.ToString();
                networkTokenInfos.Add(NetworkTokenInfoDto);
                return networkTokenInfos;
            }

        }

        public static async Task<List<NetworkTokenInfoDto>> GetNetworkTokenByUsingId(int paymentCardId)
        {
            try 
            { 
                using B2cNorthwindContext db = new();
                var networkInfos = await db.NetworkTokenInfos.Where(n => n.PaymentCardId == paymentCardId).ToListAsync();
                var networkTokenDtos = NetworkTokenMapper.MapToDtos(networkInfos)!;
                return networkTokenDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Network Token for ID: {ex}");
                var NetworkTokenInfoDtos = new List<NetworkTokenInfoDto>();
                NetworkTokenInfoDto networkTokenInfoDto = new NetworkTokenInfoDto();
                networkTokenInfoDto.Error = ex.Message;
                NetworkTokenInfoDtos.Add(networkTokenInfoDto);
                return NetworkTokenInfoDtos!;
            }
        }

        public static async Task<int> CreateNetworkToken(NetworkTokenInfoDto networkTokenInfoDto)
        {
            try
            {
                NetworkTokenInfo networkTokenInfo = NetworkTokenMapper.Map(networkTokenInfoDto)!;
                using B2cNorthwindContext db = new();
                db.NetworkTokenInfos.Add(networkTokenInfo);
                var affected = await db.SaveChangesAsync();
                return affected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Creating Network Token - {ex.ToString()}");
                return 0;
            }

        }

        public static async Task<int> UpdateNetworkToken(int id, NetworkTokenInfoDto networkTokenInfoDto)
        {
            try
            {
                NetworkTokenInfo networkTokenInfo = NetworkTokenMapper.Map(networkTokenInfoDto)!;
                using B2cNorthwindContext db = new();
                var affected = await db.NetworkTokenInfos
                .Where(model => model.PaymentTokenId == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.PaymentTokenId, networkTokenInfo.PaymentTokenId)
                    .SetProperty(m => m.PaymentCardId, networkTokenInfo.PaymentTokenId)
                    .SetProperty(m => m.TokenValue, networkTokenInfo.TokenValue)
                    .SetProperty(m => m.OriginalAccountExpMonth, networkTokenInfo.OriginalAccountExpMonth)
                    .SetProperty(m => m.OriginalAccountExpYear, networkTokenInfo.OriginalAccountExpYear)
                    .SetProperty(m => m.OriginalAccountNumber, networkTokenInfo.OriginalAccountNumber)
                    .SetProperty(m => m.OriginalAccountSuffix, networkTokenInfo.OriginalAccountSuffix)
                    .SetProperty(m => m.TokenizedCardType, networkTokenInfo.TokenizedCardType)
                    .SetProperty(m => m.PaymentAccountReferenceNumber, networkTokenInfo.PaymentAccountReferenceNumber)
                    .SetProperty(m => m.TokenAccountNumber, networkTokenInfo.TokenAccountNumber)
                    .SetProperty(m => m.TokenExpMonth, networkTokenInfo.TokenExpMonth)
                    .SetProperty(m => m.TokenExpYear, networkTokenInfo.TokenExpYear)
                    .SetProperty(m => m.TokenRequestorId, networkTokenInfo.ResponseTransactionJson)
                    .SetProperty(m => m.TokenizedCardType, networkTokenInfo.ResponseTransactionJson)
                    .SetProperty(m => m.TokenState, networkTokenInfo.ResponseTransactionJson)
                    .SetProperty(m => m.EnrollmentId, networkTokenInfo.ResponseTransactionJson)
                    .SetProperty(m => m.MitpreviousTransactionId, networkTokenInfo.ResponseTransactionJson)
                    .SetProperty(m => m.ResponseTransactionJson, networkTokenInfo.ResponseTransactionJson)
                    );
                return affected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Updating Network Token - {ex.ToString()}");
                return 0;
            }
        }

    }



}
