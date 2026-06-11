using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingTransactingMerchantSubscription")]
public class BoardingTransactingMerchantSubscription
{
    [Key]
    public int BoardingTransactingMerchantSubscriptionId { get; set; }

    public int BoardingTransactingMerchantId { get; set; }

    public int BoardingProductSubscriptionId { get; set; }

    public DateTime AssignedAt { get; set; }

    public bool IncludeInBoarding { get; set; } = true;

    [StringLength(50)]
    public string? CybersourceBoardingStatus { get; set; }

    [ForeignKey("BoardingTransactingMerchantId")]
    public virtual BoardingTransactingMerchant BoardingTransactingMerchant { get; set; } = null!;

    [ForeignKey("BoardingProductSubscriptionId")]
    public virtual BoardingCardProductSubscription BoardingCardProductSubscription { get; set; } = null!;
}
