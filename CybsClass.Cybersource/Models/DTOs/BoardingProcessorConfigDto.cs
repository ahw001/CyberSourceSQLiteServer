using System.Text.Json.Serialization;
using CybsClass.Cybersource.Models.BaseData;

namespace CybsClass.Cybersource.Models.DTOs;

public class BoardingProcessorConfigDto
{
    public int BoardingProcessorConfigId { get; set; }

    public int BoardingCardProcessingConfigId { get; set; }

    public string? ProcessorName { get; set; }   // "vdcvantiv" | "fdiglobal"

    public string? BatchGroup { get; set; }

    // Acquirer (vdcvantiv)
    public string? AcquirerCountryCode { get; set; }
    public string? AcquirerFileDestinationBin { get; set; }
    public string? AcquirerInterbankCardAssociationId { get; set; }
    public string? AcquirerInstitutionId { get; set; }
    public string? AcquirerDiscoverInstitutionId { get; set; }
    public string? AcquirerMerchantId { get; set; }

    // Common flags
    public bool? EnableTransactionReferenceNumber { get; set; }
    public bool? EnablePosNetworkSwitching { get; set; }
    public string? MerchantTaxId { get; set; }

    // vdcvantiv specific
    public bool? AllowMultipleBills { get; set; }
    public string? BusinessApplicationId { get; set; }
    public bool? EnableAutoAuthReversalAfterVoid { get; set; }
    public bool? EnableExpresspayPanTranslation { get; set; }
    public bool? QuasiCash { get; set; }

    // Card-present
    public bool? DisablePointOfSaleTerminalIdValidation { get; set; }
    public bool? EnablePinTranslation { get; set; }
    public string? DefaultPointOfSaleTerminalId { get; set; }
    public string? PointOfSaleTerminalIds { get; set; }
    public bool? EnableMultipleTerminalIDs { get; set; }
    public bool? PinDebitEnablePartialAuth { get; set; }

    // Card-not-present
    public bool? RelaxAddressVerificationSystem { get; set; }
    public bool? RelaxAvsAllowZipWithoutCountry { get; set; }
    public bool? RelaxAvsAllowExpiredCard { get; set; }
    public bool? EnableEmsTransactionRiskScore { get; set; }

    // USD currency
    public bool? CurrencyUsdEnabled { get; set; }
    public bool? CurrencyUsdEnabledCardPresent { get; set; }
    public bool? CurrencyUsdEnabledCardNotPresent { get; set; }
    public string? CurrencyUsdMerchantId { get; set; }
    public string? CurrencyUsdTerminalId { get; set; }
    public string? CurrencyUsdTerminalIds { get; set; }

    public List<BoardingPaymentTypeDto> PaymentTypes { get; set; } = new();

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public class BoardingPaymentTypeDto
{
    public int BoardingProcessorPaymentTypeId { get; set; }

    public string? PaymentType { get; set; }   // "VISA" | "MASTERCARD" | "DISCOVER" | "AMERICAN_EXPRESS" | "JCB" | "DINERS_CLUB" | "PIN_DEBIT"

    public bool Enabled { get; set; }
    public string? MerchantId { get; set; }
    public string? TerminalId { get; set; }
    public bool? EnabledCardPresent { get; set; }
    public bool? EnabledCardNotPresent { get; set; }
}
