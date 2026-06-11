using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingValueAddedServicesSubscription")]
public class BoardingValueAddedServicesSubscription
{
    [Key]
    public int BoardingValueAddedServicesSubscriptionId { get; set; }

    public int? BoardingTransactingMerchantId { get; set; }

    public bool? TransactionSearchEnabled { get; set; }
    [StringLength(50)] public string? TransactionSearchEnablementStatus { get; set; }
    [StringLength(50)] public string? TransactionSearchSelfServiceability { get; set; }
    [StringLength(50)] public string? TransactionSearchDistributability { get; set; }

    public bool? ReportingEnabled { get; set; }
    [StringLength(50)] public string? ReportingEnablementStatus { get; set; }
    [StringLength(50)] public string? ReportingSelfServiceability { get; set; }
    [StringLength(50)] public string? ReportingDistributability { get; set; }

    public bool? DisputeManagementEnabled { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("BoardingTransactingMerchantId")]
    public virtual BoardingTransactingMerchant? BoardingTransactingMerchant { get; set; }
}
