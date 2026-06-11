using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

[Index("City", Name = "City")]
[Index("PostalCode", Name = "PostalCode")]
[Index("Region", Name = "Region")]
public partial class B2cCustomer
{
    [Key]
    public int B2cCustomerId { get; set; }

    [StringLength(40)]
    public string FirstName { get; set; } = null!;

    [StringLength(40)]
    public string LastName { get; set; } = null!;

    [StringLength(60)]
    public string? Email { get; set; }

    [StringLength(60)]
    public string? Address1 { get; set; }

    [StringLength(60)]
    public string? Address2 { get; set; }

    [StringLength(60)]
    public string? City { get; set; }

    [StringLength(60)]
    public string? Region { get; set; }

    [StringLength(30)]
    public string? PostalCode { get; set; }

    [StringLength(60)]
    public string? Country { get; set; }

    [StringLength(24)]
    public string? Phone { get; set; }

    [InverseProperty("B2cCustomer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("B2cCustomer")]
    public virtual ICollection<PaymentCardInfo> PaymentCardInfos { get; set; } = new List<PaymentCardInfo>();
}
