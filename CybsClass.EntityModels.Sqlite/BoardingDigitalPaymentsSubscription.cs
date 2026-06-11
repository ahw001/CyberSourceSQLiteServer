using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingDigitalPaymentsSubscription")]
public class BoardingDigitalPaymentsSubscription
{
    [Key]
    public int BoardingDigitalPaymentsSubscriptionId { get; set; }

    // Nullable to allow subscriptions to exist independently of any merchant (many-to-many via junction).
    public int? BoardingTransactingMerchantId { get; set; }

    public bool? Enabled { get; set; }

    [StringLength(50)] public string? EnablementStatus { get; set; }
    [StringLength(50)] public string? SelfServiceability { get; set; }
    [StringLength(50)] public string? Distributability { get; set; }

    public bool? SamsungPayEnabled { get; set; }
    public bool? ApplePayEnabled { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("BoardingTransactingMerchantId")]
    public virtual BoardingTransactingMerchant? BoardingTransactingMerchant { get; set; }
}
