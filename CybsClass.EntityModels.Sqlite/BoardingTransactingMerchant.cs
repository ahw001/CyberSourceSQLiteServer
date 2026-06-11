using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingTransactingMerchant")]
public class BoardingTransactingMerchant
{
    [Key]
    public int BoardingTransactingMerchantId { get; set; }

    public int BoardingOrganizationId { get; set; }

    [Required]
    [StringLength(100)]
    public string TransactingOrganizationId { get; set; } = null!;

    [StringLength(50)]
    public string? Status { get; set; }

    [StringLength(50)]
    public string? Type { get; set; }

    public bool? Configurable { get; set; }

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

    [StringLength(50)]
    public string? BoardingProcessor { get; set; }   // "vdcvantiv" | "fdiglobal"

    [StringLength(100)]
    public string? BoardingPackageId { get; set; }

    [StringLength(50)]
    public string? CybersourceBoardingStatus { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("BoardingOrganizationId")]
    public virtual BoardingOrganization BoardingOrganization { get; set; } = null!;

    [InverseProperty("BoardingTransactingMerchant")]
    public virtual ICollection<BoardingTransactingMerchantSubscription> TransactingMerchantSubscriptions { get; set; } = new List<BoardingTransactingMerchantSubscription>();
}
