using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingProcessorPaymentType")]
public class BoardingProcessorPaymentType
{
    [Key]
    public int BoardingProcessorPaymentTypeId { get; set; }

    public int BoardingProcessorConfigId { get; set; }

    [Required]
    [StringLength(50)]
    public string PaymentType { get; set; } = null!;   // "VISA" | "MASTERCARD" | "DISCOVER" | "AMERICAN_EXPRESS" | "JCB" | "DINERS_CLUB" | "PIN_DEBIT"

    public bool Enabled { get; set; }

    [StringLength(100)]
    public string? MerchantId { get; set; }

    [StringLength(50)]
    public string? TerminalId { get; set; }

    public bool? EnabledCardPresent { get; set; }
    public bool? EnabledCardNotPresent { get; set; }

    [ForeignKey("BoardingProcessorConfigId")]
    public virtual BoardingProcessorConfig BoardingProcessorConfig { get; set; } = null!;
}
