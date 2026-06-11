using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CybsClass.EntityModels;

public partial class MerchantSampleDatum
{
    [Key]
    public int SampleMerchantId { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? OrganizationId { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? Status { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? Type { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? Configurable { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? Country { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? Address1 { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? PostalCode { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? AdministrativeArea { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? Locality { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? BusinessContactFirstName { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? BusinessContactLastName { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? BusinessContactPhoneNumber { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? BusinessContactEmail { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? TechnicalContactFirstName { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? TechnicalContactLastName { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? TechnicalContactphoneNumber { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? TechnicalContactEmail { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? EmergencyContactFirstName { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? EmergencyContactLastName { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? EmergencyContactPhoneNumber { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? EmergencyContactEmail { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? Name { get; set; }

    [Unicode(false)]
    public string? WebsiteUrl { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? BusinessInformationPhoneNumber { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? BusinessInformationTimeZone { get; set; }

    public int? MerchantCategoryCode { get; set; }
}
