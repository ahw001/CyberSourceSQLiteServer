using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingProcessorConfig")]
public class BoardingProcessorConfig
{
    [Key]
    public int BoardingProcessorConfigId { get; set; }

    public int BoardingCardProcessingConfigId { get; set; }

    [Required]
    [StringLength(50)]
    public string ProcessorName { get; set; } = null!;   // "vdcvantiv" | "fdiglobal"

    [StringLength(100)]
    public string? BatchGroup { get; set; }

    // Acquirer fields (vdcvantiv)
    [StringLength(50)]
    public string? AcquirerCountryCode { get; set; }

    [StringLength(50)]
    public string? AcquirerFileDestinationBin { get; set; }

    [StringLength(50)]
    public string? AcquirerInterbankCardAssociationId { get; set; }

    [StringLength(50)]
    public string? AcquirerInstitutionId { get; set; }

    [StringLength(50)]
    public string? AcquirerDiscoverInstitutionId { get; set; }

    [StringLength(100)]
    public string? AcquirerMerchantId { get; set; }

    // Common flags
    public bool? EnableTransactionReferenceNumber { get; set; }
    public bool? EnablePosNetworkSwitching { get; set; }

    [StringLength(50)]
    public string? MerchantTaxId { get; set; }

    // vdcvantiv specific
    public bool? AllowMultipleBills { get; set; }

    [StringLength(10)]
    public string? BusinessApplicationId { get; set; }

    public bool? EnableAutoAuthReversalAfterVoid { get; set; }
    public bool? EnableExpresspayPanTranslation { get; set; }
    public bool? QuasiCash { get; set; }

    // Card-present feature flags
    public bool? DisablePointOfSaleTerminalIdValidation { get; set; }
    public bool? EnablePinTranslation { get; set; }

    [StringLength(50)]
    public string? DefaultPointOfSaleTerminalId { get; set; }

    [StringLength(500)]
    public string? PointOfSaleTerminalIds { get; set; }

    public bool? EnableMultipleTerminalIDs { get; set; }
    public bool? PinDebitEnablePartialAuth { get; set; }

    // Card-not-present feature flags
    public bool? RelaxAddressVerificationSystem { get; set; }
    public bool? RelaxAvsAllowZipWithoutCountry { get; set; }
    public bool? RelaxAvsAllowExpiredCard { get; set; }
    public bool? EnableEmsTransactionRiskScore { get; set; }

    // Default USD currency
    public bool? CurrencyUsdEnabled { get; set; }
    public bool? CurrencyUsdEnabledCardPresent { get; set; }
    public bool? CurrencyUsdEnabledCardNotPresent { get; set; }

    [StringLength(100)]
    public string? CurrencyUsdMerchantId { get; set; }

    [StringLength(50)]
    public string? CurrencyUsdTerminalId { get; set; }

    [StringLength(500)]
    public string? CurrencyUsdTerminalIds { get; set; }

    [ForeignKey("BoardingCardProcessingConfigId")]
    public virtual BoardingCardProcessingConfig BoardingCardProcessingConfig { get; set; } = null!;

    [InverseProperty("BoardingProcessorConfig")]
    public virtual ICollection<BoardingProcessorPaymentType> PaymentTypes { get; set; } = new List<BoardingProcessorPaymentType>();
}
