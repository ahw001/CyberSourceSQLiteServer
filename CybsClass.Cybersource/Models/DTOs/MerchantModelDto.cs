namespace CybsClass.Cybersource.Models.DTOs
{
    public class MerchantModelDto
    {
        public string? OrganizationId { get; set; }
        public string? ParentOrganizationId { get; set; }
        public string? TransactingOrganizationId { get; set; }
        public string? BoardingPackageId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? Configurable { get; set; }
        public string? Country { get; set; }
        public string? Address1 { get; set; }
        public string? PostalCode { get; set; }
        public string? AdministrativeArea { get; set; }
        public string? Locality { get; set; }
        public string? BusinessContactFirstName { get; set; }
        public string? BusinessContactLastName { get; set; }
        public string? BusinessContactPhoneNumber { get; set; }
        public string? BusinessContactEmail { get; set; }
        public string? TechnicalContactFirstName { get; set; }
        public string? TechnicalContactLastName { get; set; }
        public string? TechnicalContactPhoneNumber { get; set; }
        public string? TechnicalContactEmail { get; set; }
        public string? EmergencyContactFirstName { get; set; }
        public string? EmergencyContactLastName { get; set; }
        public string? EmergencyContactPhoneNumber { get; set; }
        public string? EmergencyContactEmail { get; set; }
        public string? Name { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? BusinessInformationPhoneNumber { get; set; }
        public string? BusinessInformationTimeZone { get; set; }
        public int? MerchantCategoryCode { get; set; }
        public string? error { get; set; }
        public bool VirtualTerminal { get; set; }
        public bool UnifiedCheckout { get; set; }
        public bool TokenManagementService { get; set; }
        public bool PayerAuthentication { get; set; }
        public bool PayByLink { get; set; }
        public bool CustomerInvoicing { get; set; }
        public bool Tax { get; set; }
        public bool DecisionManager { get; set; }
        public bool CardProcessing { get; set; }
        public bool OrganizationMerchant { get; set; }
        public bool StructuralMerchant { get; set; }
        public bool TransactingMerchant { get; set; }

        public bool CybsReadyTerminal { get; set; }

        public string? BoardingProcessor { get; set; }

        /// <summary>
        /// When provided, the transacting boarding request is driven by the stored
        /// ProductSubscription configuration rather than hardcoded defaults.
        /// </summary>
        public Guid? ProductSubscriptionId { get; set; }
    }
}
