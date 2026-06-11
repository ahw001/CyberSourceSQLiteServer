using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingUnifiedCheckoutSubscription")]
public class BoardingUnifiedCheckoutSubscription
{
    [Key]
    public int BoardingUnifiedCheckoutSubscriptionId { get; set; }

    public int? BoardingTransactingMerchantId { get; set; }

    public bool? Enabled { get; set; }

    [StringLength(50)]  public string? EnablementStatus { get; set; }
    [StringLength(50)]  public string? SelfServiceability { get; set; }
    [StringLength(50)]  public string? Distributability { get; set; }
    [StringLength(50)]  public string? ConfigurationStatus { get; set; }
    [StringLength(500)] public string? ConfigurationMessage { get; set; }

    public bool? ApplePayEnabled { get; set; }
    public bool? ClickToPayEnabled { get; set; }
    public bool? ECheckEnabled { get; set; }
    public bool? GooglePayEnabled { get; set; }
    public bool? DecisionManagerEnabled { get; set; }
    public bool? PayerAuthenticationEnabled { get; set; }
    public bool? TokenManagementEnabled { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("BoardingTransactingMerchantId")]
    public virtual BoardingTransactingMerchant? BoardingTransactingMerchant { get; set; }

    [InverseProperty("BoardingUnifiedCheckoutSubscription")]
    public virtual ICollection<BoardingUnifiedCheckoutAllowedCardNetwork> AllowedCardNetworks { get; set; }
        = new List<BoardingUnifiedCheckoutAllowedCardNetwork>();
}

[Table("BoardingUnifiedCheckoutAllowedCardNetwork")]
public class BoardingUnifiedCheckoutAllowedCardNetwork
{
    [Key]
    public int BoardingUnifiedCheckoutAllowedCardNetworkId { get; set; }

    public int BoardingUnifiedCheckoutSubscriptionId { get; set; }

    [Required]
    [StringLength(50)]
    public string CardNetwork { get; set; } = null!;

    [ForeignKey("BoardingUnifiedCheckoutSubscriptionId")]
    public virtual BoardingUnifiedCheckoutSubscription BoardingUnifiedCheckoutSubscription { get; set; } = null!;
}
