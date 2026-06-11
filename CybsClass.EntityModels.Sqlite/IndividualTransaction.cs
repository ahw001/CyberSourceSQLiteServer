using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

public partial class IndividualTransaction
{
    [Key]
    public int TransactionId { get; set; }

    [StringLength(40)]
    public string RequestId { get; set; } = null!;

    [StringLength(40)]
    public string TransactionType { get; set; } = null!;

    [StringLength(40)]
    public string ReferenceTransactionId { get; set; } = null!;

    public string? ResponseTransactionJson { get; set; }
}
