using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

public partial class AuthTransResponse
{
    [Key]
    public int AuthTransResponsesId { get; set; }

    [StringLength(255)]
    public string Id { get; set; } = null!;

    public int OrderId { get; set; }

    [StringLength(255)]
    public string? ClientReferenceCode { get; set; }

    [StringLength(255)]
    public string? ConsumerAuthenticationToken { get; set; }

    public string? IssuerResponseRaw { get; set; }

    [StringLength(255)]
    public string? ReconciliationId { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    public DateTime? SubmitTimeUtc { get; set; }

    public string? Links { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AuthorizedAmount { get; set; }

    [StringLength(10)]
    public string? Currency { get; set; }

    [StringLength(50)]
    public string? CardType { get; set; }

    [StringLength(50)]
    public string? ConsumerAuthInfoToken { get; set; }

    [StringLength(50)]
    public string? ClientRefInfoCode { get; set; }

    [Column("POSInfoTerminalId")]
    [StringLength(50)]
    public string? PosinfoTerminalId { get; set; }

    [StringLength(50)]
    public string? ProcInfoPayAcctReferenceNumber { get; set; }

    [StringLength(50)]
    public string? ProcInfoMerchantNumber { get; set; }

    [StringLength(50)]
    public string? ProcInfoApprovalCode { get; set; }

    [StringLength(50)]
    public string? ProcInfoNetworkTransactionId { get; set; }

    [StringLength(50)]
    public string? ProcInfoTransactionId { get; set; }

    [StringLength(50)]
    public string? ProcInfoResponseCode { get; set; }

    [StringLength(50)]
    public string? AvsCode { get; set; }

    [StringLength(50)]
    public string? AvsCodeRaw { get; set; }

    [StringLength(50)]
    public string? TokenInformationInstIdNew { get; set; }

    [StringLength(50)]
    public string? TokenInformationInstId { get; set; }

    [StringLength(50)]
    public string? InstrumentIdentifierState { get; set; }

    [StringLength(50)]
    public string? InstrumentIdentifierId { get; set; }

    [StringLength(50)]
    public string? PaymentInstrumentId { get; set; }

    public string? ResponseTransactionJson { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("AuthTransResponses")]
    public virtual Order Order { get; set; } = null!;
}
