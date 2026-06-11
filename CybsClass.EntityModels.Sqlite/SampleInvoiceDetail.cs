using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

public partial class SampleInvoiceDetail
{
    [Key]
    public int SampleInvoiceId { get; set; }

    [StringLength(255)]
    public string? CustomerInformationName { get; set; }

    [StringLength(255)]
    public string? CustomerInformationEmail { get; set; }

    [StringLength(50)]
    public string? CustomerInformationMerchantCustomerId { get; set; }

    [StringLength(255)]
    public string? CustomerInformationCompanyName { get; set; }

    [StringLength(50)]
    public string? InvoiceInformationInvoiceNumber { get; set; }

    public DateOnly? InvoiceInformationDueDate { get; set; }

    [StringLength(50)]
    public string? InvoiceInformationSendImmediately { get; set; }

    [StringLength(50)]
    public string? InvoiceInformationAllowPartialPayments { get; set; }

    [StringLength(50)]
    public string? InvoiceInformationDeliveryMode { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? OrderInformationAmountDetailsTotalAmount { get; set; }

    [StringLength(3)]
    public string? OrderInformationAmountDetailsCurrency { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? OrderInformationAmountDetailsDiscountAmount { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? OrderInformationAmountDetailsDiscountPercent { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? OrderInformationAmountDetailsSubAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? OrderInformationAmountDetailsMinimumPartialAmount { get; set; }

    [StringLength(50)]
    public string? OrderInformationAmountDetailsTaxDetailsType { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? OrderInformationAmountDetailsTaxDetailsAmount { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? OrderInformationAmountDetailsTaxDetailsRate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? OrderInformationAmountDetailsFreightAmount { get; set; }

    [StringLength(50)]
    public string? OrderInformationAmountDetailsFreightTaxable { get; set; }

    [StringLength(50)]
    public string? OrderInformationLineItemsProductSku { get; set; }

    public int? OrderInformationLineItemsQuantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? OrderInformationLineItemsUnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? OrderInformationLineItemsDiscountAmount { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? OrderInformationLineItemsDiscountRate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? OrderInformationLineItemsTaxAmount { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? OrderInformationLineItemsTaxRate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? OrderInformationLineItemsTotalAmount { get; set; }
}
