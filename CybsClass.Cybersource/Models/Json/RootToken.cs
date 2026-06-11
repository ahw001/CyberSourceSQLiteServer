using CybsClass.Cybersource.Models.BaseData;
using System.Text.Json.Serialization;

namespace CybsClass.Cybersource.Models.Json
{
    public class RootToken
    {
        [JsonPropertyName("_links")]
        public Links? Links { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("default")]
        public bool? Default { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("card")]
        public CybsClass.Cybersource.Models.BaseData.Card? Card { get; set; }

        [JsonPropertyName("billTo")]
        public BillTo? BillTo { get; set; }

        [JsonPropertyName("instrumentIdentifier")]
        public InstrumentIdentifier? InstrumentIdentifier { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }

        [JsonPropertyName("_embedded")]
        public Embedded? Embedded { get; set; }

        [JsonPropertyName("PaymentCardId")]
        public string? PaymentCardId { get; set; }

        [JsonPropertyName("object")]
        public string? TokenObject { get; set; }

        [JsonPropertyName("tokenReferenceId")]
        public string? TokenReferenceId { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("source")]
        public string? Source { get; set; }

        [JsonPropertyName("number")]
        public string? Number { get; set; }

        [JsonPropertyName("expirationMonth")]
        public string? ExpirationMonth { get; set; }

        [JsonPropertyName("expirationYear")]
        public string? ExpirationYear { get; set; }

        [JsonPropertyName("securityCode")]
        public string? SecurityCode { get; set; }

        [JsonPropertyName("requestorId")]
        public string? RequestorId { get; set; }

        [JsonPropertyName("enrollmentId")]
        public string? EnrollmentId { get; set; }

        [JsonPropertyName("tokenizedCard")]
        public TokenizedCard? TokenizedCard { get; set; }

        [JsonPropertyName("processingInformation")]
        public ProcessingInformation? ProcessingInformation { get; set; }

        [JsonPropertyName("issuer")]
        public Issuer? Issuer { get; set; }

        [JsonPropertyName("shipTo")]
        public BillTo? ShipTo { get; set; }

        [JsonPropertyName("buyerInformation")]
        public BuyerInformation? BuyerInformation { get; set; }
    }
}
