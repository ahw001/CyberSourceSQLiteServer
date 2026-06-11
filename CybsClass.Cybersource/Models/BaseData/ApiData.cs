using CybsClass.Blazor.Model.Cybersource.NT;
using CybsClass.Cybersource.Models.OutboundTransObjects;
using System.Net;
using System.Text.Json.Serialization;


namespace CybsClass.Cybersource.Models.BaseData;

public class PayPalTransactionDetails
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("merchantReferenceCode")]
    public string? MerchantReferenceCode { get; set; }

    [JsonPropertyName("returnUrl")]
    public string PayPalReturnUrl { get; set; } = string.Empty;

    [JsonPropertyName("successUrl")]
    public string PayPalSuccessUrl { get; set; } = string.Empty;

}

public class TransactionDetails
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("merchantReferenceCode")]
    public string? MerchantReferenceCode { get; set; }

    [JsonPropertyName("submitTimeUtc")]
    public string? SubmitTimeUtc { get; set; }

    [JsonPropertyName("captured")]
    public bool? Captured { get; set; }

    [JsonPropertyName("amountDetails")]
    public AmountDetails? AmountDetails { get; set; }
}

public class SenderInformation
{
    [JsonPropertyName("referenceNumber")]
    public string? ReferenceNumber { get; set; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("middleName")]
    public string? MiddleName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    [JsonPropertyName("address2")]
    public string? Address2 { get; set; }

    [JsonPropertyName("locality")]
    public string? Locality { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("administrativeArea")]
    public string? AdministrativeArea { get; set; }

    [JsonPropertyName("account")]
    public Account? Account { get; set; }

    [JsonPropertyName("personalIdType")]
    public Account? PersonalIdType { get; set; }
}

public class DeviceInformation
{
    [JsonPropertyName("ipAddress")]
    public string? IpAddress { get; set; }

    [JsonPropertyName("httpAcceptContent")]
    public string? HttpAcceptContent { get; set; }

    [JsonPropertyName("httpBrowserLanguage")]
    public string? HttpBrowserLanguage { get; set; }

    [JsonPropertyName("httpBrowserJavaEnabled")]
    public string? HttpBrowserJavaEnabled { get; set; }

    [JsonPropertyName("httpBrowserJavaScriptEnabled")]
    public string? HttpBrowserJavaScriptEnabled { get; set; }

    [JsonPropertyName("httpBrowserColorDepth")]
    public string? HttpBrowserColorDepth { get; set; }

    [JsonPropertyName("httpBrowserScreenHeight")]
    public string? HttpBrowserScreenHeight { get; set; }

    [JsonPropertyName("httpBrowserScreenWidth")]
    public string? HttpBrowserScreenWidth { get; set; }

    [JsonPropertyName("httpBrowserTimeDifference")]
    public string? HttpBrowserTimeDifference { get; set; }

    [JsonPropertyName("userAgentBrowserValue")]
    public string? UserAgentBrowserValue { get; set; }
}
public class Account
{
    [JsonPropertyName("number")]
    public string? Number { get; set; }
}

public class ErrorObject
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("action")]
    public string? Action { get; set; }

    [JsonPropertyName("TransactionJson")]
    public string? TransactionJson { get; set; }
}

public class DeviceDataCollectionInformation
{
    [JsonPropertyName("MessageType")]
    public string? MessageType { get; set; }

    [JsonPropertyName("SessionId")]
    public string? SessionId { get; set; }

    [JsonPropertyName("Status")]
    public bool? Status { get; set; }
}

public class RecipientInformation
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("middleName")]
    public string? MiddleName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("accountType")]
    public string? AccountType { get; set; }

    [JsonPropertyName("accountId")]
    public string? AccountId { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    [JsonPropertyName("address2")]
    public string? Address2 { get; set; }

    [JsonPropertyName("administrativeArea")]
    public string? AdministrativeArea { get; set; }

    [JsonPropertyName("locality")]
    public string? Locality { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }

}

public class MerchantInformation
{
    [JsonPropertyName("merchantCategoryCode")]
    public string? MerchantCategoryCode { get; set; }

    [JsonPropertyName("merchantDescriptor")]
    public MerchantDescriptor? MerchantDescriptor { get; set; }
}

public class MerchantDescriptor
{
    [JsonPropertyName("locality")]
    public string? Locality { get; set; }

    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    [JsonPropertyName("administrativeArea")]
    public string? AdministrativeArea { get; set; }
}

public class ClientReferenceInformation
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }
}

public class AdditionalInformation
{
    [JsonPropertyName("instrumentId")]
    public string? InstrumentId { get; set; }

    [JsonPropertyName("requestId")]
    public string? RequestId { get; set; }
}


public class ProcessingDetails
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("verificationMethod")]
    public string? VerificationMethod { get; set; }

    [JsonPropertyName("entryMode")]
    public string? EntryMode { get; set; }

    [JsonPropertyName("card")]
    public Card? Card { get; set; }
}

public class Receipts
{
    [JsonPropertyName("merchantReceipt")]
    public Receipt? MerchantReceipt { get; set; }

    [JsonPropertyName("customerReceipt")]
    public Receipt? CustomerReceipt { get; set; }
}

public class Receipt
{
    [JsonPropertyName("preformattedReceipt")]
    public string? PreformattedReceipt { get; set; }

    [JsonPropertyName("receiptData")]
    public ReceiptData? ReceiptData { get; set; }
}

public class ReceiptData
{
    [JsonPropertyName("signatureLineRequired")]
    public bool? SignatureLineRequired { get; set; }

    [JsonPropertyName("lines")]
    public Dictionary<string, ReceiptLine>? Lines { get; set; }

    [JsonPropertyName("tipLineRequired")]
    public bool? TipLineRequired { get; set; }

    [JsonPropertyName("totalLineRequired")]
    public bool? TotalLineRequired { get; set; }
}

public class ReceiptLine
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}

public class MerchantDetails
{
}

public class InstallmentDetails
{
    [JsonPropertyName("numberOfInstallments")]
    public int? NumberOfInstallments { get; set; }

    [JsonPropertyName("interestPlan")]
    public bool? InterestPlan { get; set; }

    [JsonPropertyName("governmentPlan")]
    public bool? GovernmentPlan { get; set; }
}


public class Contact
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }
}

public class BearerCreate
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("secret")]
    public string? Secret { get; set; }
}


public class AddressClass
{
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }

    [JsonPropertyName("administrativeArea")]
    public string? AdministrativeArea { get; set; }

    [JsonPropertyName("locality")]
    public string? Locality { get; set; }
}

public class SimpleServiceConfig
{
    [JsonPropertyName("subscriptionInformation")]
    public SubscriptionInformation? SubscriptionInformation { get; set; }

    [JsonPropertyName("configurationInformation")]
    public ConfigurationInformation? ConfigurationInformation { get; set; }
}

public class OrderInformationNumeric
{
    [JsonPropertyName("amountDetails")]
    public AmountDetailsNumeric? AmountDetails { get; set; } = new();

    [JsonPropertyName("billTo")]
    public BillTo? BilTo { get; set; } = new();

    [JsonPropertyName("shipTo")]
    public BillTo? ShipTo { get; set; } = new();

    [JsonPropertyName("freight")]
    public Freight? Freight { get; set; } = new();

    [JsonPropertyName("lineItems")]
    public List<LineItems>? LineItems { get; set; }

    [JsonPropertyName("taxDetails")]
    public TaxDetails? TaxDetails { get; set; }

}

public class AmountDetailsNumeric
{
    [JsonPropertyName("authorizedAmount")]
    public decimal? AuthorizedAmount { get; set; }

    [JsonPropertyName("totalAmount")]
    public decimal? TotalAmount { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("discountAmount")]
    public decimal? DiscountAmount { get; set; }

    [JsonPropertyName("discountPercent")]
    public decimal? DiscountPercent { get; set; }

    [JsonPropertyName("subAmount")]
    public decimal? SubAmount { get; set; }

    [JsonPropertyName("minimumPartialAmount")]
    public decimal? MinimumPartialAmount { get; set; }

    [JsonPropertyName("taxDetails")]
    public TaxDetails? TaxDetails { get; set; }
}

public class BusinessInformation
{
    [JsonPropertyName("address")]
    public AddressClass? Address { get; set; }

    [JsonPropertyName("businessContact")]
    public Contact? BusinessContact { get; set; }

    [JsonPropertyName("technicalContact")]
    public Contact? TechnicalContact { get; set; }

    [JsonPropertyName("emergencyContact")]
    public Contact? EmergencyContact { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("websiteUrl")]
    public string? WebsiteUrl { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("timeZone")]
    public string? TimeZone { get; set; }

    [JsonPropertyName("merchantCategoryCode")]
    public string? MerchantCategoryCode { get; set; }

}
public class TokenizedCard
{
    [JsonPropertyName("object")]
    public string? TokenObject { get; set; }

    [JsonPropertyName("id")]
    public string? id { get; set; }

    [JsonPropertyName("enrollmentId")]
    public string? EnrollmentId { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("tokenReferenceId")]
    public string? TokenReferenceId { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("expirationMonth")]
    public string? ExpirationMonth { get; set; }

    [JsonPropertyName("expirationYear")]
    public string? ExpirationYear { get; set; }

    [JsonPropertyName("securityCode")]
    public string? SecurityCode { get; set; }

    [JsonPropertyName("requestorId")]
    public string? RequestorId { get; set; }

    [JsonPropertyName("card")]
    public CardDetails? Card { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata? Metadata { get; set; }

    [JsonPropertyName("buyerInformation")]
    public BuyerInformation? BuyerInformation { get; set; }
}
public class Freight
{
    [JsonPropertyName("amount")]
    public string? Amount { get; set; }

    [JsonPropertyName("taxable")]
    public string? Taxable { get; set; }

    [JsonPropertyName("taxrate")]
    public string? TaxRate { get; set; }

}

public class LineItems
{
    [JsonPropertyName("productSku")]
    public string? ProductSku { get; set; }

    [JsonPropertyName("quantity")]
    public int? Quantity { get; set; }

    [JsonPropertyName("productName")]
    public string? ProductName { get; set; }

    [JsonPropertyName("unitPrice")]
    public decimal? UnitPrice { get; set; }

    [JsonPropertyName("discountAmount")]
    public decimal? DiscountAmount { get; set; }

    [JsonPropertyName("discountRate")]
    public decimal? DiscountRate { get; set; }

    [JsonPropertyName("taxAmount")]
    public decimal? TaxAmount { get; set; }

    [JsonPropertyName("taxRate")]
    public decimal? TaxRate { get; set; }

    [JsonPropertyName("totalAmount")]
    public decimal? TotalAmount { get; set; }
}
public class Href
{
    [JsonPropertyName("href")]
    public string? HrefValue { get; set; }
}

public class ShippingAddress
{
    [JsonPropertyName("_links")]
    public ShippingLinks? ShippingLinks { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("default")]
    public bool? Default { get; set; }

    [JsonPropertyName("shipTo")]
    public BillTo? ShipTo { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata? Metadata { get; set; }
}

public class ShippingLinks
{
    [JsonPropertyName("self")]
    public Href? Self { get; set; }

    [JsonPropertyName("customer")]
    public Href? Customer { get; set; }
}

public class InvoiceInformation
{
    public string? InvoiceNumber { get; set; }
    public string? Description { get; set; }
    public string? DueDate { get; set; }
    public bool SendImmediately { get; set; }
    public string? AllowPartialPayments { get; set; }
    public string? DeliveryMode { get; set; }
}

public class CardDetails
{
    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("suffix")]
    public string? Suffix { get; set; }

    [JsonPropertyName("expirationMonth")]
    public string? ExpirationMonth { get; set; }

    [JsonPropertyName("expirationYear")]
    public string? ExpirationYear { get; set; }
}

public class Metadata
{
    [JsonPropertyName("cardArt")]
    public CardArt? CardArt { get; set; }

    [JsonPropertyName("issuer")]
    public Issuer? Issuer { get; set; }
    [JsonPropertyName("creator")]
    public string? Creator { get; set; }
}

public class BillTo
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }
    [JsonPropertyName("company")]
    public Company? Company { get; set; }

    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }
    [JsonPropertyName("locality")]
    public string? Locality { get; set; }
    [JsonPropertyName("administrativeArea")]
    public string? AdministrativeArea { get; set; }
    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("middleName")]
    public string? MiddleName { get; set; }

    [JsonPropertyName("address2")]
    public string? Address2 { get; set; }

    [JsonPropertyName("address3")]
    public string? Address3 { get; set; }


    public string? District { get; set; }

    [JsonPropertyName("buildingNumber")]
    public string? BuildingNumber { get; set; }

    [JsonPropertyName("phoneType")]
    public string? PhoneType { get; set; }
    [JsonPropertyName("title")]
    public string? Title { get; set; }


}

public class BuyerInformation
{
    [JsonPropertyName("merchantCustomerID")]
    public string? MerchantCustomerID { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("mobilePhone")]
    public string? MobilePhone { get; set; }
}

public class CardArt
{
    [JsonPropertyName("brandLogoAsset")]
    public Asset? BrandLogoAsset { get; set; }

    [JsonPropertyName("issuerLogoAsset")]
    public Asset? IssuerLogoAsset { get; set; }

    [JsonPropertyName("iconAsset")]
    public Asset? IconAsset { get; set; }

    [JsonPropertyName("foregroundColor")]
    public string? ForegroundColor { get; set; }
}

public class Asset
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("mimeType")]
    public string? MimeType { get; set; }

    [JsonPropertyName("_links")]
    public Links? Links { get; set; }
}

public class Issuer
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("shortDescription")]
    public string? ShortDescription { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("paymentAccountReference")]
    public string? PaymentAccountReference { get; set; }

}

public class CardSummary
{
    [JsonPropertyName("number")]
    public string? Number { get; set; }
}

public class IssuerSummary
{
    [JsonPropertyName("paymentAccountReference")]
    public string? PaymentAccountReference { get; set; }
}

public class ErrorInformation
{
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }

}

public class ProcessingInformation
{
    [JsonPropertyName("actionList")]
    public string[]? ActionList { get; set; }

    [JsonPropertyName("actionTokenTypes")]
    public string[]? ActionTokenTypes { get; set; }

    [JsonPropertyName("capture")]
    public string? Capture { get; set; }

    [JsonPropertyName("authorizationOptions")]
    public AuthorizationOptions? AuthorizationOptions { get; set; }

    [JsonPropertyName("commerceIndicator")]
    public string? CommerceIndicator { get; set; }

    [JsonPropertyName("businessApplicationId")]
    public string? BusinessApplicationId { get; set; }

    [JsonPropertyName("purposeOfPayment")]
    public string? PurposeOfPayment { get; set; }
}

public class AuthReversal
{
    [JsonPropertyName("method")]
    public string? Method { get; set; }

    [JsonPropertyName("href")]
    public string? Href { get; set; }
}
public class AuthorizationOptions
{
    [JsonPropertyName("initiator")]
    public Initiator? Initiator { get; set; }

    [JsonPropertyName("aftIndicator")]
    public string? AftIndicator { get; set; }

    [JsonPropertyName("partialAuthIndicator")]
    public string? PartialAuthIndicator { get; set; }

    [JsonPropertyName("ignoreAvsResult")]
    public bool? IgnoreAvsResult { get; set; }
}

public class FlexFields
{
    [JsonPropertyName("paymentInformation")]
    public PaymentInformationFlex? PaymentInformation { get; set; } = new PaymentInformationFlex();
}

public class FlexCard
{
    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("expirationMonth")]
    public ExpirationMonth? ExpirationMonth { get; set; }

    [JsonPropertyName("expirationYear")]
    public ExpirationYear? ExpirationYear { get; set; }

    [JsonPropertyName("securityCode")]
    public SecurityCode? SecurityCode { get; set; }
}

public class CustomerInformation
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("merchantCustomerId")]
    public string? MerchantCustomerId { get; set; }

    [JsonPropertyName("company")]
    public Company? Company { get; set; }
}

public class ReversalInformation
{
    [JsonPropertyName("amountDetails")]
    public AmountDetails? AmountDetails { get; set; }

    [JsonPropertyName("billTo")]
    public BillTo? BillTo { get; set; }

    [JsonPropertyName("taxDetails")]
    public TaxDetails? TaxDetails { get; set; }

}

public class Company
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }
    [JsonPropertyName("administrativeArea")]
    public string? AdministrativeArea { get; set; }
    [JsonPropertyName("buildingNumber")]
    public string? BuildingNumber { get; set; }
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    [JsonPropertyName("district")]
    public string? District { get; set; }
    [JsonPropertyName("locality")]
    public string? Locality { get; set; }
    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }
}

public class PaymentInformationFlex
{
    [JsonPropertyName("tokenizedCard")]
    public Card? TokenizedCard { get; set; }

    [JsonPropertyName("card")]
    public FlexCard? Card { get; set; }

    [JsonPropertyName("securityCode")]
    public string? FlexSecurityCode { get; set; }

    [JsonPropertyName("expirationMonth")]
    public string? FlexExpirationMonth { get; set; }

    [JsonPropertyName("expirationYear")]
    public string? FlexExpirationYear { get; set; }
}

public class CaptureMandate
{
    [JsonPropertyName("billingType")]
    public string? BillingType { get; set; }
    [JsonPropertyName("requestEmail")]
    public bool RequestEmail { get; set; }
    [JsonPropertyName("requestPhone")]
    public bool RequestPhone { get; set; }
    [JsonPropertyName("requestShipping")]
    public bool RequestShipping { get; set; }
    [JsonPropertyName("shipToCountries")]
    public string[]? ShipToCountries { get; set; }
    [JsonPropertyName("showAcceptedNetworkIcons")]
    public bool? ShowAcceptedNetworkIcons { get; set; }
}

public class Initiator
{
    [JsonPropertyName("merchantInitiatedTransaction")]
    public MerchantInitiatedTransaction? MerchantInitiatedTransaction { get; set; }

    [JsonPropertyName("previousTransactionId")]
    public string? PreviousTransactionId { get; set; }
    [JsonPropertyName("originalAuthorizedAmount")]
    public string? OriginalAuthorizedAmount { get; set; }
    [JsonPropertyName("credentialStoredOnFile")]
    public string? CredentialStoredOnFile { get; set; }
    [JsonPropertyName("storedCredentialUsed")]
    public string? StoredCredentialUsed { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }

}

public class MerchantInitiatedTransaction
{
    [JsonPropertyName("previousTransactionId")]
    public string? PreviousTransactionId { get; set; }

    [JsonPropertyName("originalTransactionId")]
    public string? OriginalTransactionId { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}
public class Links
{
    [JsonPropertyName("authReversal")]
    public LinkDetails? AuthReversal { get; set; }

    [JsonPropertyName("self")]
    public LinkDetails? Self { get; set; }

    [JsonPropertyName("capture")]
    public LinkDetails? Capture { get; set; }
}

public class LinkDetails
{
    [JsonPropertyName("method")]
    public string? Method { get; set; }

    [JsonPropertyName("href")]
    public string? Href { get; set; }
}

public class ConsumerAuthenticationInformation
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("deviceDataCollectionUrl")]
    public string? DeviceDataCollectionUrl { get; set; }

    [JsonPropertyName("referenceId")]
    public string? ReferenceId { get; set; }

    [JsonPropertyName("returnUrl")]
    public string? ReturnUrl { get; set; }

    [JsonPropertyName("deviceChannel")]
    public string? DeviceChannel { get; set; }

    [JsonPropertyName("transactionMode")]
    public string? TransactionMode { get; set; }

    [JsonPropertyName("eciRaw")]
    public string? EciRaw { get; set; }

    [JsonPropertyName("authenticationTransactionId")]
    public string? AuthenticationTransactionId { get; set; }

    [JsonPropertyName("strongAuthentication")]
    public StrongAuthentication? StrongAuthentication { get; set; }

    [JsonPropertyName("eci")]
    public string? Eci { get; set; }

    [JsonPropertyName("cavv")]
    public string? Cavv { get; set; }

    [JsonPropertyName("paresStatus")]
    public string? ParesStatus { get; set; }

    [JsonPropertyName("acsReferenceNumber")]
    public string? AcsReferenceNumber { get; set; }

    [JsonPropertyName("xid")]
    public string? Xid { get; set; }

    [JsonPropertyName("directoryServerTransactionId")]
    public string? DirectoryServerTransactionId { get; set; }

    [JsonPropertyName("veresEnrolled")]
    public string? VeresEnrolled { get; set; }

    [JsonPropertyName("threeDSServerTransactionId")]
    public string? ThreeDSServerTransactionId { get; set; }

    [JsonPropertyName("acsOperatorID")]
    public string? AcsOperatorID { get; set; }

    [JsonPropertyName("ecommerceIndicator")]
    public string? EcommerceIndicator { get; set; }

    [JsonPropertyName("specificationVersion")]
    public string? SpecificationVersion { get; set; }

    [JsonPropertyName("acsTransactionId")]
    public string? AcsTransactionId { get; set; }

    [JsonPropertyName("acsUrl")]
    public string? AcsUrl { get; set; }

    [JsonPropertyName("stepUpUrl")]
    public string? StepUpUrl { get; set; }

    [JsonPropertyName("pareq")]
    public string? Pareq { get; set; }

    [JsonPropertyName("proofXml")]
    public string? ProofXml { get; set; }

    [JsonPropertyName("authenticationResult")]
    public string? AuthenticationResult { get; set; }

    [JsonPropertyName("indicator")]
    public string? Indicator { get; set; }

    [JsonPropertyName("ucafAuthenticationData")]
    public string? UcafAuthenticationData { get; set; }

    [JsonPropertyName("ucafCollectionIndicator")]
    public string? UcafCollectionIndicator { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

}

public class StrongAuthentication
{
    [JsonPropertyName("outageExemptionIndicator")]
    public string? OutageExemptionIndicator { get; set; }
}

public class IssuerInformation
{
    [JsonPropertyName("responseRaw")]
    public string? ResponseRaw { get; set; }
}

public class CustomerTokenShippingAddress
{
    [JsonPropertyName("default")]
    public string? Default { get; set; }

    [JsonPropertyName("shipTo")]
    public BillTo? ShipTo { get; set; }
}

public class SemiPosTransactionRequest
{

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("merchantReferenceCode")]
    public string? MerchantReferenceCode { get; set; }

    [JsonPropertyName("amountDetails")]
    public AmountDetails? AmountDetails { get; set; }

}

public class CustomerPaymentInstrument
{
    [JsonPropertyName("default")]
    public string? Default { get; set; }

    [JsonPropertyName("card")]
    public FullCard? Card { get; set; }

    [JsonPropertyName("billTo")]
    public BillTo? BillTo { get; set; }

    [JsonPropertyName("instrumentIdentifier")]
    public InstrumentIdentifier? InstrumentIdentifier { get; set; }

}

public class OrderInformation
{
    [JsonPropertyName("amountDetails")]
    public AmountDetails? AmountDetails { get; set; } = new();

    [JsonPropertyName("billTo")]
    public BillTo? BillTo { get; set; } = new();

    [JsonPropertyName("shipTo")]
    public BillTo? ShipTo { get; set; } = new();

    [JsonPropertyName("freight")]
    public Freight? Freight { get; set; } = new();

    [JsonPropertyName("lineItems")]
    public List<LineItems>? LineItems { get; set; }

    [JsonPropertyName("taxDetails")]
    public TaxDetails? TaxDetails { get; set; }

    [JsonPropertyName("discountAmount")]
    public string? DiscountAmount { get; set; }

    [JsonPropertyName("discountPercent")]
    public string? DiscountPercent { get; set; }

    [JsonPropertyName("subAmount")]
    public string? SubAmount { get; set; }

    [JsonPropertyName("minimumPartialAmount")]
    public string? MinimumPartialAmount { get; set; }

     //[JsonPropertyName("freight")]
    //public Freight? Freight { get; set; } = new();
}

public class AmountDetails
{
    [JsonPropertyName("authorizedAmount")]
    public string? AuthorizedAmount { get; set; }

    [JsonPropertyName("totalAmount")]
    public string? TotalAmount { get; set; }

    [JsonPropertyName("amount")]
    public string? Amount { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("discountAmount")]
    public string? DiscountAmount { get; set; }

    [JsonPropertyName("discountPercent")]
    public string? DiscountPercent { get; set; }

    [JsonPropertyName("subAmount")]
    public string? SubAmount { get; set; }

    [JsonPropertyName("minimumPartialAmount")]
    public string? MinimumPartialAmount { get; set; }

    [JsonPropertyName("taxDetails")]
    public TaxDetails? TaxDetails { get; set; }

    [JsonPropertyName("capturedAmount")]
    public string? CapturedAmount { get; set; }

    [JsonPropertyName("refundableAmount")]
    public string? RefundableAmount { get; set; }
}

public class PaymentAccountInformation
{
    [JsonPropertyName("card")]
    public Card? Card { get; set; }
}

public class PaymentInformation
{
    [JsonPropertyName("tokenizedCard")]
    public Card? TokenizedCard { get; set; }

    [JsonPropertyName("card")]
    public FullCard? Card { get; set; }

    [JsonPropertyName("instrumentIdentifier")]
    public InstrumentIdentifier? InstrumentIdentifier { get; set; }

    [JsonPropertyName("paymentInstrument")]
    public PaymentInstrument? PaymentInstrument { get; set; }

    [JsonPropertyName("scheme")]
    public string? Scheme { get; set; }

    [JsonPropertyName("bin")]
    public string? Bin { get; set; }

    [JsonPropertyName("accountType")]
    public string? AccountType { get; set; }

    [JsonPropertyName("issuer")]
    public string? Issuer { get; set; }

    [JsonPropertyName("binCountry")]
    public string? BinCountry { get; set; }

}


public class TaxDetails
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("amount")]
    public string? Amount { get; set; }

    [JsonPropertyName("rate")]
    public string? Rate { get; set; }

    [JsonPropertyName("salesSlipNumber")]
    public int? SalesSlipNumber { get; set; }

}

public class FullCard
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("securityCode")]
    public string? SecurityCode { get; set; }

    [JsonPropertyName("expirationMonth")]
    public string? ExpirationMonth { get; set; }

    [JsonPropertyName("expirationYear")]
    public string? ExpirationYear { get; set; }

}

public class Card
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("securityCode")]
    public SecurityCode? SecurityCode { get; set; }

    [JsonPropertyName("expirationMonth")]
    public string? ExpirationMonth { get; set; }

    [JsonPropertyName("expirationYear")]
    public string? ExpirationYear { get; set; }

    [JsonPropertyName("maskedPan")]
    public string? MaskedPan { get; set; }

    [JsonPropertyName("brandName")]
    public string? BrandName { get; set; }


}

public class SecurityCode
{
    [JsonPropertyName("required")]
    public bool? Required { get; set; }
}

public class ExpirationMonth
{
    [JsonPropertyName("required")]
    public bool? Required { get; set; }

}

public class ExpirationYear
{
    [JsonPropertyName("required")]
    public bool? Required { get; set; }
}

public class Type
{
    [JsonPropertyName("required")]
    public bool? Required { get; set; }
}

public class PointOfSaleInformation
{
    [JsonPropertyName("terminalId")]
    public string? TerminalId { get; set; }
}

public class ProcessorInformation
{
    [JsonPropertyName("paymentAccountReferenceNumber")]
    public string? PaymentAccountReferenceNumber { get; set; }

    [JsonPropertyName("merchantNumber")]
    public string? MerchantNumber { get; set; }

    [JsonPropertyName("approvalCode")]
    public string? ApprovalCode { get; set; }

    [JsonPropertyName("networkTransactionId")]
    public string? NetworkTransactionId { get; set; }

    [JsonPropertyName("transactionId")]
    public string? TransactionId { get; set; }

    [JsonPropertyName("responseCode")]
    public string? ResponseCode { get; set; }

    [JsonPropertyName("avs")]
    public Avs? Avs { get; set; }

}


public class RiskInformation
{
    [JsonPropertyName("localTime")]
    public string? LocalTime { get; set; }

    [JsonPropertyName("score")]
    public Score? Score { get; set; }

    [JsonPropertyName("infoCodes")]
    public Infocodes? InfoCodes { get; set; }

    [JsonPropertyName("profile")]
    public Profile? Profile { get; set; }

    [JsonPropertyName("casePriority")]
    public string? CasePriority { get; set; }
}


public class Infocodes
{
    [JsonPropertyName("address")]
    public string[]? Address { get; set; }

    [JsonPropertyName("suspicious")]
    public string[]? Suspicious { get; set; }

    [JsonPropertyName("identityChange")]
    public string[]? IdentityChange { get; set; }
}

public class Profile
{
    [JsonPropertyName("earlyDecision")]
    public string? EarlyDecision { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("action")]
    public string? Action { get; set; }

    [JsonPropertyName("selectorRule")]
    public string? SelectorRule { get; set; }
}

public class Score
{
    [JsonPropertyName("result")]
    public string? Result { get; set; }

    [JsonPropertyName("factorCodes")]
    public string[]? FactorCodes { get; set; }

    [JsonPropertyName("modelUsed")]
    public string? ModelUsed { get; set; }
}

public class PosTransactionRequest
{
    [JsonPropertyName("serialNumber")]
    public string? SerialNumber { get; set; }

    [JsonPropertyName("request")]
    public Request? Request { get; set; }

    [JsonPropertyName("merchantInformation")]
    public MerchantInformation? MerchantInformation { get; set; }

}

public class Request
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("merchantReferenceCode")]
    public string? MerchantReferenceCode { get; set; }

    [JsonPropertyName("askForTip")]
    public string? AskForTip { get; set; }

    [JsonPropertyName("amountDetails")]
    public AmountDetails? AmountDetails { get; set; }

    [JsonPropertyName("id")]
    public string? TransactionId { get; set; }

    [JsonPropertyName("transactionId")]
    public string? OriginalTransactionId { get; set; }

    [JsonPropertyName("idType")]
    public string? IdType { get; set; }

    [JsonPropertyName("instrumentId")]
    public string? InstrumentId { get; set; }

    [JsonPropertyName("paymentMode")]
    public string? PaymentMode { get; set; }

    [JsonPropertyName("capture")]
    public bool Capture { get; set; }

}

public class PosReturnRequest
{
    [JsonPropertyName("serialNumber")]
    public string? SerialNumber { get; set; }

    [JsonPropertyName("request")]
    public Request? Request { get; set; }
}

public class Avs
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("codeRaw")]
    public string? CodeRaw { get; set; }
}

public class TokenInformation
{
    [JsonPropertyName("instrumentidentifierNew")]
    public bool? InstrumentIdentifierNew { get; set; }

    [JsonPropertyName("instrumentIdentifier")]
    public InstrumentIdentifier? InstrumentIdentifier { get; set; }

    [JsonPropertyName("shippingAddress")]
    public ShippingAddress? ShippingAddress { get; set; }

    [JsonPropertyName("paymentInstrument")]
    public PaymentInstrument? PaymentInstrument { get; set; }

    [JsonPropertyName("customer")]
    public Customer? Customer { get; set; }

    [JsonPropertyName("object")]
    public string? TokenObject { get; set; }

    [JsonPropertyName("id")]
    public string? id { get; set; }

    [JsonPropertyName("enrollmentId")]
    public string? EnrollmentId { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("tokenReferenceId")]
    public string? TokenReferenceId { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("expirationMonth")]
    public string? ExpirationMonth { get; set; }

    [JsonPropertyName("expirationYear")]
    public string? ExpirationYear { get; set; }

    [JsonPropertyName("securityCode")]
    public string? SecurityCode { get; set; }

    [JsonPropertyName("requestorId")]
    public string? RequestorId { get; set; }

    [JsonPropertyName("card")]
    public CardDetails? Card { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata? Metadata { get; set; }

    [JsonPropertyName("buyerInformation")]
    public BuyerInformation? BuyerInformation { get; set; }

    [JsonPropertyName("transientTokenJwt")]
    public string? TransientTokenJwt { get; set; }

    [JsonPropertyName("transientToken")]
    public string? TransientToken { get; set; }

}

public class InstrumentIdentifier
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class PaymentInstrument
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class Customer
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class Self
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }
}

public class PaymentInstruments
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }
}

public class EmbeddedInstrumentIdentifier
{
    [JsonPropertyName("_links")]
    public Links? Links { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("object")]
    public string? Object { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("card")]
    public Card? Card { get; set; }

    [JsonPropertyName("issuer")]
    public Issuer? Issuer { get; set; }

    [JsonPropertyName("processingInformation")]
    public ProcessingInformation? ProcessingInformation { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata? Metadata { get; set; }
}

public class Embedded
{
    [JsonPropertyName("instrumentIdentifier")]
    public EmbeddedInstrumentIdentifier? InstrumentIdentifier { get; set; }
}


public class OrganizationInformation
{

    [JsonPropertyName("organizationId")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("parentOrganizationId")]
    public string? ParentOrganizationId { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("configurable")]
    public bool? Configurable { get; set; }

    [JsonPropertyName("businessInformation")]
    public BusinessInformation? BusinessInformation { get; set; }
}

public class OutboundBoardingRoot
{
    [JsonPropertyName("registrationInformation")]
    public RegistrationInformation? RegistrationInformation { get; set; }

    [JsonPropertyName("organizationInformation")]
    public OrganizationInformation? OrganizationInformation { get; set; }

    [JsonPropertyName("productInformation")]
    public ProductInformation? ProductInformation { get; set; }
}

public class OutboundBoardingResponse
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("submitTimeUtc")]
    public DateTime? SubmitTimeUtc { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("registrationInformation")]
    public RegistrationInformation? RegistrationInformation { get; set; }

    [JsonPropertyName("organizationInformation")]
    public OrganizationInformation? OrganizationInformation { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}


public class RegistrationInformation
{
    [JsonPropertyName("boardingFlow")]
    public string? BoardingFlow { get; set; }

    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    [JsonPropertyName("boardingPackageId")]
    public string? BoardingPackageId { get; set; }
}

public class MerchantBoardingResponse
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("submitTimeUtc")]
    public DateTime? SubmitTimeUtc { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("registrationInformation")]
    public RegistrationInformation? RegistrationInformation { get; set; }

    [JsonPropertyName("organizationInformation")]
    public OrganizationInformation? OrganizationInformation { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

public class Address
{
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    [JsonPropertyName("locality")]
    public string? Locality { get; set; }

    [JsonPropertyName("administrativeArea")]
    public string? AdministrativeArea { get; set; }

    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }
}

public class ProductInformation
{
    [JsonPropertyName("selectedProducts")]
    public SelectedProducts? SelectedProducts { get; set; }

}

public class SelectedProducts
{
    [JsonPropertyName("payments")]
    public Payments? Payments { get; set; }

    [JsonPropertyName("organizationId")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("risk")]
    public Risk? Risk { get; set; }

    [JsonPropertyName("commerceSolutions")]
    public CommerceSolutions? CommerceSolutions { get; set; }

    [JsonPropertyName("valueAddedServices")]
    public ValueAddedServices? ValueAddedServices { get; set; }



}

public class Payments
{
    [JsonPropertyName("cardProcessing")]
    public CardProcessing? CardProcessing { get; set; }

    [JsonPropertyName("cardPresentConnect")]
    public CardPresentConnect? CardPresentConnect { get; set; }

    [JsonPropertyName("unifiedCheckout")]
    public SimpleServiceConfig? UnifiedCheckout { get; set; }

    [JsonPropertyName("virtualTerminal")]
    public SimpleServiceConfig? VirtualTerminal { get; set; }

    [JsonPropertyName("customerInvoicing")]
    public SimpleServiceConfig? CustomerInvoicing { get; set; }

    [JsonPropertyName("payByLink")]
    public SimpleServiceConfig? PayByLink { get; set; }

    [JsonPropertyName("tax")]
    public SimpleServiceConfig? Tax { get; set; }

    [JsonPropertyName("cybsReadyTerminal")]
    public SimpleServiceConfig? CybsReadyTerminal { get; set; }

    [JsonPropertyName("payerAuthentication")]
    public PayerAuthentication? PayerAuthentication { get; set; }

}

public class Risk
{

}

public class CommerceSolutions
{
    [JsonPropertyName("tokenManagement")]
    public SimpleServiceConfig? TokenManagement { get; set; }
}

public class CardProcessing
{
    [JsonPropertyName("subscriptionInformation")]
    public SubscriptionInformation? SubscriptionInformation { get; set; }

    [JsonPropertyName("configurationInformation")]
    public ConfigurationInformation? ConfigurationInformation { get; set; }

    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("selfServiceability")]
    public string? SelfServiceability { get; set; }
}

public class SubscriptionInformation
{
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("features")]
    public SubscriptionFeatures? Features { get; set; }

    [JsonPropertyName("selfServiceability")]
    public string? SelfServiceability { get; set; }

    [JsonPropertyName("enablementStatus")]
    public string? EnablementStatus { get; set; }

    [JsonPropertyName("distributability")]
    public string? Distributability { get; set; }
}

public class Features
{

    [JsonPropertyName("cardPresent")]
    public VantivCardPresent? VantivCardPresent { get; set; }

    [JsonPropertyName("cardNotPresent")]
    public VatntivCardNotPresent? VantivCardNotPresent { get; set; }

}

public class SubscriptionFeatures
{

    [JsonPropertyName("cardPresent")]
    public CardPresent? CardPresent { get; set; }

    [JsonPropertyName("cardNotPresent")]
    public CardNotPresent? CardNotPresent { get; set; }

}

public class CardNotPresent
{
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("processors")]
    public Dictionary<string, CardNotPresentProcessor>? Processors { get; set; }

    [JsonPropertyName("visaStraightThroughProcessingOnly")]
    public bool? VisaStraightThroughProcessingOnly { get; set; }

    [JsonPropertyName("ignoreAddressVerificationSystem")]
    public bool? IgnoreAddressVerificationSystem { get; set; }

}

public class CardPresent
{
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("processors")]
    public Dictionary<string, CardPresentProcessor>? Processors { get; set; }

}

public class ConfigurationInformation
{
    [JsonPropertyName("configurations")]
    public Configurations? Configurations { get; set; }

    [JsonPropertyName("templateId")]
    public string? TemplateId { get; set; }

}

public class PaConfigurationInformation
{
    [JsonPropertyName("configurations")]
    public PaConfigurations? PaConfigurations { get; set; }

    [JsonPropertyName("templateId")]
    public string? TemplateId { get; set; }

}

public class Configurations
{
    [JsonPropertyName("common")]
    public Common? Common { get; set; }

    [JsonPropertyName("partnerSolutionIdentifier")]
    public string? PartnerSolutionIdentifier { get; set; }

    [JsonPropertyName("features")]
    public Features? Features { get; set; }

    [JsonPropertyName("cardTypes")]
    public CardTypes? CardTypes { get; set; }

}

public class PaConfigurations
{
    [JsonPropertyName("common")]
    public Common? Common { get; set; }

    [JsonPropertyName("partnerSolutionIdentifier")]
    public string? PartnerSolutionIdentifier { get; set; }

    [JsonPropertyName("features")]
    public Features? Features { get; set; }

    [JsonPropertyName("cardTypes")]
    public PaCardTypes? PaCardTypes { get; set; }

}

public class Common
{
    [JsonPropertyName("processors")]
    public Processors? Processors { get; set; }

    [JsonPropertyName("merchantCategoryCode")]
    public string? MerchantCategoryCode { get; set; }

    [JsonPropertyName("defaultAuthTypeCode")]
    public string? DefaultAuthTypeCode { get; set; }

    [JsonPropertyName("masterCardAssignedId")]
    public string? MasterCardAssignedId { get; set; }

    [JsonPropertyName("sicCode")]
    public string? SicCode { get; set; }

    [JsonPropertyName("enablePartialAuth")]
    public bool? EnablePartialAuth { get; set; }

    [JsonPropertyName("enableInterchangeOptimization")]
    public bool? EnableInterchangeOptimization { get; set; }

    [JsonPropertyName("enableSplitShipment")]
    public bool? EnableSplitShipment { get; set; }

    [JsonPropertyName("visaDelegatedAuthenticationId")]
    public string? VisaDelegatedAuthenticationId { get; set; }

    [JsonPropertyName("domesticMerchantId")]
    public string? DomesticMerchantId { get; set; }

    [JsonPropertyName("creditCardRefundLimitPercent")]
    public double? CreditCardRefundLimitPercent { get; set; }

    [JsonPropertyName("businessCenterCreditCardRefundLimitPercent")]
    public double? BusinessCenterCreditCardRefundLimitPercent { get; set; }

    [JsonPropertyName("allowCapturesGreaterThanAuthorizations")]
    public bool? AllowCapturesGreaterThanAuthorizations { get; set; }

    [JsonPropertyName("enableDuplicateMerchantReferenceNumberBlocking")]
    public bool? EnableDuplicateMerchantReferenceNumberBlocking { get; set; }

    [JsonPropertyName("merchantDescriptorInformation")]
    public MerchantDescriptorInformation? MerchantDescriptorInformation { get; set; }

    [JsonPropertyName("governmentControlled")]
    public bool? GovernmentControlled { get; set; }

    [JsonPropertyName("dropBillingInfo")]
    public bool? DropBillingInfo { get; set; }

    [JsonPropertyName("authMerchantRetryDisabled")]
    public bool? AuthMerchantRetryDisabled { get; set; }

    [JsonPropertyName("cardProcessingType")]
    public string? CardProcessingType { get; set; }

}


public class MerchantDescriptorInformation
{
    [JsonPropertyName("countryOfOrigin")]
    public string? CountryOfOrigin { get; set; }
}

public class Processors
{
    [JsonPropertyName("VPC")]
    public VPC? VPC { get; set; }

    [JsonPropertyName("vdcvantiv")]
    public VdcVantiv? VdcVantiv { get; set; }

    [JsonPropertyName("fdiglobal")]
    public FdiGlobalProcessor? FdiGlobal { get; set; }

}

public class VPC
{
    [JsonPropertyName("batchGroup")]
    public string? BatchGroup { get; set; }

    [JsonPropertyName("acquirer")]
    public Acquirer? Acquirer { get; set; }

    [JsonPropertyName("merchantId")]
    public string? MerchantId { get; set; }

    [JsonPropertyName("terminalId")]
    public string? TerminalId { get; set; }

    [JsonPropertyName("paymentTypes")]
    public PaymentTypes? PaymentTypes { get; set; }

    //[JsonPropertyName("currencies")]
    //public Currencies? Currencies { get; set; }

    [JsonPropertyName("enableCreditAuth")]
    public bool? EnableCreditAuth { get; set; }
}

public class Acquirer
{
    [JsonPropertyName("institutionId")]
    public string? InstitutionId { get; set; }

    [JsonPropertyName("interbankCardAssociationId")]
    public string? InterbankCardAssociationId { get; set; }

    [JsonPropertyName("discoverInstitutionId")]
    public string? DiscoverInstitutionId { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("fileDestinationBin")]
    public string? FileDestinationBin { get; set; }

}

public class PaymentTypes
{
    [JsonPropertyName("VISA")]
    public CardType? VISA { get; set; }

    [JsonPropertyName("MASTERCARD")]
    public CardType? MASTERCARD { get; set; }

    [JsonPropertyName("AMERICAN_EXPRESS")]
    public CardType? AMERICAN_EXPRESS { get; set; }

    [JsonPropertyName("DISCOVER")]
    public CardType? DISCOVER { get; set; }
}

public class CardType
{
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("currencies")]
    public Dictionary<string, Currency>? Currencies { get; set; }

}




public class Currency
{
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("enabledCardPresent")]
    public bool? EnabledCardPresent { get; set; }

    [JsonPropertyName("enabledCardNotPresent")]
    public bool? EnabledCardNotPresent { get; set; }

    [JsonPropertyName("merchantId")]
    public string? MerchantId { get; set; }

    [JsonPropertyName("terminalId")]
    public string? TerminalId { get; set; }

    [JsonPropertyName("terminalIds")]
    public List<string?>? TerminalIds { get; set; }

    [JsonPropertyName("serviceEnablementNumber")]
    public string? ServiceEnablementNumber { get; set; }

    [JsonPropertyName("currencyCodes")]
    public List<string>? CurrencyCodes { get; set; }

    [JsonPropertyName("acquirerId")]
    public string? AcquirerId { get; set; }

    [JsonPropertyName("processorMerchantId")]
    public string? ProcessorMerchantId { get; set; }


}

public class CardPresentConnect
{
    [JsonPropertyName("subscriptionInformation")]
    public CardPresentSubscriptionInformation? SubscriptionInformation { get; set; }

    [JsonPropertyName("configurationInformation")]
    public CardPresentConfigurationInformation? ConfigurationInformation { get; set; }
}

public class CardPresentSubscriptionInformation
{
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("selfServiceability")]
    public string? SelfServiceability { get; set; }


}

public class CardPresentConfigurationInformation
{
    [JsonPropertyName("templateId")]
    public string? TemplateId { get; set; }

    [JsonPropertyName("configurations")]
    public Configurations? Configurations { get; set; }
}


public class MerchantTransactionRoot
{
    [JsonPropertyName("processors")]
    public Dictionary<string, Processor>? Processors { get; set; }
}

public class Processor
{
    [JsonPropertyName("acquirer")]
    public Acquirer? Acquirer { get; set; }

    [JsonPropertyName("paymentTypes")]
    public Dictionary<string, PaymentType>? PaymentTypes { get; set; }

    [JsonPropertyName("acquirerMerchantId")]
    public string? AcquirerMerchantId { get; set; }

    [JsonPropertyName("allowMultipleBills")]
    public bool? AllowMultipleBills { get; set; }

    [JsonPropertyName("amexAggregatorId")]
    public string? AmexAggregatorId { get; set; }

    [JsonPropertyName("batchGroup")]
    public string? BatchGroup { get; set; }

    [JsonPropertyName("businessApplicationId")]
    public string? BusinessApplicationId { get; set; }

    [JsonPropertyName("creditAuthUnsupportedCardTypes")]
    public string? CreditAuthUnsupportedCardTypes { get; set; }

    [JsonPropertyName("enableAutoAuthReversalAfterVoid")]
    public bool? EnableAutoAuthReversalAfterVoid { get; set; }

    [JsonPropertyName("enableExpresspayPanTranslation")]
    public bool? EnableExpresspayPanTranslation { get; set; }

    [JsonPropertyName("masterCardAggregatorId")]
    public string? MasterCardAggregatorId { get; set; }

    [JsonPropertyName("merchantVerificationValue")]
    public string? MerchantVerificationValue { get; set; }

    [JsonPropertyName("quasiCash")]
    public bool? QuasiCash { get; set; }

    [JsonPropertyName("enableTransactionReferenceNumber")]
    public bool? EnableTransactionReferenceNumber { get; set; }

    [JsonPropertyName("visaAggregatorId")]
    public string? VisaAggregatorId { get; set; }

    [JsonPropertyName("enableDynamicCurrencyConversion")]
    public bool? EnableDynamicCurrencyConversion { get; set; }
}

public class PaymentType
{
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("currencies")]
    public Dictionary<string, CurrencyDetail>? Currencies { get; set; }
}

public class PaCardType
{
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("currencies")]
    public List<Currencies>? Currencies { get; set; }

}

public class Currencies
{
    [JsonPropertyName("currencyCodes")]
    public List<string>? CurrencyCodes { get; set; }

    [JsonPropertyName("acquirerId")]
    public string? AcquirerId { get; set; }

    [JsonPropertyName("processorMerchantId")]
    public string? ProcessorMerchantId { get; set; }
}

public class CurrencyDetail
{
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("terminalId")]
    public string? TerminalId { get; set; }

    [JsonPropertyName("enabledCardPresent")]
    public bool? EnabledCardPresent { get; set; }

    [JsonPropertyName("enabledCardNotPresent")]
    public bool? EnabledCardNotPresent { get; set; }

    [JsonPropertyName("merchantId")]
    public string? MerchantId { get; set; }
}


public class CardPresentProcessor
{
    [JsonPropertyName("disablePointOfSaleTerminalIdValidation")]
    public bool? DisablePointOfSaleTerminalIdValidation { get; set; }

    [JsonPropertyName("enablePinTranslation")]
    public bool? EnablePinTranslation { get; set; }

    [JsonPropertyName("defaultPointOfSaleTerminalId")]
    public string? DefaultPointOfSaleTerminalId { get; set; }

    [JsonPropertyName("pointOfSaleTerminalIds")]
    public string? PointOfSaleTerminalIds { get; set; }

    [JsonPropertyName("vdcvantiv")]
    public VdcvantivCardPresent? Vdcvantiv { get; set; }

}


public class CardNotPresentProcessor
{
    [JsonPropertyName("enableEmsTransactionRiskScore")]
    public bool? EnableEmsTransactionRiskScore { get; set; }

    [JsonPropertyName("relaxAddressVerificationSystem")]
    public bool? RelaxAddressVerificationSystem { get; set; }

    [JsonPropertyName("relaxAddressVerificationSystemAllowExpiredCard")]
    public bool? RelaxAddressVerificationSystemAllowExpiredCard { get; set; }

    [JsonPropertyName("relaxAddressVerificationSystemAllowZipWithoutCountry")]
    public bool? RelaxAddressVerificationSystemAllowZipWithoutCountry { get; set; }

    [JsonPropertyName("vdcvantiv")]
    public VdcvantivCardNotPresent? Vdcvantiv { get; set; }
}

public class ValueAddedServices
{
    [JsonPropertyName("transactionSearch")]
    public TransactionSearch? TransactionSearch { get; set; }

    [JsonPropertyName("reporting")]
    public Reporting? Reporting { get; set; }
}

public class TransactionSearch
{
    [JsonPropertyName("subscriptionInformation")]
    public SubscriptionInformation? SubscriptionInformation { get; set; }
}

public class Reporting
{
    [JsonPropertyName("subscriptionInformation")]
    public SubscriptionInformation? SubscriptionInformation { get; set; }
}

public class VdcVantiv
{
    [JsonPropertyName("acquirer")]
    public Acquirer? Acquirer { get; set; }

    [JsonPropertyName("paymentTypes")]
    public PaymentTypes? PaymentTypes { get; set; }

    [JsonPropertyName("acquirerMerchantId")]
    public string? AcquirerMerchantId { get; set; }

    [JsonPropertyName("allowMultipleBills")]
    public bool? AllowMultipleBills { get; set; }

    [JsonPropertyName("amexAggregatorId")]
    public string? AmexAggregatorId { get; set; }

    [JsonPropertyName("batchGroup")]
    public string? BatchGroup { get; set; }

    [JsonPropertyName("businessApplicationId")]
    public string? BusinessApplicationId { get; set; }

    [JsonPropertyName("creditAuthUnsupportedCardTypes")]
    public string? CreditAuthUnsupportedCardTypes { get; set; }

    [JsonPropertyName("enableAutoAuthReversalAfterVoid")]
    public bool? EnableAutoAuthReversalAfterVoid { get; set; }

    [JsonPropertyName("enableExpresspayPanTranslation")]
    public bool? EnableExpresspayPanTranslation { get; set; }

    [JsonPropertyName("masterCardAggregatorId")]
    public string? MasterCardAggregatorId { get; set; }

    [JsonPropertyName("merchantVerificationValue")]
    public string? MerchantVerificationValue { get; set; }

    [JsonPropertyName("quasiCash")]
    public bool? QuasiCash { get; set; }

    [JsonPropertyName("enableTransactionReferenceNumber")]
    public bool? EnableTransactionReferenceNumber { get; set; }

    [JsonPropertyName("visaAggregatorId")]
    public string? VisaAggregatorId { get; set; }

    [JsonPropertyName("enableDynamicCurrencyConversion")]
    public bool? EnableDynamicCurrencyConversion { get; set; }
}

public class ProcessorsCardPresent
{
    [JsonPropertyName("vdcvantiv")]
    public VdcvantivCardPresent? Vdcvantiv { get; set; }

    [JsonPropertyName("fdiglobal")]
    public CardPresentFdiGlobal? FdiGlobal { get; set; }
}

public class ProcessorsCardNotPresent
{
    [JsonPropertyName("vdcvantiv")]
    public VdcvantivCardNotPresent? Vdcvantiv { get; set; }

    [JsonPropertyName("fdiglobal")]
    public CardNotPresentFdiGlobal? FdiGlobal { get; set; }
}

public class VdcvantivCardPresent
{
    [JsonPropertyName("disablePointOfSaleTerminalIdValidation")]
    public bool DisablePointOfSaleTerminalIdValidation { get; set; }

    [JsonPropertyName("enablePinTranslation")]
    public bool EnablePinTranslation { get; set; }

    [JsonPropertyName("defaultPointOfSaleTerminalId")]
    public string? DefaultPointOfSaleTerminalId { get; set; }

    [JsonPropertyName("pointOfSaleTerminalIds")]
    public string? PointOfSaleTerminalIds { get; set; }
}

public class VdcvantivCardNotPresent
{
    [JsonPropertyName("enableEmsTransactionRiskScore")]
    public bool? EnableEmsTransactionRiskScore { get; set; }

    [JsonPropertyName("relaxAddressVerificationSystem")]
    public bool? RelaxAddressVerificationSystem { get; set; }

    [JsonPropertyName("relaxAddressVerificationSystemAllowExpiredCard")]
    public bool? RelaxAddressVerificationSystemAllowExpiredCard { get; set; }

    [JsonPropertyName("relaxAddressVerificationSystemAllowZipWithoutCountry")]
    public bool? RelaxAddressVerificationSystemAllowZipWithoutCountry { get; set; }
}

public class VantivCardPresent
{
    [JsonPropertyName("processors")]
    public ProcessorsCardPresent? Processors { get; set; }

    [JsonPropertyName("cardPresentSolutionType")]
    public string? CardPresentSolutionType { get; set; }

}

public class VatntivCardNotPresent
{
    [JsonPropertyName("processors")]
    public ProcessorsCardNotPresent? Processors { get; set; }

    [JsonPropertyName("visaStraightThroughProcessingOnly")]
    public bool VisaStraightThroughProcessingOnly { get; set; }

    [JsonPropertyName("ignoreAddressVerificationSystem")]
    public bool IgnoreAddressVerificationSystem { get; set; }

}


public class PayerAuthentication
{
    [JsonPropertyName("subscriptionInformation")]
    public SubscriptionInformation? SubscriptionInformation { get; set; }

    [JsonPropertyName("configurationInformation")]
    public PaConfigurationInformation? PaConfigurationInformation { get; set; }
}

public class PaCardTypes
{
    [JsonPropertyName("VerifiedByVisa")]
    public PaCardType? VerifiedByVisa { get; set; }

    [JsonPropertyName("AmexSafeKey")]
    public PaCardType? AmexSafeKey { get; set; }

    [JsonPropertyName("MasterCardSecureCode")]
    public PaCardType? MasterCardSecureCode { get; set; }

    [JsonPropertyName("DinersClubInternationalProtectBuy")]
    public PaCardType? DinersClubInternationalProtectBuy { get; set; }

    [JsonPropertyName("currencies")]
    public Currencies? Currencies { get; set; }
}

public class CardTypes
{
    [JsonPropertyName("currencies")]
    public Dictionary<string, Currency>? Currencies { get; set; }
}

// ── MLE Tokenize (/tms/v2/tokens) ────────────────────────────────────────

public class TokenizeApiRequest
{
    [JsonPropertyName("tokenInformation")]
    public TokenizeApiRequestTokenInfo? TokenInformation { get; set; }
}

public class TokenizeApiRequestTokenInfo
{
    [JsonPropertyName("card")]
    public FullCard? Card { get; set; }
}

public class TokenizeApiResponse
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("tokenInformation")]
    public TokenizeApiResponseTokenInfo? TokenInformation { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public class TokenizeApiResponseTokenInfo
{
    [JsonPropertyName("paymentInstrument")]
    public TokenRef? PaymentInstrument { get; set; }

    [JsonPropertyName("instrumentIdentifier")]
    public TokenRef? InstrumentIdentifier { get; set; }

    [JsonPropertyName("customer")]
    public TokenRef? Customer { get; set; }
}

public class TokenRef
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

// ── MLE Unified Tokenize (/tms/v2/tokenize) ──────────────────────────────────

public class TokenizeV2Request
{
    [JsonPropertyName("processingInformation")]
    public TokenizeV2ProcessingInfo? ProcessingInformation { get; set; }

    [JsonPropertyName("tokenInformation")]
    public TokenizeV2TokenInfo? TokenInformation { get; set; }
}

public class TokenizeV2ProcessingInfo
{
    [JsonPropertyName("actionList")]
    public string[]? ActionList { get; set; }

    [JsonPropertyName("actionTokenTypes")]
    public string[]? ActionTokenTypes { get; set; }
}

public class TokenizeV2TokenInfo
{
    [JsonPropertyName("customer")]
    public TokenizeV2CustomerInfo? Customer { get; set; }

    [JsonPropertyName("shippingAddress")]
    public TokenizeV2ShippingAddress? ShippingAddress { get; set; }

    [JsonPropertyName("paymentInstrument")]
    public TokenizeV2PaymentInstrument? PaymentInstrument { get; set; }

    [JsonPropertyName("instrumentIdentifier")]
    public TokenizeV2InstrumentIdentifier? InstrumentIdentifier { get; set; }
}

public class TokenizeV2CustomerInfo
{
    [JsonPropertyName("buyerInformation")]
    public TokenizeV2BuyerInfo? BuyerInformation { get; set; }

    [JsonPropertyName("clientReferenceInformation")]
    public TokenizeV2ClientRef? ClientReferenceInformation { get; set; }

    [JsonPropertyName("merchantDefinedInformation")]
    public List<TokenizeV2MerchantDefinedInfo>? MerchantDefinedInformation { get; set; }
}

public class TokenizeV2BuyerInfo
{
    [JsonPropertyName("merchantCustomerID")]
    public string? MerchantCustomerID { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }
}

public class TokenizeV2ClientRef
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }
}

public class TokenizeV2MerchantDefinedInfo
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}

public class TokenizeV2ShippingAddress
{
    [JsonPropertyName("default")]
    public string? Default { get; set; }

    [JsonPropertyName("shipTo")]
    public TokenizeV2ShipTo? ShipTo { get; set; }
}

public class TokenizeV2ShipTo
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("company")]
    public string? Company { get; set; }

    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    [JsonPropertyName("locality")]
    public string? Locality { get; set; }

    [JsonPropertyName("administrativeArea")]
    public string? AdministrativeArea { get; set; }

    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }
}

public class TokenizeV2PaymentInstrument
{
    [JsonPropertyName("default")]
    public string? Default { get; set; }

    [JsonPropertyName("card")]
    public TokenizeV2Card? Card { get; set; }

    [JsonPropertyName("billTo")]
    public TokenizeV2BillTo? BillTo { get; set; }
}

public class TokenizeV2Card
{
    [JsonPropertyName("expirationMonth")]
    public string? ExpirationMonth { get; set; }

    [JsonPropertyName("expirationYear")]
    public string? ExpirationYear { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public class TokenizeV2BillTo
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("company")]
    public string? Company { get; set; }

    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    [JsonPropertyName("locality")]
    public string? Locality { get; set; }

    [JsonPropertyName("administrativeArea")]
    public string? AdministrativeArea { get; set; }

    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }
}

public class TokenizeV2InstrumentIdentifier
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("card")]
    public TokenizeV2IdentifierCard? Card { get; set; }
}

public class TokenizeV2IdentifierCard
{
    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("expirationMonth")]
    public string? ExpirationMonth { get; set; }

    [JsonPropertyName("expirationYear")]
    public string? ExpirationYear { get; set; }
}

