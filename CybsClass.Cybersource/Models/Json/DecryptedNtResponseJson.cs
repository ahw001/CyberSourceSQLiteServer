using System;
using System.Text.Json.Serialization;

namespace CybsClass.Blazor.Model.Cybersource.NT
{

    public class DecryptedNtResponseJson
    {
        [JsonPropertyName("PaymentCardId")]
        public string? PaymentCardId { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("tokenReferenceId")]
        public string? TokenReferenceId { get; set; }

        [JsonPropertyName("number")]
        public string? Number { get; set; }

        [JsonPropertyName("expirationMonth")]
        public string? ExpirationMonth { get; set; }

        [JsonPropertyName("expirationYear")]
        public string? ExpirationYear { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("securityCode")]
        public string? SecurityCode { get; set; }

        [JsonPropertyName("requestorId")]
        public string? RequestorId { get; set; }

        [JsonPropertyName("card")]
        public CardDetails? Card { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }

    public class CardDetails
    {
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

    public class ClientReferenceInformation
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }

    public class ConsumerAuthenticationInformation
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }
    }

    public class IssuerInformation
    {
        [JsonPropertyName("responseRaw")]
        public string? ResponseRaw { get; set; }
    }

    public class OrderInformation
    {
        [JsonPropertyName("amountDetails")]
        public AmountDetails? AmountDetails { get; set; }
    }

    public class AmountDetails
    {
        [JsonPropertyName("authorizedAmount")]
        public string? AuthorizedAmount { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }
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
        public Card? Card { get; set; }
    }

    public class Card
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
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
        public bool? InstrumentidentifierNew { get; set; }

        [JsonPropertyName("instrumentIdentifier")]
        public InstrumentIdentifier? InstrumentIdentifier { get; set; }

        [JsonPropertyName("paymentInstrument")]
        public PaymentInstrument? PaymentInstrument { get; set; }
    }

    public class InstrumentIdentifier
    {
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

}
