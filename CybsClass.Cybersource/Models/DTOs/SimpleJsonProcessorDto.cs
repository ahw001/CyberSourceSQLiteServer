using System.Text.Json.Serialization;

namespace CybsClass.Cybersource.Models.DTOs
{
    public class SimpleJsonProcessorDto
    {
        [JsonPropertyName("value")]
        public string Value { get; set; } = null!;

        [JsonPropertyName("resource")]
        public string Resource { get; set; } = null!;

        [JsonPropertyName("isBoarding")]
        public bool IsBoarding { get; set; } = false;

        [JsonPropertyName("requestEncryption")]
        public string RequestEncryption { get; set; } = "none";

        [JsonPropertyName("responseEncryption")]
        public string ResponseEncryption { get; set; } = "none";

        [JsonPropertyName("httpMethod")]
        public string? HttpMethod { get; set; }
    }
}
