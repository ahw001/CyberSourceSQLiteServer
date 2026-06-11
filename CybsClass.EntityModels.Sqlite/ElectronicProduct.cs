using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

public partial class ElectronicProduct
{
    [Key]
    public int ElectronicProductId { get; set; }

    [StringLength(40)]
    public string ProductName { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal? UnitPrice { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string? ProductSku { get; set; }

    public string? Picture { get; set; }

    public string? ProductLabel { get; set; }

    [StringLength(40)]
    public string? Brand { get; set; }
}
