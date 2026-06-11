using System.Text.Json.Serialization;

namespace CybsClass.Cybersource.Models.OutboundTransObjects
{
    public class LineItemData
    {
        [JsonPropertyName("productSku")]
        public string? ProductSku { get; set; }

        [JsonPropertyName("quantity")]
        public string? Quantity { get; set; }

        [JsonPropertyName("productName")]
        public string? ProductName { get; set; }

        [JsonPropertyName("unitPrice")]
        public string? UnitPrice { get; set; }

        [JsonPropertyName("discountAmount")]
        public string? DiscountAmount { get; set; }

        [JsonPropertyName("taxAmount")]
        public string? TaxAmount { get; set; }

        [JsonPropertyName("taxRate")]
        public string? TaxRate { get; set; }

        [JsonPropertyName("totalAmount")]
        public string? TotalAmount { get; set; }
    }
}
