using System.Text.Json.Serialization;

namespace CybsClass.Cybersource.Models.BaseData
{
    public class UrlEndpointEntry
    {
        [JsonPropertyName("resource")]
        public string Resource { get; set; } = string.Empty;

        [JsonPropertyName("method")]
        public string Method { get; set; } = "POST";

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
