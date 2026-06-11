using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable

namespace CybsClass.Cybersource.Models.BaseData.Boarding;

public sealed class PayerAuthenticationBaseDto
{
    [JsonPropertyName("setups")]
    public PayerAuthenticationSetups? Setups { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public sealed class PayerAuthenticationSetups
{
    [JsonPropertyName("payments")]
    public PayerAuthenticationPayments? Payments { get; set; }
}

public sealed class PayerAuthenticationPayments
{
    [JsonPropertyName("payerAuthentication")]
    public PayerAuthenticationProduct? PayerAuthentication { get; set; }
}

public sealed class PayerAuthenticationProduct
{
    [JsonPropertyName("subscriptionInformation")]
    public BasicSubscriptionInformation? SubscriptionInformation { get; set; }

    [JsonPropertyName("configurationInformation")]
    public PayerAuthenticationConfigurationInformation? ConfigurationInformation { get; set; }
}

public sealed class PayerAuthenticationConfigurationInformation
{
    [JsonPropertyName("templateId")]
    public string? TemplateId { get; set; }

    [JsonPropertyName("configurations")]
    public PayerAuthenticationConfigurations? Configurations { get; set; }
}

public sealed class PayerAuthenticationConfigurations
{
    [JsonPropertyName("cardTypes")]
    public Dictionary<string, PayerAuthenticationCardTypeConfig>? CardTypes { get; set; }
}

public sealed class PayerAuthenticationCardTypeConfig
{
    [JsonPropertyName("currencies")]
    public List<PayerAuthenticationCurrencyConfig>? Currencies { get; set; }
}

public sealed class PayerAuthenticationCurrencyConfig
{
    [JsonPropertyName("currencyCodes")]
    public List<string>? CurrencyCodes { get; set; }

    [JsonPropertyName("acquirerId")]
    public string? AcquirerId { get; set; }

    [JsonPropertyName("processorMerchantId")]
    public string? ProcessorMerchantId { get; set; }
}
