using System.Text.Json.Serialization;

#nullable enable

namespace CybsClass.Cybersource.Models.BaseData.Boarding;

public sealed class TokenManagementBaseDto
{
    [JsonPropertyName("setups")]
    public TokenManagementSetups? Setups { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public sealed class TokenManagementSetups
{
    [JsonPropertyName("commerceSolutions")]
    public TokenManagementCommerceSolutions? CommerceSolutions { get; set; }
}

public sealed class TokenManagementCommerceSolutions
{
    [JsonPropertyName("tokenManagement")]
    public TokenManagementProduct? TokenManagement { get; set; }
}

public sealed class TokenManagementProduct
{
    [JsonPropertyName("subscriptionInformation")]
    public BasicSubscriptionInformation? SubscriptionInformation { get; set; }

    [JsonPropertyName("configurationInformation")]
    public TokenManagementConfigurationInformation? ConfigurationInformation { get; set; }
}

public sealed class TokenManagementConfigurationInformation
{
    [JsonPropertyName("configurations")]
    public TokenManagementConfigurations? Configurations { get; set; }

    [JsonPropertyName("configurationStatus")]
    public ConfigurationStatus? ConfigurationStatus { get; set; }
}

public sealed class TokenManagementConfigurations
{
    [JsonPropertyName("customerTokenFormat")]
    public string? CustomerTokenFormat { get; set; }

    [JsonPropertyName("instrumentIdentifierCardTokenFormat")]
    public string? InstrumentIdentifierCardTokenFormat { get; set; }

    [JsonPropertyName("paymentInstrumentTokenFormat")]
    public string? PaymentInstrumentTokenFormat { get; set; }

    [JsonPropertyName("cardNumberMaskingFormat")]
    public string? CardNumberMaskingFormat { get; set; }

    [JsonPropertyName("visaTokenServiceEnabled")]
    public bool? VisaTokenServiceEnabled { get; set; }

    [JsonPropertyName("visaTokenTransactionEnabled")]
    public bool? VisaTokenTransactionEnabled { get; set; }

    [JsonPropertyName("mastercardTokenServiceEnabled")]
    public bool? MastercardTokenServiceEnabled { get; set; }

    [JsonPropertyName("mastercardTokenTransactionEnabled")]
    public bool? MastercardTokenTransactionEnabled { get; set; }

    [JsonPropertyName("createInstrumentIdentifierOnSuccess")]
    public bool? CreateInstrumentIdentifierOnSuccess { get; set; }

    [JsonPropertyName("createInstrumentIdentifierOnFailure")]
    public bool? CreateInstrumentIdentifierOnFailure { get; set; }
}

public sealed class ConfigurationStatus
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
