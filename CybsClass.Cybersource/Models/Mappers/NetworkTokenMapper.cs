using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybsClass.Cybersource.Models.Mappers
{
    public static class NetworkTokenMapper
    {
        public static NetworkTokenInfoDto? Map(NetworkTokenInfo networkTokenInfo)
        {
            return networkTokenInfo != null ? new NetworkTokenInfoDto
            {
                PaymentTokenId = networkTokenInfo.PaymentTokenId,
                PaymentCardId = networkTokenInfo.PaymentCardId,
                TokenValue = networkTokenInfo.TokenValue,
                OriginalAccountExpMonth = networkTokenInfo.OriginalAccountExpMonth,
                OriginalAccountExpYear = networkTokenInfo.OriginalAccountExpYear,
                OriginalAccountNumber = networkTokenInfo.OriginalAccountNumber,
                OriginalAccountSuffix = networkTokenInfo.OriginalAccountSuffix,
                PaymentAccountReferenceNumber = networkTokenInfo.PaymentAccountReferenceNumber,
                TokenAccountNumber = networkTokenInfo.TokenAccountNumber,
                TokenExpMonth = networkTokenInfo.TokenExpMonth,
                TokenExpYear = networkTokenInfo.TokenExpYear,
                TokenizedCardType = networkTokenInfo.TokenizedCardType,
                TokenState = networkTokenInfo.TokenState,
                TokenRequestorId = networkTokenInfo.TokenRequestorId,
                EnrollmentId = networkTokenInfo.EnrollmentId,
                MitpreviousTransactionId = networkTokenInfo.MitpreviousTransactionId,
                ResponseTransactionJson = networkTokenInfo.ResponseTransactionJson
            } : null;
        }

        public static NetworkTokenInfo? Map(NetworkTokenInfoDto networkTokenDto)
        {
            return networkTokenDto != null ? new NetworkTokenInfo
            {
                PaymentTokenId = networkTokenDto.PaymentTokenId,
                PaymentCardId = networkTokenDto.PaymentCardId,
                TokenValue = networkTokenDto.TokenValue,
                OriginalAccountExpMonth = networkTokenDto.OriginalAccountExpMonth,
                OriginalAccountExpYear = networkTokenDto.OriginalAccountExpYear,
                OriginalAccountNumber = networkTokenDto.OriginalAccountNumber,
                OriginalAccountSuffix = networkTokenDto.OriginalAccountSuffix,
                PaymentAccountReferenceNumber = networkTokenDto.PaymentAccountReferenceNumber,
                TokenAccountNumber = networkTokenDto.TokenAccountNumber,
                TokenExpMonth = networkTokenDto.TokenExpMonth,
                TokenExpYear = networkTokenDto.TokenExpYear,
                TokenizedCardType = networkTokenDto.TokenizedCardType,
                TokenState = networkTokenDto.TokenState,
                TokenRequestorId = networkTokenDto.TokenRequestorId,
                EnrollmentId = networkTokenDto.EnrollmentId,
                MitpreviousTransactionId = networkTokenDto.MitpreviousTransactionId,
                ResponseTransactionJson = networkTokenDto.ResponseTransactionJson
            } : null;
        }

        public static List<NetworkTokenInfo> Map(List<NetworkTokenInfoDto> networkTokenDtos)
        {
            return networkTokenDtos.Select(Map).ToList()!;
        }

        public static List<NetworkTokenInfoDto> MapToDtos(List<NetworkTokenInfo> networkTokenInfos)
        {
            return networkTokenInfos.Select(Map).ToList()!;
        }
    }
}
