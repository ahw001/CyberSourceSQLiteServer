using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.Cybersource.Models.Mappers
{
    public static class AuthTransResponseMapper
    {
        public static AuthTransResponseDto? Map(AuthTransResponse authTransResponse)
        {
            return authTransResponse != null ? new AuthTransResponseDto
            {
                AuthTransResponsesId = authTransResponse.AuthTransResponsesId,
                Id = authTransResponse.Id,
                OrderId = authTransResponse.OrderId,
                ClientReferenceCode = authTransResponse.ClientReferenceCode,
                ConsumerAuthenticationToken = authTransResponse.ConsumerAuthenticationToken,
                IssuerResponseRaw = authTransResponse.IssuerResponseRaw,
                ReconciliationId = authTransResponse.ReconciliationId,
                Status = authTransResponse.Status,
                SubmitTimeUtc = authTransResponse.SubmitTimeUtc,
                Links = authTransResponse.Links,
                AuthorizedAmount = authTransResponse.AuthorizedAmount,
                Currency = authTransResponse.Currency,
                CardType = authTransResponse.CardType,
                ConsumerAuthInfoToken = authTransResponse.ConsumerAuthInfoToken,
                ClientRefInfoCode = authTransResponse.ClientRefInfoCode,
                PosinfoTerminalId = authTransResponse.PosinfoTerminalId,
                ProcInfoPayAcctReferenceNumber = authTransResponse.ProcInfoPayAcctReferenceNumber,
                ProcInfoMerchantNumber = authTransResponse.ProcInfoMerchantNumber,
                ProcInfoApprovalCode = authTransResponse.ProcInfoApprovalCode,
                ProcInfoNetworkTransactionId = authTransResponse.ProcInfoNetworkTransactionId,
                ProcInfoResponseCode = authTransResponse.ProcInfoResponseCode,
                ProcInfoTransactionId = authTransResponse.ProcInfoTransactionId,
                AvsCode = authTransResponse.AvsCode,
                AvsCodeRaw = authTransResponse.AvsCodeRaw,
                TokenInformationInstIdNew = authTransResponse.TokenInformationInstIdNew,
                TokenInformationInstId  = authTransResponse.TokenInformationInstId,
                InstrumentIdentifierState = authTransResponse.InstrumentIdentifierState,
                InstrumentIdentifierId = authTransResponse.InstrumentIdentifierId,
                PaymentInstrumentId = authTransResponse.PaymentInstrumentId,
                ResponseTransactionJson = authTransResponse.ResponseTransactionJson,


            } : null;
        }

        public static AuthTransResponse? Map(AuthTransResponseDto authTransResponseDto)
        {
            return authTransResponseDto != null ? new AuthTransResponse
            {
                AuthTransResponsesId = authTransResponseDto.AuthTransResponsesId,
                Id = authTransResponseDto.Id,
                OrderId = authTransResponseDto.OrderId,
                ClientReferenceCode = authTransResponseDto.ClientReferenceCode,
                ConsumerAuthenticationToken = authTransResponseDto.ConsumerAuthenticationToken,
                IssuerResponseRaw = authTransResponseDto.IssuerResponseRaw,
                ReconciliationId = authTransResponseDto.ReconciliationId,
                Status = authTransResponseDto.Status,
                SubmitTimeUtc = authTransResponseDto.SubmitTimeUtc,
                Links = authTransResponseDto.Links,
                AuthorizedAmount = authTransResponseDto.AuthorizedAmount,
                Currency = authTransResponseDto.Currency,
                CardType = authTransResponseDto.CardType,
                ConsumerAuthInfoToken = authTransResponseDto.ConsumerAuthInfoToken,
                ClientRefInfoCode = authTransResponseDto.ClientRefInfoCode,
                PosinfoTerminalId = authTransResponseDto.PosinfoTerminalId,
                ProcInfoPayAcctReferenceNumber = authTransResponseDto.ProcInfoPayAcctReferenceNumber,
                ProcInfoMerchantNumber = authTransResponseDto.ProcInfoMerchantNumber,
                ProcInfoApprovalCode = authTransResponseDto.ProcInfoApprovalCode,
                ProcInfoNetworkTransactionId = authTransResponseDto.ProcInfoNetworkTransactionId,
                ProcInfoResponseCode = authTransResponseDto.ProcInfoResponseCode,
                ProcInfoTransactionId = authTransResponseDto.ProcInfoTransactionId,
                AvsCode = authTransResponseDto.AvsCode,
                AvsCodeRaw = authTransResponseDto.AvsCodeRaw,
                TokenInformationInstIdNew = authTransResponseDto.TokenInformationInstIdNew,
                TokenInformationInstId = authTransResponseDto.TokenInformationInstId,
                InstrumentIdentifierState = authTransResponseDto.InstrumentIdentifierState,
                InstrumentIdentifierId = authTransResponseDto.InstrumentIdentifierId,
                PaymentInstrumentId = authTransResponseDto.PaymentInstrumentId,
                ResponseTransactionJson = authTransResponseDto.ResponseTransactionJson,
            } : null;
        }

        public static List<AuthTransResponse> Map(List<AuthTransResponseDto> authTransResponseDtos)
        {
            return authTransResponseDtos.Select(Map).ToList()!;
        }

        public static List<AuthTransResponseDto> Map(List<AuthTransResponse> authTransResponses)
        {
            return authTransResponses.Select(Map).ToList()!;
        }
    }
}
