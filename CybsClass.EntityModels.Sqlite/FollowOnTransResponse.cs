using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

public partial class FollowOnTransResponse
{
    [Key]
    public int TransResponseId { get; set; }

    [StringLength(100)]
    public string FollowOnTransResponseId { get; set; } = null!;

    [StringLength(255)]
    public string OriginalTransactionId { get; set; } = null!;

    public int OrderId { get; set; }

    [StringLength(255)]
    public string TransactionType { get; set; } = null!;

    public string? ResponseTransactionJson { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("FollowOnTransResponses")]
    public virtual Order Order { get; set; } = null!;
}
