using System.Text.Json.Serialization;

#nullable enable

namespace CybsClass.Cybersource.Models.BaseData.Boarding;

public sealed class PayByLinkBaseDto
{
    [JsonPropertyName("setups")]
    public PayByLinkSetups? Setups { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public sealed class PayByLinkSetups
{
    [JsonPropertyName("payments")]
    public PayByLinkPayments? Payments { get; set; }
}

public sealed class PayByLinkPayments
{
    [JsonPropertyName("payByLink")]
    public PayByLinkProduct? PayByLink { get; set; }
}

public sealed class PayByLinkProduct
{
    [JsonPropertyName("subscriptionInformation")]
    public BasicSubscriptionInformation? SubscriptionInformation { get; set; }
}
