using System.Text.Json.Serialization;
using CybsClass.Cybersource.Models.BaseData;

namespace CybsClass.Cybersource.Models.DTOs;

public class BoardingCardProductSubscriptionDto
{
    public int BoardingProductSubscriptionId { get; set; }

    // Nullable — subscriptions are saved independently and linked to merchants via junction table
    public int? BoardingTransactingMerchantId { get; set; }

    public string? ProductName { get; set; }       // "cardProcessing" | "unifiedCheckout" | "virtualTerminal" | ...
    public string? ProductCategory { get; set; }   // "payments" | "commerceSolutions" | "valueAddedServices"

    public bool SubscriptionEnabled { get; set; }
    public string? EnablementStatus { get; set; }
    public string? SelfServiceability { get; set; }
    public string? Distributability { get; set; }

    public bool? CardPresentEnabled { get; set; }
    public bool? CardNotPresentEnabled { get; set; }

    public string? TemplateId { get; set; }

    // Card processing common config (populated for cardProcessing product only)
    public BoardingCardProcessingConfigDto? CardProcessingConfig { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public class BoardingTransactingMerchantSubscriptionDto
{
    public int BoardingTransactingMerchantSubscriptionId { get; set; }
    public int BoardingTransactingMerchantId { get; set; }
    public int BoardingProductSubscriptionId { get; set; }
    public bool IncludeInBoarding { get; set; } = true;

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public class IncludeInBoardingUpdateDto
{
    public bool Include { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public class BoardingCardProcessingConfigDto
{
    public int BoardingCardProcessingConfigId { get; set; }

    public string? DefaultAuthTypeCode { get; set; }
    public bool? EnablePartialAuth { get; set; }
    public string? MerchantCategoryCode { get; set; }
    public bool? EnableDuplicateRefNumBlocking { get; set; }
    public bool? AuthMerchantRetryDisabled { get; set; }
    public bool? IgnoreAddressVerificationSystem { get; set; }
    public bool? VisaStraightThroughProcessingOnly { get; set; }
    public string? CardPresentSolutionType { get; set; }
    public string? CardPresentProductSelected { get; set; }
    public bool? CpRelaxAddressVerificationSystem { get; set; }
    public bool? CpRelaxAvsAllowZipWithoutCountry { get; set; }
    public bool? CpRelaxAvsAllowExpiredCard { get; set; }

    public List<BoardingProcessorConfigDto> ProcessorConfigs { get; set; } = new();
}
