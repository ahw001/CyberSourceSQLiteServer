using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

[Table("NetworkTokenInfo")]
public partial class NetworkTokenInfo
{
    [Key]
    public int PaymentTokenId { get; set; }

    public int PaymentCardId { get; set; }

    [StringLength(40)]
    public string TokenAccountNumber { get; set; } = null!;

    [StringLength(40)]
    public string? TokenValue { get; set; }

    [StringLength(2)]
    public string? TokenExpMonth { get; set; }

    [StringLength(4)]
    public string? TokenExpYear { get; set; }

    [StringLength(40)]
    public string? OriginalAccountSuffix { get; set; }

    [StringLength(40)]
    public string? OriginalAccountExpMonth { get; set; }

    [StringLength(40)]
    public string? OriginalAccountExpYear { get; set; }

    [StringLength(40)]
    public string? OriginalAccountNumber { get; set; }

    [StringLength(200)]
    public string? PaymentAccountReferenceNumber { get; set; }

    [StringLength(40)]
    public string? TokenizedCardType { get; set; }

    [StringLength(40)]
    public string? TokenState { get; set; }

    [StringLength(200)]
    public string? TokenRequestorId { get; set; }

    [StringLength(200)]
    public string? EnrollmentId { get; set; }

    [Column("MITPreviousTransactionId")]
    [StringLength(200)]
    public string? MitpreviousTransactionId { get; set; }

    public string? ResponseTransactionJson { get; set; }

    [ForeignKey("PaymentCardId")]
    [InverseProperty("NetworkTokenInfos")]
    public virtual PaymentCardInfo PaymentCard { get; set; } = null!;
}
