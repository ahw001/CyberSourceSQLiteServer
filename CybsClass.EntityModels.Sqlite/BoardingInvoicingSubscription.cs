using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingInvoicingSubscription")]
public class BoardingInvoicingSubscription
{
    [Key]
    public int BoardingInvoicingSubscriptionId { get; set; }

    public int? BoardingTransactingMerchantId { get; set; }

    public bool? Enabled { get; set; }

    [StringLength(50)] public string? EnablementStatus { get; set; }
    [StringLength(50)] public string? SelfServiceability { get; set; }
    [StringLength(50)] public string? Distributability { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("BoardingTransactingMerchantId")]
    public virtual BoardingTransactingMerchant? BoardingTransactingMerchant { get; set; }
}
