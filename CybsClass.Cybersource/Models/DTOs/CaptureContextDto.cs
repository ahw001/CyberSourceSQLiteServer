using CybsClass.Cybersource.Models.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CybsClass.Cybersource.Models.DTOs
{
    public class CaptureContextDto
    {
        [JsonPropertyName("B2cCustomerId")]
        public string? B2cCustomerId { get; set; }

        [JsonPropertyName("ctx")]
        public string? Ctx { get; set; }

        [JsonPropertyName("OrderId")]
        public string? OrderId { get; set; }

        [JsonPropertyName("clientReferenceInformation")]
        public ClientReferenceInformation? ClientReferenceInformation { get; set; } = new();

        [JsonPropertyName("orderInformation")]
        public OrderInformation? OrderInformation { get; set; } = new();

        [JsonPropertyName("billTo")]
        public BillTo? BillTo { get; set; } = new();

        [JsonPropertyName("tokenInformation")]
        public TokenInformation? TokenInformation { get; set; } = new();

        [JsonPropertyName("processingInformation")]
        public ProcessingInformation? ProcessingInformation { get; set; } = new();

    }
}
