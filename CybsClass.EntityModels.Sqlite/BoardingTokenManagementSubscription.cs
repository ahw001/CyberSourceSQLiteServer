using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingTokenManagementSubscription")]
public class BoardingTokenManagementSubscription
{
    [Key]
    public int BoardingTokenManagementSubscriptionId { get; set; }

    public int? BoardingTransactingMerchantId { get; set; }

    public bool? Enabled { get; set; }

    [StringLength(50)] public string? EnablementStatus { get; set; }
    [StringLength(50)] public string? SelfServiceability { get; set; }
    [StringLength(50)] public string? Distributability { get; set; }

    [StringLength(50)] public string? CustomerTokenFormat { get; set; }
    [StringLength(50)] public string? InstrumentIdentifierCardTokenFormat { get; set; }
    [StringLength(50)] public string? PaymentInstrumentTokenFormat { get; set; }
    [StringLength(50)] public string? CardNumberMaskingFormat { get; set; }

    public bool? VisaTokenServiceEnabled { get; set; }
    public bool? VisaTokenTransactionEnabled { get; set; }
    public bool? MastercardTokenServiceEnabled { get; set; }
    public bool? MastercardTokenTransactionEnabled { get; set; }
    public bool? CreateInstrumentIdentifierOnSuccess { get; set; }
    public bool? CreateInstrumentIdentifierOnFailure { get; set; }

    [StringLength(50)]  public string? ConfigurationStatus { get; set; }
    [StringLength(500)] public string? ConfigurationMessage { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("BoardingTransactingMerchantId")]
    public virtual BoardingTransactingMerchant? BoardingTransactingMerchant { get; set; }
}
