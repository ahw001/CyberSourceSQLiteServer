using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

public partial class PaymentCardSampleDatum
{
    [Key]
    public int SamplePaymentCardId { get; set; }

    [StringLength(40)]
    public string CardBrand { get; set; } = null!;

    [StringLength(40)]
    public string AccountNumber { get; set; } = null!;

    [StringLength(2)]
    public string? ExpMonth { get; set; }

    [StringLength(4)]
    public string? ExpYear { get; set; }

    [StringLength(3)]
    public string? Cvv { get; set; }

    [Column("NT")]
    public bool Nt { get; set; }

    [StringLength(50)]
    public string? NtScenario { get; set; }
}
