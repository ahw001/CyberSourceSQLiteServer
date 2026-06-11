using System.Text.Json.Serialization;

namespace CybsClass.Cybersource.Models.SemiIntegrated
{
    public class PosSetup
    {
        [JsonPropertyName("posId")]
        public string? PosId { get; set; }

        [JsonPropertyName("setupCode")]
        public string? SetupCode { get; set; }
    }
}
