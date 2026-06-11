using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CybsClass.Cybersource.Models.BaseData;
using System;
using System.Text.Json.Serialization;

namespace CybsClass.Cybersource.Models.Json
{
    public class ZeroAuthRootToken
    {
        [JsonPropertyName("_links")]
        public Links? Links { get; set; }

        [JsonPropertyName("clientReferenceInformation")]
        public ClientReferenceInformation? ClientReferenceInformation { get; set; }

        [JsonPropertyName("consumerAuthenticationInformation")]
        public ConsumerAuthenticationInformation? ConsumerAuthenticationInformation { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("issuerInformation")]
        public IssuerInformation? IssuerInformation { get; set; }

        [JsonPropertyName("orderInformation")]
        public OrderInformation? OrderInformation { get; set; }

        [JsonPropertyName("paymentAccountInformation")]
        public PaymentAccountInformation? PaymentAccountInformation { get; set; }

        [JsonPropertyName("paymentInformation")]
        public PaymentInformation? PaymentInformation { get; set; }

        [JsonPropertyName("pointOfSaleInformation")]
        public PointOfSaleInformation? PointOfSaleInformation { get; set; }

        [JsonPropertyName("processorInformation")]
        public ProcessorInformation? ProcessorInformation { get; set; }

        [JsonPropertyName("reconciliationId")]
        public string? ReconciliationId { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("submitTimeUtc")]
        public string? SubmitTimeUtc { get; set; }

        [JsonPropertyName("tokenInformation")]
        public TokenInformation? TokenInformation { get; set; }
    
    }
}
