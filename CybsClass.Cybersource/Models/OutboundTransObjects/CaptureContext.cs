using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CybsClass.Cybersource.Models.BaseData;

namespace CybsClass.Cybersource.Models.OutboundTransObjects
{
    public class CaptureContext
    {
        [JsonPropertyName("targetOrigins")]
        public List<string>? TargetOrigins { get; set; }

        [JsonPropertyName("clientVersion")]
        public string? ClientVersion { get; set; }

        [JsonPropertyName("allowedCardNetworks")]
        public string[]? AllowedCardNetworks { get; set; }

        [JsonPropertyName("allowedPaymentTypes")]
        public string[]? AllowedPaymentTypes { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("locale")]
        public string? Locale { get; set; }

        [JsonPropertyName("captureMandate")]
        public CaptureMandate? CaptureMandate { get; set; }

        [JsonPropertyName("orderInformation")]
        public OrderInformation? OrderInformation { get; set; }
    }
}
