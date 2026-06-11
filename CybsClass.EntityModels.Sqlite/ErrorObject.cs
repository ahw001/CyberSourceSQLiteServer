using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

public partial class ErrorObject
{
    [Key]
    public int Id { get; set; }

    public string? Error { get; set; }

    public string? Message { get; set; }

    public string? Reason { get; set; }

    public string? Action { get; set; }

    public string? TransactionJson { get; set; }

    public DateTime CreatedAt { get; set; }
}
