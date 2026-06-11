using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

[Keyless]
[Table("CustomerCustomerDemo")]
public partial class CustomerCustomerDemo
{
    [StringLength(5)]
    public string CustomerId { get; set; } = null!;

    [Column("CustomerTypeID")]
    [StringLength(10)]
    public string CustomerTypeId { get; set; } = null!;
}
