using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

[Table("StandAloneCredit")]
public partial class StandAloneCredit
{
    [Key]
    public int StandAloneCreditCardId { get; set; }

    public int? OrderId { get; set; }

    public int? B2cCustomerId { get; set; }

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
    public string? CreditAmount { get; set; }

    public string? ResponseTransactionJson { get; set; }
}
