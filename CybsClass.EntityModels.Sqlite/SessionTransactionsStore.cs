using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

[Table("SessionTransactionsStore")]
public partial class SessionTransactionsStore
{
    [Key]
    public Guid Id { get; set; }

    public string SerializedData { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
