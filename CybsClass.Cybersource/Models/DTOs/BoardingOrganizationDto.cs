using System.Text.Json.Serialization;
using CybsClass.Cybersource.Models.BaseData;

namespace CybsClass.Cybersource.Models.DTOs;

public class BoardingOrganizationDto
{
    public int BoardingOrganizationId { get; set; }

    public string? OrganizationId { get; set; }
    public string? ParentOrganizationId { get; set; }
    public string? Status { get; set; }
    public string? Type { get; set; }
    public bool? Configurable { get; set; }
    public string? BoardingFlow { get; set; }
    public string? Mode { get; set; }
    public string? BoardingPackageId { get; set; }

    // Business information
    public string? BusinessName { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? BusinessPhoneNumber { get; set; }
    public string? TimeZone { get; set; }
    public string? MerchantCategoryCode { get; set; }

    // Address
    public string? AddressCountry { get; set; }
    public string? Address1 { get; set; }
    public string? PostalCode { get; set; }
    public string? AdministrativeArea { get; set; }
    public string? Locality { get; set; }

    // Business contact
    public string? BusinessContactFirstName { get; set; }
    public string? BusinessContactLastName { get; set; }
    public string? BusinessContactPhone { get; set; }
    public string? BusinessContactEmail { get; set; }

    // Technical contact
    public string? TechnicalContactFirstName { get; set; }
    public string? TechnicalContactLastName { get; set; }
    public string? TechnicalContactPhone { get; set; }
    public string? TechnicalContactEmail { get; set; }

    // Emergency contact
    public string? EmergencyContactFirstName { get; set; }
    public string? EmergencyContactLastName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactEmail { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}
