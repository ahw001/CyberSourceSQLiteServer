using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CybsClass.Cybersource.Models.Json
{
    public class PosSetup
    {
        [JsonPropertyName("posId")]
        public string? PosId { get; set; }

        [JsonPropertyName("setupCode")]
        public string? SetupCode { get; set; }
    }
}
