using System.Text.Json.Serialization;

namespace CybsClass.Cybersource.Models.OutboundTransObjects
{
    public class FlexData
    {
        [JsonPropertyName("targetOrigins")]
        public string[]? TargetOrigins { get; set; }

        [JsonPropertyName("allowedCardNetworks")]
        public string[]? AllowedCardNetworks { get; set; }

        [JsonPropertyName("clientVersion")]
        public string? ClientVersion { get; set; }
    }
}
