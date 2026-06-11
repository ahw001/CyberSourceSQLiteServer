using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingCardProcessingConfig")]
public class BoardingCardProcessingConfig
{
    [Key]
    public int BoardingCardProcessingConfigId { get; set; }

    public int BoardingProductSubscriptionId { get; set; }

    [StringLength(50)]
    public string? DefaultAuthTypeCode { get; set; }

    public bool? EnablePartialAuth { get; set; }

    [StringLength(10)]
    public string? MerchantCategoryCode { get; set; }

    public bool? EnableDuplicateRefNumBlocking { get; set; }

    public bool? AuthMerchantRetryDisabled { get; set; }

    public bool? IgnoreAddressVerificationSystem { get; set; }

    public bool? VisaStraightThroughProcessingOnly { get; set; }

    [StringLength(50)]
    public string? CardPresentSolutionType { get; set; }

    [StringLength(100)]
    public string? CardPresentProductSelected { get; set; }

    public bool? CpRelaxAddressVerificationSystem { get; set; }

    public bool? CpRelaxAvsAllowZipWithoutCountry { get; set; }

    public bool? CpRelaxAvsAllowExpiredCard { get; set; }

    [ForeignKey("BoardingProductSubscriptionId")]
    public virtual BoardingCardProductSubscription BoardingCardProductSubscription { get; set; } = null!;

    [InverseProperty("BoardingCardProcessingConfig")]
    public virtual ICollection<BoardingProcessorConfig> ProcessorConfigs { get; set; } = new List<BoardingProcessorConfig>();
}
