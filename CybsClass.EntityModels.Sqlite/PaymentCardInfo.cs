using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

[Table("PaymentCardInfo")]
public partial class PaymentCardInfo
{
    [Key]
    public int PaymentCardId { get; set; }

    public int B2cCustomerId { get; set; }

    [StringLength(40)]
    public string? AccountNumber { get; set; }

    [StringLength(40)]
    public string? TokenValue { get; set; }

    [StringLength(2)]
    public string? ExpMonth { get; set; }

    [StringLength(4)]
    public string? ExpYear { get; set; }

    [StringLength(3)]
    public string? Cvv { get; set; }

    [StringLength(40)]
    public string? PaymentAccountReferenceNumber { get; set; }

    [StringLength(40)]
    public string? TokenizedCardType { get; set; }

    [StringLength(40)]
    public string? InstrumentidentifierNew { get; set; }

    [StringLength(40)]
    public string? InstrumentIdentifierId { get; set; }

    [StringLength(40)]
    public string? InstrumentIdentifierState { get; set; }

    [StringLength(40)]
    public string? PaymentInstrumentId { get; set; }

    [StringLength(40)]
    public string? CustomerInstrumentId { get; set; }

    [Column("MerchantCustomerID")]
    [StringLength(40)]
    public string? MerchantCustomerId { get; set; }

    [StringLength(40)]
    public string? ShippingInstrumentId { get; set; }

    public string? ResponseTransactionJson { get; set; }

    [ForeignKey("B2cCustomerId")]
    [InverseProperty("PaymentCardInfos")]
    public virtual B2cCustomer B2cCustomer { get; set; } = null!;

    [InverseProperty("PaymentCard")]
    public virtual ICollection<NetworkTokenInfo> NetworkTokenInfos { get; set; } = new List<NetworkTokenInfo>();
}
