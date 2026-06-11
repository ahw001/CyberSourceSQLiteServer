using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

[Table("ShippingInstAddress")]
public partial class ShippingInstAddress
{
    [Key]
    [StringLength(60)]
    public string ShippingInstId { get; set; } = null!;

    [StringLength(60)]
    public string CustomerInstId { get; set; } = null!;

    [StringLength(40)]
    public string? FirstName { get; set; }

    [StringLength(40)]
    public string? LastName { get; set; }

    [StringLength(24)]
    public string? Phone { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(60)]
    public string? Address1 { get; set; }

    [StringLength(60)]
    public string? Address2 { get; set; }

    [StringLength(60)]
    public string? City { get; set; }

    [StringLength(60)]
    public string? Region { get; set; }

    [StringLength(60)]
    public string? PostalCode { get; set; }

    [StringLength(60)]
    public string? Country { get; set; }

    [StringLength(100)]
    public string? Company { get; set; }
}
