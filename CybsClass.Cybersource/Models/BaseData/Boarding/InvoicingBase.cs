using System.Text.Json.Serialization;

#nullable enable

namespace CybsClass.Cybersource.Models.BaseData.Boarding;

public sealed class InvoicingBaseDto
{
    [JsonPropertyName("setups")]
    public InvoicingSetups? Setups { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public sealed class InvoicingSetups
{
    [JsonPropertyName("payments")]
    public InvoicingPayments? Payments { get; set; }
}

public sealed class InvoicingPayments
{
    [JsonPropertyName("customerInvoicing")]
    public CustomerInvoicingProduct? CustomerInvoicing { get; set; }
}

public sealed class CustomerInvoicingProduct
{
    [JsonPropertyName("subscriptionInformation")]
    public BasicSubscriptionInformation? SubscriptionInformation { get; set; }
}

public sealed class BasicSubscriptionInformation
{
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("enablementStatus")]
    public string? EnablementStatus { get; set; }

    [JsonPropertyName("selfServiceability")]
    public string? SelfServiceability { get; set; }

    [JsonPropertyName("distributability")]
    public string? Distributability { get; set; }
}
