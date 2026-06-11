using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CybsClass.Cybersource.Models.OutboundTransObjects
{
    public class TaxDetailData
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("amount")]
        public string? Amount { get; set; }
        [JsonPropertyName("rate")]
        public string? Rate { get; set; }
    }
}
