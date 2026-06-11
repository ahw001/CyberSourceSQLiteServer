namespace CybsClass.Cybersource.Models.DTOs
{
    public class PaymentCardDto
    {
        public int PaymentCardId { get; set; }

        public int B2cCustomerId { get; set; }

        public string? CustomerInstrumentId { get; set; }

        public string AccountNumber { get; set; } = null!;

        public string? TokenValue { get; set; }

        public string? ExpMonth { get; set; }

        public string? ExpYear { get; set; }

        public string? Cvv { get; set; }

        public string? PaymentAccountReferenceNumber { get; set; }

        public string? TokenizedCardType { get; set; }

        public string? InstrumentidentifierNew { get; set; }

        public string? InstrumentIdentifierId { get; set; }

        public string? InstrumentIdentifierState { get; set; }

        public string? PaymentInstrumentId { get; set; }

        public string? ResponseTransactionJson { get; set; }
        public string? Error { get; set;}
    }
}
