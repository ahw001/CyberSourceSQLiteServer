using System.Text.Json.Serialization;
using CybsClass.Cybersource.Models.BaseData;

namespace CybsClass.Cybersource.Models.DTOs;

public class BoardingTransactingMerchantDto
{
    public int BoardingTransactingMerchantId { get; set; }

    public int BoardingOrganizationId { get; set; }

    public string? TransactingOrganizationId { get; set; }
    public string? Status { get; set; }
    public string? Type { get; set; }
    public bool? Configurable { get; set; }

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

    public string? BoardingProcessor { get; set; }   // "vdcvantiv" | "fdiglobal"
    public string? BoardingPackageId { get; set; }

    // Convenience: org OrganizationId for display in dropdowns
    public string? ParentOrganizationId { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}
