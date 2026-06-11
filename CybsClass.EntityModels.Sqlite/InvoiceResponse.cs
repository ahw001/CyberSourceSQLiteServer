using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

[Table("InvoiceResponse")]
public partial class InvoiceResponse
{
    [Key]
    public int InvoiceResponseId { get; set; }

    [StringLength(60)]
    public string InvoiceNumber { get; set; } = null!;

    public string TransactionJson { get; set; } = null!;
}
