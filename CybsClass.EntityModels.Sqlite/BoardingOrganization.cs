using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingOrganization")]
public class BoardingOrganization
{
    [Key]
    public int BoardingOrganizationId { get; set; }

    [Required]
    [StringLength(100)]
    public string OrganizationId { get; set; } = null!;

    [StringLength(100)]
    public string? ParentOrganizationId { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [StringLength(50)]
    public string? Type { get; set; }

    public bool? Configurable { get; set; }

    [StringLength(50)]
    public string? BoardingFlow { get; set; }

    [StringLength(50)]
    public string? Mode { get; set; }

    [StringLength(100)]
    public string? BoardingPackageId { get; set; }

    [StringLength(200)]
    public string? BusinessName { get; set; }

    [StringLength(200)]
    public string? WebsiteUrl { get; set; }

    [StringLength(30)]
    public string? BusinessPhoneNumber { get; set; }

    [StringLength(100)]
    public string? TimeZone { get; set; }

    [StringLength(10)]
    public string? MerchantCategoryCode { get; set; }

    [StringLength(10)]
    public string? AddressCountry { get; set; }

    [StringLength(200)]
    public string? Address1 { get; set; }

    [StringLength(20)]
    public string? PostalCode { get; set; }

    [StringLength(10)]
    public string? AdministrativeArea { get; set; }

    [StringLength(100)]
    public string? Locality { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [InverseProperty("BoardingOrganization")]
    public virtual ICollection<BoardingContact> Contacts { get; set; } = new List<BoardingContact>();

    [InverseProperty("BoardingOrganization")]
    public virtual ICollection<BoardingTransactingMerchant> TransactingMerchants { get; set; } = new List<BoardingTransactingMerchant>();
}
