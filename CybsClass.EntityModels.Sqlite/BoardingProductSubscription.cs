using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingCardProductSubscription")]
public class BoardingCardProductSubscription
{
    [Key]
    public int BoardingProductSubscriptionId { get; set; }

    // Nullable — subscription is saved independently; linked to merchants via junction table
    public int? BoardingTransactingMerchantId { get; set; }

    [Required]
    [StringLength(100)]
    public string ProductName { get; set; } = null!;   // "cardProcessing" | "unifiedCheckout" | "virtualTerminal" | ...

    [Required]
    [StringLength(100)]
    public string ProductCategory { get; set; } = null!;   // "payments" | "commerceSolutions" | "valueAddedServices"

    public bool SubscriptionEnabled { get; set; }

    [StringLength(50)]
    public string? EnablementStatus { get; set; }

    [StringLength(50)]
    public string? SelfServiceability { get; set; }

    [StringLength(50)]
    public string? Distributability { get; set; }

    public bool? CardPresentEnabled { get; set; }

    public bool? CardNotPresentEnabled { get; set; }

    [StringLength(100)]
    public string? TemplateId { get; set; }

    [StringLength(50)]
    public string? CybersourceBoardingStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("BoardingCardProductSubscription")]
    public virtual ICollection<BoardingCardProcessingConfig> CardProcessingConfigs { get; set; } = new List<BoardingCardProcessingConfig>();

    [InverseProperty("BoardingCardProductSubscription")]
    public virtual ICollection<BoardingTransactingMerchantSubscription> TransactingMerchantSubscriptions { get; set; } = new List<BoardingTransactingMerchantSubscription>();
}
