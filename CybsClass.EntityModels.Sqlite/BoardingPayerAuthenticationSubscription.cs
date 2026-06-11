using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingPayerAuthenticationSubscription")]
public class BoardingPayerAuthenticationSubscription
{
    [Key]
    public int BoardingPayerAuthenticationSubscriptionId { get; set; }

    public int? BoardingTransactingMerchantId { get; set; }

    public bool? Enabled { get; set; }

    [StringLength(50)]  public string? EnablementStatus { get; set; }
    [StringLength(50)]  public string? SelfServiceability { get; set; }
    [StringLength(50)]  public string? Distributability { get; set; }
    [StringLength(100)] public string? TemplateId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("BoardingTransactingMerchantId")]
    public virtual BoardingTransactingMerchant? BoardingTransactingMerchant { get; set; }

    [InverseProperty("BoardingPayerAuthenticationSubscription")]
    public virtual ICollection<BoardingPayerAuthenticationCardTypeConfig> CardTypeConfigs { get; set; }
        = new List<BoardingPayerAuthenticationCardTypeConfig>();
}

[Table("BoardingPayerAuthenticationCardTypeConfig")]
public class BoardingPayerAuthenticationCardTypeConfig
{
    [Key]
    public int BoardingPayerAuthenticationCardTypeConfigId { get; set; }

    public int BoardingPayerAuthenticationSubscriptionId { get; set; }

    [Required]
    [StringLength(100)]
    public string CardTypeName { get; set; } = null!;

    [ForeignKey("BoardingPayerAuthenticationSubscriptionId")]
    public virtual BoardingPayerAuthenticationSubscription BoardingPayerAuthenticationSubscription { get; set; } = null!;

    [InverseProperty("BoardingPayerAuthenticationCardTypeConfig")]
    public virtual ICollection<BoardingPayerAuthenticationCurrency> Currencies { get; set; }
        = new List<BoardingPayerAuthenticationCurrency>();
}

[Table("BoardingPayerAuthenticationCurrency")]
public class BoardingPayerAuthenticationCurrency
{
    [Key]
    public int BoardingPayerAuthenticationCurrencyId { get; set; }

    public int BoardingPayerAuthenticationCardTypeConfigId { get; set; }

    [StringLength(200)] public string? CurrencyCodes { get; set; }
    [StringLength(100)] public string? AcquirerId { get; set; }
    [StringLength(200)] public string? ProcessorMerchantId { get; set; }

    [ForeignKey("BoardingPayerAuthenticationCardTypeConfigId")]
    public virtual BoardingPayerAuthenticationCardTypeConfig BoardingPayerAuthenticationCardTypeConfig { get; set; } = null!;
}
