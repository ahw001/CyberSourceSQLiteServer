using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable

namespace CybsClass.Cybersource.Models.BaseData.Boarding;

public sealed class UnifiedCheckoutBaseDto
{
    [JsonPropertyName("setups")]
    public UnifiedCheckoutSetups? Setups { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public sealed class UnifiedCheckoutSetups
{
    [JsonPropertyName("payments")]
    public UnifiedCheckoutPayments? Payments { get; set; }
}

public sealed class UnifiedCheckoutPayments
{
    [JsonPropertyName("unifiedCheckout")]
    public UnifiedCheckoutProduct? UnifiedCheckout { get; set; }
}

public sealed class UnifiedCheckoutProduct
{
    [JsonPropertyName("subscriptionInformation")]
    public BasicSubscriptionInformation? SubscriptionInformation { get; set; }

    [JsonPropertyName("configurationInformation")]
    public UnifiedCheckoutConfigurationInformation? ConfigurationInformation { get; set; }
}

public sealed class UnifiedCheckoutConfigurationInformation
{
    [JsonPropertyName("configurations")]
    public UnifiedCheckoutConfigurations? Configurations { get; set; }

    [JsonPropertyName("configurationStatus")]
    public ConfigurationStatus? ConfigurationStatus { get; set; }
}

public sealed class UnifiedCheckoutConfigurations
{
    [JsonPropertyName("common")]
    public UnifiedCheckoutCommon? Common { get; set; }
}

public sealed class UnifiedCheckoutCommon
{
    [JsonPropertyName("allowedCardNetworks")]
    public List<string>? AllowedCardNetworks { get; set; }
}
