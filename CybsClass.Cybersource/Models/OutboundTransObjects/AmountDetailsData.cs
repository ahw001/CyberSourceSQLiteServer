using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CybsClass.Cybersource.Models.OutboundTransObjects
{
    public class AmountDetailsData
    {
        [JsonPropertyName("authorizedAmount")]
        public string? AuthorizedAmount { get; set; }

        [JsonPropertyName("totalAmount")]
        public string? TotalAmount { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("discountAmount")]
        public string? DiscountAmount { get; set; }

        [JsonPropertyName("discountPercent")]
        public string? DiscountPercent { get; set; }

        [JsonPropertyName("subAmount")]
        public string? SubAmount { get; set; }

        [JsonPropertyName("minimumPartialAmount")]
        public string? MinimumPartialAmount { get; set; }

        [JsonPropertyName("taxDetails")]
        public TaxDetailData? TaxDetails { get; set; }
    }
}
