using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.Cybersource.Models.Mappers
{
    public static class PaymentCardMapper
    {
        public static PaymentCardDto? Map(PaymentCardInfo paymentCardInfo)
        {
            return paymentCardInfo != null ? new PaymentCardDto
            {
                PaymentCardId = paymentCardInfo.PaymentCardId,
                B2cCustomerId = paymentCardInfo.B2cCustomerId,
                CustomerInstrumentId = paymentCardInfo.CustomerInstrumentId,
                AccountNumber = paymentCardInfo!.AccountNumber!,
                TokenValue = paymentCardInfo.TokenValue,
                ExpMonth = paymentCardInfo.ExpMonth,
                ExpYear = paymentCardInfo.ExpYear,
                Cvv = paymentCardInfo.Cvv,
                InstrumentIdentifierId = paymentCardInfo.InstrumentIdentifierId,
                InstrumentIdentifierState = paymentCardInfo.InstrumentIdentifierState,
                InstrumentidentifierNew = paymentCardInfo.InstrumentidentifierNew,
                PaymentInstrumentId = paymentCardInfo.PaymentInstrumentId,
                ResponseTransactionJson = paymentCardInfo.ResponseTransactionJson,
                TokenizedCardType = paymentCardInfo.TokenizedCardType,
                PaymentAccountReferenceNumber = paymentCardInfo.PaymentAccountReferenceNumber
            } : null;
        }

        public static PaymentCardInfo? Map(PaymentCardDto paymentCardDto)
        {
            if (paymentCardDto.PaymentCardId == 0)
            {
                return paymentCardDto != null ? new PaymentCardInfo
                {
                    B2cCustomerId = paymentCardDto.B2cCustomerId,
                    AccountNumber = paymentCardDto.AccountNumber,
                    CustomerInstrumentId = paymentCardDto.CustomerInstrumentId,
                    TokenValue = paymentCardDto.TokenValue,
                    ExpMonth = paymentCardDto.ExpMonth,
                    ExpYear = paymentCardDto.ExpYear,
                    Cvv = paymentCardDto.Cvv,
                    InstrumentIdentifierId = paymentCardDto.InstrumentIdentifierId,
                    InstrumentIdentifierState = paymentCardDto.InstrumentIdentifierState,
                    InstrumentidentifierNew = paymentCardDto.InstrumentidentifierNew,
                    PaymentInstrumentId = paymentCardDto.PaymentInstrumentId,
                    ResponseTransactionJson = paymentCardDto.ResponseTransactionJson,
                    TokenizedCardType = paymentCardDto.TokenizedCardType,
                    PaymentAccountReferenceNumber = paymentCardDto.PaymentAccountReferenceNumber
                } : null;
            }
            else
            {
                return paymentCardDto != null ? new PaymentCardInfo
                {
                    PaymentCardId = paymentCardDto.PaymentCardId,
                    B2cCustomerId = paymentCardDto.B2cCustomerId,
                    CustomerInstrumentId = paymentCardDto.CustomerInstrumentId,
                    AccountNumber = paymentCardDto.AccountNumber,
                    TokenValue = paymentCardDto.TokenValue,
                    ExpMonth = paymentCardDto.ExpMonth,
                    ExpYear = paymentCardDto.ExpYear,
                    Cvv = paymentCardDto.Cvv,
                    InstrumentIdentifierId = paymentCardDto.InstrumentIdentifierId,
                    InstrumentIdentifierState = paymentCardDto.InstrumentIdentifierState,
                    InstrumentidentifierNew = paymentCardDto.InstrumentidentifierNew,
                    PaymentInstrumentId = paymentCardDto.PaymentInstrumentId,
                    ResponseTransactionJson = paymentCardDto.ResponseTransactionJson,
                    TokenizedCardType = paymentCardDto.TokenizedCardType,
                    PaymentAccountReferenceNumber = paymentCardDto.PaymentAccountReferenceNumber
                } : null;
            }
        }

        public static List<PaymentCardInfo> Map(List<PaymentCardDto> paymentCardDtos)
        {
            return paymentCardDtos.Select(Map).ToList()!;
        }

        public static List<PaymentCardDto> Map(List<PaymentCardInfo> paymentCardInfos)
        {
            return paymentCardInfos.Select(Map).ToList()!;
        }
    }
}
