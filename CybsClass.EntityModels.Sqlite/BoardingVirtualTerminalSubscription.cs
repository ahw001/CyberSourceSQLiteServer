using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingVirtualTerminalSubscription")]
public class BoardingVirtualTerminalSubscription
{
    [Key]
    public int BoardingVirtualTerminalSubscriptionId { get; set; }

    public int? BoardingTransactingMerchantId { get; set; }

    public bool? Enabled { get; set; }

    [StringLength(50)] public string? EnablementStatus { get; set; }
    [StringLength(50)] public string? SelfServiceability { get; set; }
    [StringLength(50)] public string? Distributability { get; set; }
    [StringLength(50)] public string? ConfigurationStatus { get; set; }

    // common
    public bool? AllowECheckFields { get; set; }
    public bool? AllowLevel3Fields { get; set; }
    public bool? AllowServiceFeeFields { get; set; }
    public bool? ProductProfileEnabled { get; set; }
    [StringLength(10)]  public string? MerchantCountry { get; set; }
    public bool? AccountLevelEnabled { get; set; }
    [StringLength(50)]  public string? TokenProvider { get; set; }
    public bool? SecureStorageEnabled { get; set; }
    [StringLength(50)]  public string? OtsTokenClass { get; set; }
    [StringLength(100)] public string? OtsProfileId { get; set; }
    [StringLength(50)]  public string? CardProcessingType { get; set; }
    [StringLength(20)]  public string? DefaultTransactionMethod { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("BoardingTransactingMerchantId")]
    public virtual BoardingTransactingMerchant? BoardingTransactingMerchant { get; set; }

    [InverseProperty("BoardingVirtualTerminalSubscription")]
    public virtual BoardingVirtualTerminalGlobalPaymentInfo? GlobalPaymentInfo { get; set; }

    [InverseProperty("BoardingVirtualTerminalSubscription")]
    public virtual BoardingVirtualTerminalReceiptInfo? ReceiptInfo { get; set; }

    [InverseProperty("BoardingVirtualTerminalSubscription")]
    public virtual BoardingVirtualTerminalReaderInfo? ReaderInfo { get; set; }

    [InverseProperty("BoardingVirtualTerminalSubscription")]
    public virtual ICollection<BoardingVirtualTerminalAcceptedCardType> AcceptedCardTypes { get; set; }
        = new List<BoardingVirtualTerminalAcceptedCardType>();

    [InverseProperty("BoardingVirtualTerminalSubscription")]
    public virtual ICollection<BoardingVirtualTerminalMerchantDefinedField> MerchantDefinedFields { get; set; }
        = new List<BoardingVirtualTerminalMerchantDefinedField>();
}

[Table("BoardingVirtualTerminalAcceptedCardType")]
public class BoardingVirtualTerminalAcceptedCardType
{
    [Key]
    public int BoardingVirtualTerminalAcceptedCardTypeId { get; set; }

    public int BoardingVirtualTerminalSubscriptionId { get; set; }

    [Required]
    [StringLength(30)]
    public string ListType { get; set; } = null!;   // ACCEPTED | CVV_DISPLAY | CVV_REQUIRE

    [Required]
    [StringLength(50)]
    public string CardType { get; set; } = null!;

    [ForeignKey("BoardingVirtualTerminalSubscriptionId")]
    public virtual BoardingVirtualTerminalSubscription BoardingVirtualTerminalSubscription { get; set; } = null!;
}

[Table("BoardingVirtualTerminalMerchantDefinedField")]
public class BoardingVirtualTerminalMerchantDefinedField
{
    [Key]
    public int BoardingVirtualTerminalMerchantDefinedFieldId { get; set; }

    public int BoardingVirtualTerminalSubscriptionId { get; set; }

    public byte FieldIndex { get; set; }   // 1..20

    public bool? DisplayField { get; set; }
    public bool? RequiredField { get; set; }
    public bool? ShowReceipt { get; set; }
    public bool? ReceiptDisplayEnabled { get; set; }

    [ForeignKey("BoardingVirtualTerminalSubscriptionId")]
    public virtual BoardingVirtualTerminalSubscription BoardingVirtualTerminalSubscription { get; set; } = null!;
}
