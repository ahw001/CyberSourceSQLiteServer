using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

/// <summary>
/// Polymorphic junction — pairs a BoardingTransactingMerchant with any one of the
/// seven supplemental product subscription tables. ProductType is the discriminator
/// ("digitalPayments" | "customerInvoicing" | "payByLink" | "tokenManagement" |
///  "unifiedCheckout" | "valueAddedServices" | "virtualTerminal") and the
/// ProductSubscriptionId is the integer PK on the corresponding product table.
/// Referential integrity to the product row is enforced by the application layer.
/// </summary>
[Table("BoardingTransactingMerchantProductSubscription")]
public class BoardingTransactingMerchantProductSubscription
{
    [Key]
    public int BoardingTransactingMerchantProductSubscriptionId { get; set; }

    public int BoardingTransactingMerchantId { get; set; }

    [Required]
    [StringLength(50)]
    public string ProductType { get; set; } = null!;

    public int ProductSubscriptionId { get; set; }

    public DateTime AssignedAt { get; set; }

    public bool IncludeInBoarding { get; set; } = true;

    [StringLength(50)]
    public string? CybersourceBoardingStatus { get; set; }

    [ForeignKey("BoardingTransactingMerchantId")]
    public virtual BoardingTransactingMerchant BoardingTransactingMerchant { get; set; } = null!;
}
