using System.Text.Json.Serialization;

#nullable enable

namespace CybsClass.Cybersource.Models.BaseData.Boarding;

public sealed class DigitalPaymentsBaseDto
{
    [JsonPropertyName("setups")]
    public DigitalPaymentsSetups? Setups { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public sealed class DigitalPaymentsSetups
{
    [JsonPropertyName("payments")]
    public DigitalPaymentsPayments? Payments { get; set; }
}

public sealed class DigitalPaymentsPayments
{
    [JsonPropertyName("digitalPayments")]
    public DigitalPaymentsProduct? DigitalPayments { get; set; }
}

public sealed class DigitalPaymentsProduct
{
    [JsonPropertyName("subscriptionInformation")]
    public DigitalPaymentsSubscriptionInformation? SubscriptionInformation { get; set; }
}

public sealed class DigitalPaymentsSubscriptionInformation
{
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("enablementStatus")]
    public string? EnablementStatus { get; set; }

    [JsonPropertyName("selfServiceability")]
    public string? SelfServiceability { get; set; }

    [JsonPropertyName("features")]
    public DigitalPaymentsFeatures? Features { get; set; }

    [JsonPropertyName("distributability")]
    public string? Distributability { get; set; }
}

public sealed class DigitalPaymentsFeatures
{
    [JsonPropertyName("samsungPay")]
    public EnabledFlag? SamsungPay { get; set; }

    [JsonPropertyName("applePay")]
    public EnabledFlag? ApplePay { get; set; }
}
