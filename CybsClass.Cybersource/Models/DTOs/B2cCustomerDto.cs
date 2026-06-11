using CybsClass.Cybersource.Models.BaseData;
using CybsClass.EntityModels;
using System.Text.Json.Serialization;

namespace CybsClass.Cybersource.Models.DTOs;

public class B2cCustomerDto
{
    [JsonPropertyName("b2cCustomerId")]
    public int B2cCustomerId { get; set; }

    [JsonPropertyName("paymentCardId")]
    public string? PaymentCardId { get; set; }

    [JsonPropertyName("orderId")]
    public string? OrderId { get; set; }

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = null!;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = null!;

    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = null!;

    [JsonPropertyName("middleName")]
    public string? MiddleName { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("district")]
    public string? District { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    [JsonPropertyName("address2")]
    public string? Address2 { get; set; }

    [JsonPropertyName("address3")]
    public string? Address3 { get; set; }

    [JsonPropertyName("buildingNumber")]
    public string? BuildingNumber { get; set; }

    [JsonPropertyName("phoneType")]
    public string? PhoneType { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("administrativeArea")]
    public string? AdministrativeArea { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("bearerToken")]
    public string? BearerToken { get; set; }

    [JsonPropertyName("cloudPosType")]
    public string? CloudPosType { get; set; }

    [JsonPropertyName("merchantCustomerID")]
    public string? MerchantCustomerID { get; set; } = string.Empty;

    [JsonPropertyName("merchantReferenceCode")]
    public string? MerchantReferenceCode { get; set; }

    [JsonPropertyName("customerInstrumentId")]
    public string? CustomerInstrumentId { get; set; }

    [JsonPropertyName("instrumentIdentifier")]
    public string? InstrumentIdentifier { get; set; }

    [JsonPropertyName("paymentMethod")]
    public string? PaymentMethod { get; set; } = "Credit/Debit";

    [JsonPropertyName("preAuthOrOnDeviceTip")]
    public string? PreAuthOrOnDeviceTip { get; set; } = "Standard";

    [JsonPropertyName("invoiceType")]
    public string? InvoiceType { get; set; } = "Draft";

    [JsonPropertyName("shippingSameAsBilling")]
    public bool ShippingSameAsBilling { get; set; }

    [JsonPropertyName("saveFormData")]
    public bool SaveFormData { get; set; }

    [JsonPropertyName("performZeroAuth")]
    public bool PerformZeroAuth { get; set; }

    [JsonPropertyName("accountNumber")]
    public string? AccountNumber { get; set; } = null!;

    [JsonPropertyName("expMonth")]
    public string? ExpMonth { get; set; }

    [JsonPropertyName("expYear")]
    public string? ExpYear { get; set; }

    [JsonPropertyName("cvv")]
    public string? Cvv { get; set; }

    [JsonPropertyName("cardType")]
    public string? CardType { get; set; } = null!;

    [JsonPropertyName("transientToken")]
    public string? TransientToken { get; set; }

    [JsonPropertyName("transientTokenJwt")]
    public string? TransientTokenJwt { get; set; }

    [JsonPropertyName("markedForCapture")]
    public bool MarkedForCapture { get; set; }

    [JsonPropertyName("actionTokenTypes")]
    public string[]? ActionTokenTypes { get; set; }

    [JsonPropertyName("totalAmount")]
    public decimal? TotalAmount { get; set; }

    [JsonPropertyName("companyName")]
    public string? CompanyName { get; set; }

    [JsonPropertyName("companyAddress1")]
    public string? CompanyAddress1 { get; set; }

    [JsonPropertyName("companyAdministrativeArea")]
    public string? CompanyAdministrativeArea { get; set; }

    [JsonPropertyName("companyBuildingNumber")]
    public string? CompanyBuildingNumber { get; set; }

    [JsonPropertyName("companyCountry")]
    public string? CompanyCountry { get; set; }

    [JsonPropertyName("companyDistrict")]
    public string? CompanyDistrict { get; set; }

    [JsonPropertyName("companyLocality")]
    public string? CompanyLocality { get; set; }

    [JsonPropertyName("companyPostalCode")]
    public string? CompanyPostalCode { get; set; }

    [JsonPropertyName("shippingFirstName")]
    public string ShippingFirstName { get; set; } = null!;

    [JsonPropertyName("shippingLastName")]
    public string ShippingLastName { get; set; } = null!;

    [JsonPropertyName("shippingFullName")]
    public string ShippingFullName { get; set; } = null!;

    [JsonPropertyName("shippingEmail")]
    public string? ShippingEmail { get; set; }

    [JsonPropertyName("shippingAddress1")]
    public string? ShippingAddress1 { get; set; }

    [JsonPropertyName("shippingAddress2")]
    public string? ShippingAddress2 { get; set; }

    [JsonPropertyName("shippingAddress3")]
    public string? ShippingAddress3 { get; set; }

    [JsonPropertyName("shippingBuildingNumber")]
    public string? ShippingBuildingNumber { get; set; }

    [JsonPropertyName("shippingPhoneType")]
    public string? ShippingPhoneType { get; set; }

    [JsonPropertyName("shippingCity")]
    public string? ShippingCity { get; set; }

    [JsonPropertyName("shippingAdministrativeArea")]
    public string? ShippingAdministrativeArea { get; set; }

    [JsonPropertyName("shippingPostalCode")]
    public string? ShippingPostalCode { get; set; }

    [JsonPropertyName("shippingCountry")]
    public string? ShippingCountry { get; set; }

    [JsonPropertyName("shippingPhone")]
    public string? ShippingPhone { get; set; }

    [JsonPropertyName("freightAmount")]
    public string? FreightAmount { get; set; }

    [JsonPropertyName("taxableFreightAmount")]
    public string? TaxableFreightAmount { get; set; }

    [JsonPropertyName("taxDetailsType")]
    public string? TaxDetailsType { get; set; }

    [JsonPropertyName("taxDetailsAmount")]
    public string? TaxDetailsAmount { get; set; }

    [JsonPropertyName("taxDetailsRate")]
    public string? TaxDetailsRate { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; } = "USD";

    [JsonPropertyName("posTransId")]
    public string? PosTransId { get; set; }

    [JsonPropertyName("idType")]
    public string? IdType { get; set; }

    [JsonPropertyName("cloudStatusType")]
    public string? CloudStatusType { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("onDeviceTip")]
    public bool OnDeviceTip { get; set; } = false;

    [JsonPropertyName("preAuthOnly")]
    public bool PreAuthOnly { get; set; } = false;

    [JsonPropertyName("preAuthTip")]
    public bool PreAuthTip { get; set; }

    [JsonPropertyName("incrementalAuth")]
    public bool IncrementalAuth { get; set; }

    [JsonPropertyName("allowPartialAuth")]
    public bool AllowPartialAuth { get; set; }

    [JsonPropertyName("cloudPaymentMode")]
    public string? CloudPaymentMode { get; set; }

    [JsonPropertyName("posActivationCode")]
    public string? PosActivationCode { get; set; }

    [JsonPropertyName("posSetupCode")]
    public string? PosSetupCode { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("targetOrigin")]
    public string? TargetOrigin { get; set; }

    [JsonPropertyName("freight")]
    public Freight? Freight { get; set; } = new();

    [JsonPropertyName("amountDetails")]
    public AmountDetails? AmountDetails { get; set; } = new();

    [JsonPropertyName("lineItem")]
    public LineItems? LineItem { get; set; } = new();

    [JsonPropertyName("invoiceInformation")]
    public InvoiceInformation? InvoiceInformation { get; set; } = new();

    [JsonPropertyName("additionalInformation")]
    public AdditionalInformation? AdditionalInformation { get; set; } = new();

    [JsonPropertyName("cart")]
    public List<DBProductDto>? Cart { get; set; } = new();

    [JsonPropertyName("lineItems")]
    public List<LineItems> LineItems { get; set; } = new();

    [JsonPropertyName("errorObject")]
    public CybsClass.Cybersource.Models.BaseData.ErrorObject? ErrorObject { get; set; } = new();

    [JsonPropertyName("dbErrorObject")]
    public CybsClass.Cybersource.Models.BaseData.ErrorObject? DbErrorObject { get; set; } = new();

    [JsonPropertyName("payPalTransactionDetails")]
    public CybsClass.Cybersource.Models.BaseData.PayPalTransactionDetails? PayPalTransactionDetails { get; set; } = new();

}
