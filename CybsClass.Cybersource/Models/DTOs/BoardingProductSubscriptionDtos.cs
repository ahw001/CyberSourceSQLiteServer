using System.Collections.Generic;
using System.Text.Json.Serialization;
using CybsClass.Cybersource.Models.BaseData;

namespace CybsClass.Cybersource.Models.DTOs;

// ── Product type discriminator ──────────────────────────────────────────────
public static class BoardingProductTypes
{
    public const string DigitalPayments    = "digitalPayments";
    public const string CustomerInvoicing  = "customerInvoicing";
    public const string PayByLink          = "payByLink";
    public const string TokenManagement    = "tokenManagement";
    public const string UnifiedCheckout    = "unifiedCheckout";
    public const string ValueAddedServices = "valueAddedServices";
    public const string VirtualTerminal    = "virtualTerminal";
    public const string PayerAuthentication = "payerAuthentication";

    public static readonly string[] All =
    {
        DigitalPayments, CustomerInvoicing, PayByLink, TokenManagement,
        UnifiedCheckout, ValueAddedServices, VirtualTerminal, PayerAuthentication
    };

    public static bool IsValid(string? productType) => productType is not null && System.Array.IndexOf(All, productType) >= 0;
}

// ── Junction ────────────────────────────────────────────────────────────────
public class BoardingTransactingMerchantProductSubscriptionDto
{
    public int BoardingTransactingMerchantProductSubscriptionId { get; set; }
    public int BoardingTransactingMerchantId { get; set; }
    public string? ProductType { get; set; }
    public int ProductSubscriptionId { get; set; }
    public bool IncludeInBoarding { get; set; } = true;

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

// ── Simple "subscription-only" shape used by invoicing / payByLink ──────────
public abstract class BoardingBasicSubscriptionDto
{
    public bool? Enabled { get; set; }
    public string? EnablementStatus { get; set; }
    public string? SelfServiceability { get; set; }
    public string? Distributability { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

// ── Digital Payments ────────────────────────────────────────────────────────
public class BoardingDigitalPaymentsSubscriptionDto : BoardingBasicSubscriptionDto
{
    public int BoardingDigitalPaymentsSubscriptionId { get; set; }
    public bool? SamsungPayEnabled { get; set; }
    public bool? ApplePayEnabled { get; set; }
}

// ── Invoicing ───────────────────────────────────────────────────────────────
public class BoardingInvoicingSubscriptionDto : BoardingBasicSubscriptionDto
{
    public int BoardingInvoicingSubscriptionId { get; set; }
}

// ── Pay By Link ─────────────────────────────────────────────────────────────
public class BoardingPayByLinkSubscriptionDto : BoardingBasicSubscriptionDto
{
    public int BoardingPayByLinkSubscriptionId { get; set; }
}

// ── Token Management ────────────────────────────────────────────────────────
public class BoardingTokenManagementSubscriptionDto : BoardingBasicSubscriptionDto
{
    public int BoardingTokenManagementSubscriptionId { get; set; }

    public string? CustomerTokenFormat { get; set; }
    public string? InstrumentIdentifierCardTokenFormat { get; set; }
    public string? PaymentInstrumentTokenFormat { get; set; }
    public string? CardNumberMaskingFormat { get; set; }

    public bool? VisaTokenServiceEnabled { get; set; }
    public bool? VisaTokenTransactionEnabled { get; set; }
    public bool? MastercardTokenServiceEnabled { get; set; }
    public bool? MastercardTokenTransactionEnabled { get; set; }
    public bool? CreateInstrumentIdentifierOnSuccess { get; set; }
    public bool? CreateInstrumentIdentifierOnFailure { get; set; }

    public string? ConfigurationStatus { get; set; }
    public string? ConfigurationMessage { get; set; }
}

// ── Unified Checkout ────────────────────────────────────────────────────────
public class BoardingUnifiedCheckoutSubscriptionDto : BoardingBasicSubscriptionDto
{
    public int BoardingUnifiedCheckoutSubscriptionId { get; set; }

    public string? ConfigurationStatus { get; set; }
    public string? ConfigurationMessage { get; set; }

    public bool? ApplePayEnabled { get; set; }
    public bool? ClickToPayEnabled { get; set; }
    public bool? ECheckEnabled { get; set; }
    public bool? GooglePayEnabled { get; set; }
    public bool? DecisionManagerEnabled { get; set; }
    public bool? PayerAuthenticationEnabled { get; set; }
    public bool? TokenManagementEnabled { get; set; }

    public List<string> AllowedCardNetworks { get; set; } = new();
}

// ── Value Added Services ────────────────────────────────────────────────────
public class BoardingValueAddedServicesSubscriptionDto
{
    public int BoardingValueAddedServicesSubscriptionId { get; set; }

    public bool? TransactionSearchEnabled { get; set; }
    public string? TransactionSearchEnablementStatus { get; set; }
    public string? TransactionSearchSelfServiceability { get; set; }
    public string? TransactionSearchDistributability { get; set; }

    public bool? ReportingEnabled { get; set; }
    public string? ReportingEnablementStatus { get; set; }
    public string? ReportingSelfServiceability { get; set; }
    public string? ReportingDistributability { get; set; }

    public bool? DisputeManagementEnabled { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

// ── Virtual Terminal ────────────────────────────────────────────────────────
public class BoardingVirtualTerminalSubscriptionDto : BoardingBasicSubscriptionDto
{
    public int BoardingVirtualTerminalSubscriptionId { get; set; }

    public string? ConfigurationStatus { get; set; }

    // common
    public bool? AllowECheckFields { get; set; }
    public bool? AllowLevel3Fields { get; set; }
    public bool? AllowServiceFeeFields { get; set; }
    public bool? ProductProfileEnabled { get; set; }
    public string? MerchantCountry { get; set; }
    public bool? AccountLevelEnabled { get; set; }
    public string? TokenProvider { get; set; }
    public bool? SecureStorageEnabled { get; set; }
    public string? OtsTokenClass { get; set; }
    public string? OtsProfileId { get; set; }
    public string? CardProcessingType { get; set; }
    public string? DefaultTransactionMethod { get; set; }

    // The five child tables are forwarded as raw JSON strings so the page can round-trip
    // the full cardNotPresent configuration without expanding every column on-screen.
    // DB service parses these and persists to the corresponding child tables.
    public string? GlobalPaymentInfoJson { get; set; }
    public string? ReceiptInfoJson { get; set; }
    public string? ReaderInfoJson { get; set; }
    public List<BoardingVirtualTerminalAcceptedCardTypeDto> AcceptedCardTypes { get; set; } = new();
    public List<BoardingVirtualTerminalMerchantDefinedFieldDto> MerchantDefinedFields { get; set; } = new();
}

public class BoardingVirtualTerminalAcceptedCardTypeDto
{
    public int BoardingVirtualTerminalAcceptedCardTypeId { get; set; }
    public string? ListType { get; set; }   // ACCEPTED | CVV_DISPLAY | CVV_REQUIRE
    public string? CardType { get; set; }
}

public class BoardingVirtualTerminalMerchantDefinedFieldDto
{
    public int BoardingVirtualTerminalMerchantDefinedFieldId { get; set; }
    public byte FieldIndex { get; set; }
    public bool? DisplayField { get; set; }
    public bool? RequiredField { get; set; }
    public bool? ShowReceipt { get; set; }
    public bool? ReceiptDisplayEnabled { get; set; }
}

// ── Payer Authentication ────────────────────────────────────────────────────
public class BoardingPayerAuthenticationSubscriptionDto : BoardingBasicSubscriptionDto
{
    public int BoardingPayerAuthenticationSubscriptionId { get; set; }
    public string? TemplateId { get; set; }
    public List<BoardingPayerAuthenticationCardTypeDto> CardTypeConfigs { get; set; } = new();
}

public class BoardingPayerAuthenticationCardTypeDto
{
    public int BoardingPayerAuthenticationCardTypeConfigId { get; set; }
    public string? CardTypeName { get; set; }
    public List<BoardingPayerAuthenticationCurrencyDto> Currencies { get; set; } = new();
}

public class BoardingPayerAuthenticationCurrencyDto
{
    public int BoardingPayerAuthenticationCurrencyId { get; set; }
    public string? CurrencyCodes { get; set; }
    public string? AcquirerId { get; set; }
    public string? ProcessorMerchantId { get; set; }
}
