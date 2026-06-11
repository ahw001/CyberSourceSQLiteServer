using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingVirtualTerminalReaderInfo")]
public class BoardingVirtualTerminalReaderInfo
{
    [Key]
    public int BoardingVirtualTerminalReaderInfoId { get; set; }

    public int BoardingVirtualTerminalSubscriptionId { get; set; }

    // terminalSupport
    public bool? EnableCreditCardCapture { get; set; }
    public bool? CardReaderPresent { get; set; }
    [StringLength(50)] public string? WisepadDecryptionService { get; set; }

    // terminalSettings
    public bool? EmvReaderUpdateCheck { get; set; }
    public bool? EmvEnabled { get; set; }
    public bool? EmvChipEnabled { get; set; }

    // _embedded
    public bool? DoesProcessorSupportsForcedCapture { get; set; }
    public bool? JapaneseMerchant { get; set; }
    public bool? ServiceFeeMerchantConfigFlagEnabled { get; set; }
    public bool? ProfileEnabled { get; set; }
    public bool? IsProfileEnabled { get; set; }

    [ForeignKey("BoardingVirtualTerminalSubscriptionId")]
    public virtual BoardingVirtualTerminalSubscription BoardingVirtualTerminalSubscription { get; set; } = null!;
}
