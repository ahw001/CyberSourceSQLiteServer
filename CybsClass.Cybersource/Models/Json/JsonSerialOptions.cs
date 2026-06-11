using System.Text.Json;
using System.Text.Json.Serialization;

namespace CybsClass.Cybersource.Models.Json
{
    public class JsonSerialOptions
    {
        public JsonSerializerOptions SerializerOptions { get; set; } = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
    }
}
