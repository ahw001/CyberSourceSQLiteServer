using CybsClass.Cybersource.Models.BaseData;
using System.Text.Json.Serialization;

namespace CybsClass.Cybersource.Models.DTOs
{
    public class CtxPaymentDto
    {

        public string? B2cCustomerId { get; set; }

        public string? OrderId { get; set; }

        [JsonPropertyName("clientReferenceInformation")]
        public ClientReferenceInformation? ClientReferenceInformation { get; set; }

        [JsonPropertyName("orderInformation")]
        public OrderInformation? OrderInformation { get; set; }

        [JsonPropertyName("billTo")]
        public BillTo? BillTo { get; set; }

        [JsonPropertyName("tokenInformation")]
        public TokenInformation? TokenInformation { get; set; }

        [JsonPropertyName("processingInformation")]
        public ProcessingInformation? ProcessingInformation { get; set; } = new();
    }
}
