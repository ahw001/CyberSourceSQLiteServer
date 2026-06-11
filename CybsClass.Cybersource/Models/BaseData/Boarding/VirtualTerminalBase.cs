using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable

namespace CybsClass.Cybersource.Models.BaseData.Boarding;

public sealed class VirtualTerminalBaseDto
{
    [JsonPropertyName("setups")]
    public VirtualTerminalSetups? Setups { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public sealed class VirtualTerminalSetups
{
    [JsonPropertyName("payments")]
    public VirtualTerminalPayments? Payments { get; set; }
}

public sealed class VirtualTerminalPayments
{
    [JsonPropertyName("virtualTerminal")]
    public VirtualTerminalProduct? VirtualTerminal { get; set; }
}

public sealed class VirtualTerminalProduct
{
    [JsonPropertyName("subscriptionInformation")]
    public BasicSubscriptionInformation? SubscriptionInformation { get; set; }

    [JsonPropertyName("configurationInformation")]
    public VirtualTerminalConfigurationInformation? ConfigurationInformation { get; set; }
}

public sealed class VirtualTerminalConfigurationInformation
{
    [JsonPropertyName("configurations")]
    public VirtualTerminalConfigurations? Configurations { get; set; }

    [JsonPropertyName("configurationStatus")]
    public ConfigurationStatus? ConfigurationStatus { get; set; }
}

public sealed class VirtualTerminalConfigurations
{
    [JsonPropertyName("common")]
    public VirtualTerminalCommon? Common { get; set; }

    [JsonPropertyName("cardNotPresent")]
    public VirtualTerminalCardNotPresent? CardNotPresent { get; set; }
}

public sealed class VirtualTerminalCommon
{
    [JsonPropertyName("allowECheckFields")]
    public bool? AllowECheckFields { get; set; }

    [JsonPropertyName("allowLevel3Fields")]
    public bool? AllowLevel3Fields { get; set; }

    [JsonPropertyName("allowServiceFeeFields")]
    public bool? AllowServiceFeeFields { get; set; }

    [JsonPropertyName("productProfileEnabled")]
    public bool? ProductProfileEnabled { get; set; }

    [JsonPropertyName("merchantCountry")]
    public string? MerchantCountry { get; set; }

    [JsonPropertyName("accountLevelEnabled")]
    public bool? AccountLevelEnabled { get; set; }

    [JsonPropertyName("tokenProvider")]
    public string? TokenProvider { get; set; }

    [JsonPropertyName("secureStorageEnabled")]
    public bool? SecureStorageEnabled { get; set; }

    [JsonPropertyName("otsTokenClass")]
    public string? OtsTokenClass { get; set; }

    [JsonPropertyName("otsProfileid")]
    public string? OtsProfileId { get; set; }

    [JsonPropertyName("cardProcessingType")]
    public string? CardProcessingType { get; set; }

    [JsonPropertyName("defaultTransactionMethod")]
    public string? DefaultTransactionMethod { get; set; }
}

public sealed class VirtualTerminalCardNotPresent
{
    [JsonPropertyName("globalPaymentInformation")]
    public VirtualTerminalGlobalPaymentInformation? GlobalPaymentInformation { get; set; }

    [JsonPropertyName("receiptInformation")]
    public VirtualTerminalReceiptInformation? ReceiptInformation { get; set; }

    [JsonPropertyName("readerInformation")]
    public VirtualTerminalReaderInformation? ReaderInformation { get; set; }

    [JsonPropertyName("_embedded")]
    public VirtualTerminalEmbedded? Embedded { get; set; }
}

public sealed class VirtualTerminalGlobalPaymentInformation
{
    [JsonPropertyName("basicInformation")]
    public VirtualTerminalBasicInformation? BasicInformation { get; set; }

    [JsonPropertyName("paymentInformation")]
    public VirtualTerminalPaymentInformation? PaymentInformation { get; set; }

    [JsonPropertyName("customerInformation")]
    public VirtualTerminalCustomerInformation? CustomerInformation { get; set; }

    [JsonPropertyName("orderInformation")]
    public VirtualTerminalOrderInformation? OrderInformation { get; set; }

    [JsonPropertyName("levelTwoFields")]
    public VirtualTerminalLevelTwoFields? LevelTwoFields { get; set; }

    [JsonPropertyName("levelThreeFields")]
    public VirtualTerminalLevelThreeFields? LevelThreeFields { get; set; }

    [JsonPropertyName("merchantDefinedDataFields")]
    public VirtualTerminalMerchantDefinedDataFields? MerchantDefinedDataFields { get; set; }

    [JsonPropertyName("virtualTerminalPage")]
    public VirtualTerminalPage? VirtualTerminalPage { get; set; }

    [JsonPropertyName("serviceFeeTermsAndConditions")]
    public VirtualTerminalServiceFeeTermsAndConditions? ServiceFeeTermsAndConditions { get; set; }

    [JsonPropertyName("differentialFee")]
    public VirtualTerminalDifferentialFee? DifferentialFee { get; set; }
}

public sealed class VirtualTerminalBasicInformation
{
    [JsonPropertyName("defaultCountryCode")]
    public string? DefaultCountryCode { get; set; }

    [JsonPropertyName("defaultCurrencyCode")]
    public string? DefaultCurrencyCode { get; set; }

    [JsonPropertyName("defaultPaymentType")]
    public string? DefaultPaymentType { get; set; }

    [JsonPropertyName("defaultTransactionSource")]
    public string? DefaultTransactionSource { get; set; }

    [JsonPropertyName("displayRetail")]
    public bool? DisplayRetail { get; set; }

    [JsonPropertyName("displayMoto")]
    public bool? DisplayMoto { get; set; }

    [JsonPropertyName("displayInternet")]
    public bool? DisplayInternet { get; set; }

    [JsonPropertyName("defaultStandardEntryClassCode")]
    public string? DefaultStandardEntryClassCode { get; set; }
}

public sealed class VirtualTerminalPaymentInformation
{
    [JsonPropertyName("acceptedCardTypes")]
    public List<string>? AcceptedCardTypes { get; set; }

    [JsonPropertyName("displayAuthIndicator")]
    public bool? DisplayAuthIndicator { get; set; }

    [JsonPropertyName("displayJapanPayOnly")]
    public bool? DisplayJapanPayOnly { get; set; }

    [JsonPropertyName("displayCreditCards")]
    public bool? DisplayCreditCards { get; set; }

    [JsonPropertyName("displayEchecks")]
    public bool? DisplayEchecks { get; set; }

    [JsonPropertyName("displayDebtIndicator")]
    public bool? DisplayDebtIndicator { get; set; }

    [JsonPropertyName("displayBillPayment")]
    public bool? DisplayBillPayment { get; set; }

    [JsonPropertyName("displayECheckEffectiveDate")]
    public bool? DisplayECheckEffectiveDate { get; set; }

    [JsonPropertyName("displayIgnoreECheckAvsCheckbox")]
    public bool? DisplayIgnoreECheckAvsCheckbox { get; set; }

    [JsonPropertyName("displayCardVerificationValue")]
    public List<string>? DisplayCardVerificationValue { get; set; }

    [JsonPropertyName("requireCardVerificationValue")]
    public List<string>? RequireCardVerificationValue { get; set; }

    [JsonPropertyName("displayECheckReferenceNumber")]
    public bool? DisplayECheckReferenceNumber { get; set; }

    [JsonPropertyName("requireECheckReferenceNumber")]
    public bool? RequireECheckReferenceNumber { get; set; }

    [JsonPropertyName("enableEchecks")]
    public bool? EnableEchecks { get; set; }
}

public sealed class VirtualTerminalCustomerInformation
{
    [JsonPropertyName("displayIgnoreAddressVerificationSystemCheckbox")]
    public bool? DisplayIgnoreAddressVerificationSystemCheckbox { get; set; }

    [JsonPropertyName("requirePhoneNumber")]
    public bool? RequirePhoneNumber { get; set; }

    [JsonPropertyName("requireEmailAddress")]
    public bool? RequireEmailAddress { get; set; }

    [JsonPropertyName("requireCustomerId")]
    public bool? RequireCustomerId { get; set; }

    [JsonPropertyName("requireBillingAddress")]
    public bool? RequireBillingAddress { get; set; }

    [JsonPropertyName("displayEmailAddress")]
    public bool? DisplayEmailAddress { get; set; }

    [JsonPropertyName("displayCompany")]
    public bool? DisplayCompany { get; set; }

    [JsonPropertyName("displayBillingAddress")]
    public bool? DisplayBillingAddress { get; set; }

    [JsonPropertyName("displayCustomerId")]
    public bool? DisplayCustomerId { get; set; }

    [JsonPropertyName("requireCustomerCompany")]
    public bool? RequireCustomerCompany { get; set; }

    [JsonPropertyName("displayCustomerIdForBilling")]
    public bool? DisplayCustomerIdForBilling { get; set; }

    [JsonPropertyName("requireCustomerIdForBilling")]
    public bool? RequireCustomerIdForBilling { get; set; }

    [JsonPropertyName("requireFirstNameForBilling")]
    public bool? RequireFirstNameForBilling { get; set; }

    [JsonPropertyName("displayFirstNameForBilling")]
    public bool? DisplayFirstNameForBilling { get; set; }

    [JsonPropertyName("requireLastNameForBilling")]
    public bool? RequireLastNameForBilling { get; set; }

    [JsonPropertyName("displayLastNameForBilling")]
    public bool? DisplayLastNameForBilling { get; set; }

    [JsonPropertyName("requireCompanyNameForBilling")]
    public bool? RequireCompanyNameForBilling { get; set; }

    [JsonPropertyName("displayCompanyNameForBilling")]
    public bool? DisplayCompanyNameForBilling { get; set; }

    [JsonPropertyName("requireStreetAddress1ForBilling")]
    public bool? RequireStreetAddress1ForBilling { get; set; }

    [JsonPropertyName("displayStreetAddress1ForBilling")]
    public bool? DisplayStreetAddress1ForBilling { get; set; }

    [JsonPropertyName("requireStreetAddress2ForBilling")]
    public bool? RequireStreetAddress2ForBilling { get; set; }

    [JsonPropertyName("displayStreetAddress2ForBilling")]
    public bool? DisplayStreetAddress2ForBilling { get; set; }

    [JsonPropertyName("requireCityForBilling")]
    public bool? RequireCityForBilling { get; set; }

    [JsonPropertyName("displayCityForBilling")]
    public bool? DisplayCityForBilling { get; set; }

    [JsonPropertyName("requireStateForBilling")]
    public bool? RequireStateForBilling { get; set; }

    [JsonPropertyName("displayStateForBilling")]
    public bool? DisplayStateForBilling { get; set; }

    [JsonPropertyName("requireZipPostalCodeForBilling")]
    public bool? RequireZipPostalCodeForBilling { get; set; }

    [JsonPropertyName("displayZipPostalCodeForBilling")]
    public bool? DisplayZipPostalCodeForBilling { get; set; }

    [JsonPropertyName("requireCountryForBilling")]
    public bool? RequireCountryForBilling { get; set; }

    [JsonPropertyName("displayCountryForBilling")]
    public bool? DisplayCountryForBilling { get; set; }

    [JsonPropertyName("requirePhoneNumberForBilling")]
    public bool? RequirePhoneNumberForBilling { get; set; }

    [JsonPropertyName("displayPhoneNumberForBilling")]
    public bool? DisplayPhoneNumberForBilling { get; set; }

    [JsonPropertyName("requireEmailAddressForBilling")]
    public bool? RequireEmailAddressForBilling { get; set; }

    [JsonPropertyName("displayEmailAddressForBilling")]
    public bool? DisplayEmailAddressForBilling { get; set; }

    [JsonPropertyName("requireFirstNameForShipping")]
    public bool? RequireFirstNameForShipping { get; set; }

    [JsonPropertyName("displayFirstNameForShipping")]
    public bool? DisplayFirstNameForShipping { get; set; }

    [JsonPropertyName("requireLastNameForShipping")]
    public bool? RequireLastNameForShipping { get; set; }

    [JsonPropertyName("displayLastNameForShipping")]
    public bool? DisplayLastNameForShipping { get; set; }

    [JsonPropertyName("requireCompanyNameForShipping")]
    public bool? RequireCompanyNameForShipping { get; set; }

    [JsonPropertyName("displayCompanyNameForShipping")]
    public bool? DisplayCompanyNameForShipping { get; set; }

    [JsonPropertyName("requireStreetAddress1ForShipping")]
    public bool? RequireStreetAddress1ForShipping { get; set; }

    [JsonPropertyName("displayStreetAddress1ForShipping")]
    public bool? DisplayStreetAddress1ForShipping { get; set; }

    [JsonPropertyName("requireStreetAddress2ForShipping")]
    public bool? RequireStreetAddress2ForShipping { get; set; }

    [JsonPropertyName("displayStreetAddress2ForShipping")]
    public bool? DisplayStreetAddress2ForShipping { get; set; }

    [JsonPropertyName("requireCityForShipping")]
    public bool? RequireCityForShipping { get; set; }

    [JsonPropertyName("displayCityForShipping")]
    public bool? DisplayCityForShipping { get; set; }

    [JsonPropertyName("requireStateForShipping")]
    public bool? RequireStateForShipping { get; set; }

    [JsonPropertyName("displayStateForShipping")]
    public bool? DisplayStateForShipping { get; set; }

    [JsonPropertyName("requirePostalCodeForShipping")]
    public bool? RequirePostalCodeForShipping { get; set; }

    [JsonPropertyName("displayPostalCodeForShipping")]
    public bool? DisplayPostalCodeForShipping { get; set; }

    [JsonPropertyName("requireCountryForShipping")]
    public bool? RequireCountryForShipping { get; set; }

    [JsonPropertyName("displayCountryForShipping")]
    public bool? DisplayCountryForShipping { get; set; }

    [JsonPropertyName("requirePhoneNumberForShipping")]
    public bool? RequirePhoneNumberForShipping { get; set; }

    [JsonPropertyName("displayPhoneNumberForShipping")]
    public bool? DisplayPhoneNumberForShipping { get; set; }

    [JsonPropertyName("displayPhoneNumber")]
    public bool? DisplayPhoneNumber { get; set; }

    [JsonPropertyName("firstNameRequired")]
    public bool? FirstNameRequired { get; set; }

    [JsonPropertyName("lastNameRequired")]
    public bool? LastNameRequired { get; set; }

    [JsonPropertyName("displayFirstName")]
    public bool? DisplayFirstName { get; set; }

    [JsonPropertyName("displayLastName")]
    public bool? DisplayLastName { get; set; }

    [JsonPropertyName("firstNameShowReceipt")]
    public bool? FirstNameShowReceipt { get; set; }

    [JsonPropertyName("lastNameShowReceipt")]
    public bool? LastNameShowReceipt { get; set; }
}

public sealed class VirtualTerminalOrderInformation
{
    [JsonPropertyName("displayShippingAddress")]
    public bool? DisplayShippingAddress { get; set; }

    [JsonPropertyName("requireShippingAddress")]
    public bool? RequireShippingAddress { get; set; }

    [JsonPropertyName("shippingAddressDisplayOnReceipt")]
    public bool? ShippingAddressDisplayOnReceipt { get; set; }

    [JsonPropertyName("merchantDescriptorContact")]
    public bool? MerchantDescriptorContact { get; set; }

    [JsonPropertyName("merchantDescriptorAdditionalInformation")]
    public bool? MerchantDescriptorAdditionalInformation { get; set; }

    [JsonPropertyName("displayOrderNumber")]
    public bool? DisplayOrderNumber { get; set; }

    [JsonPropertyName("requireTransactionReferenceNumber")]
    public bool? RequireTransactionReferenceNumber { get; set; }

    [JsonPropertyName("requireOrderNumber")]
    public bool? RequireOrderNumber { get; set; }

    [JsonPropertyName("displayMerchantDescriptor")]
    public bool? DisplayMerchantDescriptor { get; set; }

    [JsonPropertyName("displayTransactionReferenceNumber")]
    public bool? DisplayTransactionReferenceNumber { get; set; }

    [JsonPropertyName("displayComment")]
    public bool? DisplayComment { get; set; }

    [JsonPropertyName("requireComment")]
    public bool? RequireComment { get; set; }

    [JsonPropertyName("commentShowReceipt")]
    public bool? CommentShowReceipt { get; set; }

    [JsonPropertyName("orderNumberDisplayOnReceipt")]
    public bool? OrderNumberDisplayOnReceipt { get; set; }

    [JsonPropertyName("companyDisplayOnReceipt")]
    public bool? CompanyDisplayOnReceipt { get; set; }

    [JsonPropertyName("addressVerificationSystemResultsDisplayOnReceipt")]
    public bool? AddressVerificationSystemResultsDisplayOnReceipt { get; set; }

    [JsonPropertyName("authCodeDisplayOnReceipt")]
    public bool? AuthCodeDisplayOnReceipt { get; set; }

    [JsonPropertyName("enableEcheckServiceFee")]
    public bool? EnableEcheckServiceFee { get; set; }

    [JsonPropertyName("europayMasterCardVisaReaderUpdateCheck")]
    public bool? EuropayMasterCardVisaReaderUpdateCheck { get; set; }
}

public sealed class VirtualTerminalLevelTwoFields
{
    [JsonPropertyName("displayLevel2Duty")]
    public bool? DisplayLevel2Duty { get; set; }

    [JsonPropertyName("level2DutyRequired")]
    public bool? Level2DutyRequired { get; set; }

    [JsonPropertyName("displayLevel2PurchaseOrderNumber")]
    public bool? DisplayLevel2PurchaseOrderNumber { get; set; }

    [JsonPropertyName("requireLevel2PurchaseOrderNumber")]
    public bool? RequireLevel2PurchaseOrderNumber { get; set; }

    [JsonPropertyName("displayLevel2Tax")]
    public bool? DisplayLevel2Tax { get; set; }

    [JsonPropertyName("requireLevel2Tax")]
    public bool? RequireLevel2Tax { get; set; }

    [JsonPropertyName("displayLevel2TaxExempt")]
    public bool? DisplayLevel2TaxExempt { get; set; }

    [JsonPropertyName("requireLevel2TaxExempt")]
    public bool? RequireLevel2TaxExempt { get; set; }

    [JsonPropertyName("level2l3Enabled")]
    public bool? Level2L3Enabled { get; set; }

    [JsonPropertyName("level2DutyDisplayOnReceipt")]
    public bool? Level2DutyDisplayOnReceipt { get; set; }

    [JsonPropertyName("level2PurchaseOrderNumberDisplayOnReceipt")]
    public bool? Level2PurchaseOrderNumberDisplayOnReceipt { get; set; }

    [JsonPropertyName("level2TaxDisplayOnReceipt")]
    public bool? Level2TaxDisplayOnReceipt { get; set; }

    [JsonPropertyName("level2TaxExemptDisplayOnReceipt")]
    public bool? Level2TaxExemptDisplayOnReceipt { get; set; }

    [JsonPropertyName("level3Configuration")]
    public string? Level3Configuration { get; set; }
}

public sealed class VirtualTerminalLevelThreeFields
{
    [JsonPropertyName("itemInvoiceNumberQuantityCustom")]
    public string? ItemInvoiceNumberQuantityCustom { get; set; }

    [JsonPropertyName("virtualTerminalReceiptHeader")]
    public string? VirtualTerminalReceiptHeader { get; set; }

    [JsonPropertyName("level3AlternateTaxRequired")]
    public bool? Level3AlternateTaxRequired { get; set; }

    [JsonPropertyName("displayLevel3AlternateTax")]
    public bool? DisplayLevel3AlternateTax { get; set; }

    [JsonPropertyName("itemAlternateTaxAmountDisplayOnReceipt")]
    public bool? ItemAlternateTaxAmountDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemAlternateTaxIdDisplayOnReceipt")]
    public bool? ItemAlternateTaxIdDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemAlternateTaxRateDisplayOnReceipt")]
    public bool? ItemAlternateTaxRateDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemAlternateTaxTypeApplicationDisplayOnReceipt")]
    public bool? ItemAlternateTaxTypeApplicationDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemAlternateTaxTypeDisplayOnReceipt")]
    public bool? ItemAlternateTaxTypeDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemUnitPriceDisplayOnReceipt")]
    public bool? ItemUnitPriceDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemCommodityCodeDisplayOnReceipt")]
    public bool? ItemCommodityCodeDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemDiscountAmountDisplayOnReceipt")]
    public bool? ItemDiscountAmountDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemDiscountIndicatorDisplayOnReceipt")]
    public bool? ItemDiscountIndicatorDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemDiscountRateDisplayOnReceipt")]
    public bool? ItemDiscountRateDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemGrossNetIndicatorDisplayOnReceipt")]
    public bool? ItemGrossNetIndicatorDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemLocalTaxDisplayOnReceipt")]
    public bool? ItemLocalTaxDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemProductSKUDisplayOnReceipt")]
    public bool? ItemProductSKUDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemNationalTaxDisplayOnReceipt")]
    public bool? ItemNationalTaxDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemProductCodeDisplayOnReceipt")]
    public bool? ItemProductCodeDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemProductNameDisplayOnReceipt")]
    public bool? ItemProductNameDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemQuantityDisplayOnReceipt")]
    public bool? ItemQuantityDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemTaxAmountDisplayOnReceipt")]
    public bool? ItemTaxAmountDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemTaxRateDisplayOnReceipt")]
    public bool? ItemTaxRateDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemTaxTypeAppliedDisplayOnReceipt")]
    public bool? ItemTaxTypeAppliedDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemTotalAmountDisplayOnReceipt")]
    public bool? ItemTotalAmountDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemUnitOfMeasureDisplayOnReceipt")]
    public bool? ItemUnitOfMeasureDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemVatRateDisplayOnReceipt")]
    public bool? ItemVatRateDisplayOnReceipt { get; set; }

    [JsonPropertyName("itemInvoiceNumberDisplayOnReceipt")]
    public bool? ItemInvoiceNumberDisplayOnReceipt { get; set; }

    [JsonPropertyName("level3LineItemDisplayOnReceipt")]
    public bool? Level3LineItemDisplayOnReceipt { get; set; }

    [JsonPropertyName("copyConsumer")]
    public bool? CopyConsumer { get; set; }

    [JsonPropertyName("printSingleDisplayOnReceipt")]
    public bool? PrintSingleDisplayOnReceipt { get; set; }

    [JsonPropertyName("printDualDisplayOnReceipt")]
    public bool? PrintDualDisplayOnReceipt { get; set; }

    [JsonPropertyName("level3AlternateTaxDisplayOnReceipt")]
    public bool? Level3AlternateTaxDisplayOnReceipt { get; set; }

    [JsonPropertyName("alternateTaxAmountDisplayOnReceipt")]
    public bool? AlternateTaxAmountDisplayOnReceipt { get; set; }

    [JsonPropertyName("alternateTaxIndicatorDisplayOnReceipt")]
    public bool? AlternateTaxIndicatorDisplayOnReceipt { get; set; }

    [JsonPropertyName("alternateTaxIdDisplayOnReceipt")]
    public bool? AlternateTaxIdDisplayOnReceipt { get; set; }

    [JsonPropertyName("amexDataTAA1DisplayOnReceipt")]
    public bool? AmexDataTAA1DisplayOnReceipt { get; set; }

    [JsonPropertyName("amexDataTAA2DisplayOnReceipt")]
    public bool? AmexDataTAA2DisplayOnReceipt { get; set; }

    [JsonPropertyName("amexDataTAA3DisplayOnReceipt")]
    public bool? AmexDataTAA3DisplayOnReceipt { get; set; }

    [JsonPropertyName("amexDataTAA4DisplayOnReceipt")]
    public bool? AmexDataTAA4DisplayOnReceipt { get; set; }

    [JsonPropertyName("dutyAmountDisplayOnReceipt")]
    public bool? DutyAmountDisplayOnReceipt { get; set; }

    [JsonPropertyName("freightAmountDisplayOnReceipt")]
    public bool? FreightAmountDisplayOnReceipt { get; set; }

    [JsonPropertyName("userPurchaseOrderDisplayOnReceipt")]
    public bool? UserPurchaseOrderDisplayOnReceipt { get; set; }

    [JsonPropertyName("localTaxAmountDisplayOnReceipt")]
    public bool? LocalTaxAmountDisplayOnReceipt { get; set; }

    [JsonPropertyName("localTaxIndicatorDisplayOnReceipt")]
    public bool? LocalTaxIndicatorDisplayOnReceipt { get; set; }

    [JsonPropertyName("merchantVATRegistrationNumberDisplayOnReceipt")]
    public bool? MerchantVATRegistrationNumberDisplayOnReceipt { get; set; }

    [JsonPropertyName("nationalTaxAmountDisplayOnReceipt")]
    public bool? NationalTaxAmountDisplayOnReceipt { get; set; }

    [JsonPropertyName("nationalTaxIndicatorDisplayOnReceipt")]
    public bool? NationalTaxIndicatorDisplayOnReceipt { get; set; }

    [JsonPropertyName("discountAmountDisplayOnReceipt")]
    public bool? DiscountAmountDisplayOnReceipt { get; set; }

    [JsonPropertyName("purchaserCodeDisplayOnReceipt")]
    public bool? PurchaserCodeDisplayOnReceipt { get; set; }

    [JsonPropertyName("purchaserOrderDateDisplayOnReceipt")]
    public bool? PurchaserOrderDateDisplayOnReceipt { get; set; }

    [JsonPropertyName("purchaserVATRegistrationNumberDisplayOnReceipt")]
    public bool? PurchaserVATRegistrationNumberDisplayOnReceipt { get; set; }

    [JsonPropertyName("shipFrmpostalCodeDisplayOnReceipt")]
    public bool? ShipFrmPostalCodeDisplayOnReceipt { get; set; }

    [JsonPropertyName("smryCommodityCodeDisplayOnReceipt")]
    public bool? SmryCommodityCodeDisplayOnReceipt { get; set; }

    [JsonPropertyName("supplierOrderReferenceDisplayOnReceipt")]
    public bool? SupplierOrderReferenceDisplayOnReceipt { get; set; }

    [JsonPropertyName("taxableDisplayOnReceipt")]
    public bool? TaxableDisplayOnReceipt { get; set; }

    [JsonPropertyName("vatInvoiceReferenceNumberDisplayOnReceipt")]
    public bool? VatInvoiceReferenceNumberDisplayOnReceipt { get; set; }

    [JsonPropertyName("vatTaxAmountDisplayOnReceipt")]
    public bool? VatTaxAmountDisplayOnReceipt { get; set; }

    [JsonPropertyName("vatTaxRateDisplayOnReceipt")]
    public bool? VatTaxRateDisplayOnReceipt { get; set; }
}

public sealed class VirtualTerminalMerchantDefinedDataFields
{
    [JsonPropertyName("displayMerchantDefinedData1")]
    public bool? DisplayMerchantDefinedData1 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData2")]
    public bool? DisplayMerchantDefinedData2 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData3")]
    public bool? DisplayMerchantDefinedData3 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData4")]
    public bool? DisplayMerchantDefinedData4 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData5")]
    public bool? DisplayMerchantDefinedData5 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData6")]
    public bool? DisplayMerchantDefinedData6 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData7")]
    public bool? DisplayMerchantDefinedData7 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData8")]
    public bool? DisplayMerchantDefinedData8 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData9")]
    public bool? DisplayMerchantDefinedData9 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData10")]
    public bool? DisplayMerchantDefinedData10 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData11")]
    public bool? DisplayMerchantDefinedData11 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData12")]
    public bool? DisplayMerchantDefinedData12 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData13")]
    public bool? DisplayMerchantDefinedData13 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData14")]
    public bool? DisplayMerchantDefinedData14 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData15")]
    public bool? DisplayMerchantDefinedData15 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData16")]
    public bool? DisplayMerchantDefinedData16 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData17")]
    public bool? DisplayMerchantDefinedData17 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData18")]
    public bool? DisplayMerchantDefinedData18 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData19")]
    public bool? DisplayMerchantDefinedData19 { get; set; }
    [JsonPropertyName("displayMerchantDefinedData20")]
    public bool? DisplayMerchantDefinedData20 { get; set; }

    [JsonPropertyName("requiredMerchantDefinedData1")]
    public bool? RequiredMerchantDefinedData1 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData2")]
    public bool? RequiredMerchantDefinedData2 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData3")]
    public bool? RequiredMerchantDefinedData3 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData4")]
    public bool? RequiredMerchantDefinedData4 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData5")]
    public bool? RequiredMerchantDefinedData5 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData6")]
    public bool? RequiredMerchantDefinedData6 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData7")]
    public bool? RequiredMerchantDefinedData7 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData8")]
    public bool? RequiredMerchantDefinedData8 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData9")]
    public bool? RequiredMerchantDefinedData9 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData10")]
    public bool? RequiredMerchantDefinedData10 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData11")]
    public bool? RequiredMerchantDefinedData11 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData12")]
    public bool? RequiredMerchantDefinedData12 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData13")]
    public bool? RequiredMerchantDefinedData13 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData14")]
    public bool? RequiredMerchantDefinedData14 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData15")]
    public bool? RequiredMerchantDefinedData15 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData16")]
    public bool? RequiredMerchantDefinedData16 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData17")]
    public bool? RequiredMerchantDefinedData17 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData18")]
    public bool? RequiredMerchantDefinedData18 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData19")]
    public bool? RequiredMerchantDefinedData19 { get; set; }
    [JsonPropertyName("requiredMerchantDefinedData20")]
    public bool? RequiredMerchantDefinedData20 { get; set; }

    [JsonPropertyName("merchantDefinedData1ShowReceipt")]
    public bool? MerchantDefinedData1ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData2ShowReceipt")]
    public bool? MerchantDefinedData2ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData3ShowReceipt")]
    public bool? MerchantDefinedData3ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData4ShowReceipt")]
    public bool? MerchantDefinedData4ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData5ShowReceipt")]
    public bool? MerchantDefinedData5ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData6ShowReceipt")]
    public bool? MerchantDefinedData6ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData7ShowReceipt")]
    public bool? MerchantDefinedData7ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData8ShowReceipt")]
    public bool? MerchantDefinedData8ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData9ShowReceipt")]
    public bool? MerchantDefinedData9ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData10ShowReceipt")]
    public bool? MerchantDefinedData10ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData11ShowReceipt")]
    public bool? MerchantDefinedData11ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData12ShowReceipt")]
    public bool? MerchantDefinedData12ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData13ShowReceipt")]
    public bool? MerchantDefinedData13ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData14ShowReceipt")]
    public bool? MerchantDefinedData14ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData15ShowReceipt")]
    public bool? MerchantDefinedData15ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData16ShowReceipt")]
    public bool? MerchantDefinedData16ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData17ShowReceipt")]
    public bool? MerchantDefinedData17ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData18ShowReceipt")]
    public bool? MerchantDefinedData18ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData19ShowReceipt")]
    public bool? MerchantDefinedData19ShowReceipt { get; set; }
    [JsonPropertyName("merchantDefinedData20ShowReceipt")]
    public bool? MerchantDefinedData20ShowReceipt { get; set; }

    [JsonPropertyName("merchantDescriptorReceipt")]
    public bool? MerchantDescriptorReceipt { get; set; }

    [JsonPropertyName("merchantDescriptorContactReceipt")]
    public bool? MerchantDescriptorContactReceipt { get; set; }
}

public sealed class VirtualTerminalPage
{
    [JsonPropertyName("virtualTerminalLongFormEnabled")]
    public bool? VirtualTerminalLongFormEnabled { get; set; }
}

public sealed class VirtualTerminalServiceFeeTermsAndConditions
{
    [JsonPropertyName("serviceFeeEnabled")]
    public bool? ServiceFeeEnabled { get; set; }

    [JsonPropertyName("serviceFeeInSubscriptionOTPEnabled")]
    public bool? ServiceFeeInSubscriptionOTPEnabled { get; set; }

    [JsonPropertyName("serviceFeeInPaymentTokenOTPEnabled")]
    public bool? ServiceFeeInPaymentTokenOTPEnabled { get; set; }
}

public sealed class VirtualTerminalDifferentialFee
{
    [JsonPropertyName("displaySurchargeAmount")]
    public bool? DisplaySurchargeAmount { get; set; }

    [JsonPropertyName("displayCalculateSurchargeAmount")]
    public bool? DisplayCalculateSurchargeAmount { get; set; }
}

public sealed class VirtualTerminalReceiptInformation
{
    [JsonPropertyName("header")]
    public VirtualTerminalReceiptHeader? Header { get; set; }

    [JsonPropertyName("description")]
    public VirtualTerminalReceiptDescription? Description { get; set; }

    [JsonPropertyName("orderInformation")]
    public VirtualTerminalReceiptOrderInformation? OrderInformation { get; set; }

    [JsonPropertyName("customerInformation")]
    public VirtualTerminalReceiptCustomerInformation? CustomerInformation { get; set; }

    [JsonPropertyName("resultFields")]
    public VirtualTerminalReceiptResultFields? ResultFields { get; set; }

    [JsonPropertyName("emailReceipt")]
    public VirtualTerminalEmailReceipt? EmailReceipt { get; set; }

    [JsonPropertyName("levelTwoFields")]
    public VirtualTerminalReceiptLevelTwoFields? LevelTwoFields { get; set; }

    [JsonPropertyName("levelTwoAndLevelThreeFields")]
    public VirtualTerminalReceiptLevelTwoAndLevelThreeFields? LevelTwoAndLevelThreeFields { get; set; }

    [JsonPropertyName("merchantDefinedFields")]
    public VirtualTerminalReceiptMerchantDefinedFields? MerchantDefinedFields { get; set; }

    [JsonPropertyName("printableData")]
    public VirtualTerminalPrintableData? PrintableData { get; set; }

    [JsonPropertyName("europayMasterCardVisaFields")]
    public VirtualTerminalReceiptEmvFields? EuropayMasterCardVisaFields { get; set; }
}

public sealed class VirtualTerminalReceiptHeader
{
    [JsonPropertyName("virtualTerminalReceiptHeader")]
    public string? HeaderText { get; set; }
}

public sealed class VirtualTerminalReceiptDescription
{
    [JsonPropertyName("displayTransactionDescription")]
    public bool? DisplayTransactionDescription { get; set; }
}

public sealed class VirtualTerminalReceiptOrderInformation
{
    [JsonPropertyName("displayMerchReferenceNumber")]
    public bool? DisplayMerchReferenceNumber { get; set; }

    [JsonPropertyName("displayCommentOnReceipt")]
    public bool? DisplayCommentOnReceipt { get; set; }

    [JsonPropertyName("displayShippingAddressOnReceipt")]
    public bool? DisplayShippingAddressOnReceipt { get; set; }
}

public sealed class VirtualTerminalReceiptCustomerInformation
{
    [JsonPropertyName("displayPhoneNumberOnReceipt")]
    public bool? DisplayPhoneNumberOnReceipt { get; set; }

    [JsonPropertyName("displayEmailAddressOnReceipt")]
    public bool? DisplayEmailAddressOnReceipt { get; set; }

    [JsonPropertyName("displayCustomerIdOnReceipt")]
    public bool? DisplayCustomerIdOnReceipt { get; set; }

    [JsonPropertyName("displayCompanyOnReceipt")]
    public bool? DisplayCompanyOnReceipt { get; set; }

    [JsonPropertyName("displayCustomerIdOnReceiptForBilling")]
    public bool? DisplayCustomerIdOnReceiptForBilling { get; set; }

    [JsonPropertyName("displayFirstNameOnReceiptForBilling")]
    public bool? DisplayFirstNameOnReceiptForBilling { get; set; }

    [JsonPropertyName("displayLastNameOnReceiptForBilling")]
    public bool? DisplayLastNameOnReceiptForBilling { get; set; }

    [JsonPropertyName("displayCompanyOnReceiptForBilling")]
    public bool? DisplayCompanyOnReceiptForBilling { get; set; }

    [JsonPropertyName("displayStreetAddress1OnReceiptForBilling")]
    public bool? DisplayStreetAddress1OnReceiptForBilling { get; set; }

    [JsonPropertyName("displayStreetAddress2OnReceiptForBilling")]
    public bool? DisplayStreetAddress2OnReceiptForBilling { get; set; }

    [JsonPropertyName("displayCityOnReceiptForBilling")]
    public bool? DisplayCityOnReceiptForBilling { get; set; }

    [JsonPropertyName("displayStateOnReceiptForBilling")]
    public bool? DisplayStateOnReceiptForBilling { get; set; }

    [JsonPropertyName("displayZipPostalCodeOnReceiptForBilling")]
    public bool? DisplayZipPostalCodeOnReceiptForBilling { get; set; }

    [JsonPropertyName("displayCountryOnReceiptForBilling")]
    public bool? DisplayCountryOnReceiptForBilling { get; set; }

    [JsonPropertyName("displayPhoneNumberOnReceiptForBilling")]
    public bool? DisplayPhoneNumberOnReceiptForBilling { get; set; }

    [JsonPropertyName("displayEmailAddressOnReceiptForBilling")]
    public bool? DisplayEmailAddressOnReceiptForBilling { get; set; }

    [JsonPropertyName("displayFirstNameOnReceiptForShipping")]
    public bool? DisplayFirstNameOnReceiptForShipping { get; set; }

    [JsonPropertyName("displayLastNameOnReceiptForShipping")]
    public bool? DisplayLastNameOnReceiptForShipping { get; set; }

    [JsonPropertyName("displayCompanyOnReceiptForShipping")]
    public bool? DisplayCompanyOnReceiptForShipping { get; set; }

    [JsonPropertyName("displayStreetAddress1OnReceiptForShipping")]
    public bool? DisplayStreetAddress1OnReceiptForShipping { get; set; }

    [JsonPropertyName("displayStreetAddress2OnReceiptForShipping")]
    public bool? DisplayStreetAddress2OnReceiptForShipping { get; set; }

    [JsonPropertyName("displayCityOnReceiptForShipping")]
    public bool? DisplayCityOnReceiptForShipping { get; set; }

    [JsonPropertyName("displayStateOnReceiptForShipping")]
    public bool? DisplayStateOnReceiptForShipping { get; set; }

    [JsonPropertyName("displayZipPostalCodeOnReceiptForShipping")]
    public bool? DisplayZipPostalCodeOnReceiptForShipping { get; set; }

    [JsonPropertyName("displayCountryOnReceiptForShipping")]
    public bool? DisplayCountryOnReceiptForShipping { get; set; }

    [JsonPropertyName("displayPhoneNumberOnReceiptForShipping")]
    public bool? DisplayPhoneNumberOnReceiptForShipping { get; set; }
}

public sealed class VirtualTerminalReceiptResultFields
{
    [JsonPropertyName("displayAddressVerificationSystemResults")]
    public bool? DisplayAddressVerificationSystemResults { get; set; }

    [JsonPropertyName("displayAuthCode")]
    public bool? DisplayAuthCode { get; set; }
}

public sealed class VirtualTerminalEmailReceipt
{
    [JsonPropertyName("copyConsumer")]
    public bool? CopyConsumer { get; set; }
}

public sealed class VirtualTerminalReceiptLevelTwoFields
{
    [JsonPropertyName("displayLevel2DutyOnReceipt")]
    public bool? DisplayLevel2DutyOnReceipt { get; set; }

    [JsonPropertyName("displayLevel2PurchOrderNumber")]
    public bool? DisplayLevel2PurchOrderNumber { get; set; }

    [JsonPropertyName("displayLevel2TaxOnReceipt")]
    public bool? DisplayLevel2TaxOnReceipt { get; set; }

    [JsonPropertyName("displayLevel2TaxExemptOnReceipt")]
    public bool? DisplayLevel2TaxExemptOnReceipt { get; set; }
}

public sealed class VirtualTerminalReceiptLevelTwoAndLevelThreeFields
{
    [JsonPropertyName("displayAlternateTaxAmount")]
    public bool? DisplayAlternateTaxAmount { get; set; }
    [JsonPropertyName("displayAlternateTaxIndicator")]
    public bool? DisplayAlternateTaxIndicator { get; set; }
    [JsonPropertyName("displayAlternateTaxId")]
    public bool? DisplayAlternateTaxId { get; set; }
    [JsonPropertyName("displayAmexDataTAA1")]
    public bool? DisplayAmexDataTAA1 { get; set; }
    [JsonPropertyName("displayAmexDataTAA2")]
    public bool? DisplayAmexDataTAA2 { get; set; }
    [JsonPropertyName("displayAmexDataTAA3")]
    public bool? DisplayAmexDataTAA3 { get; set; }
    [JsonPropertyName("displayAmexDataTAA4")]
    public bool? DisplayAmexDataTAA4 { get; set; }
    [JsonPropertyName("displayDutyAmount")]
    public bool? DisplayDutyAmount { get; set; }
    [JsonPropertyName("displayFreightAmount")]
    public bool? DisplayFreightAmount { get; set; }
    [JsonPropertyName("displayUserPurchaseOrder")]
    public bool? DisplayUserPurchaseOrder { get; set; }
    [JsonPropertyName("displayLocalTaxAmount")]
    public bool? DisplayLocalTaxAmount { get; set; }
    [JsonPropertyName("displayLocalTaxIndicator")]
    public bool? DisplayLocalTaxIndicator { get; set; }
    [JsonPropertyName("displayMerchantVATRegistrationNumber")]
    public bool? DisplayMerchantVATRegistrationNumber { get; set; }
    [JsonPropertyName("displayNationalTaxAmount")]
    public bool? DisplayNationalTaxAmount { get; set; }
    [JsonPropertyName("displayNationalTaxIndicator")]
    public bool? DisplayNationalTaxIndicator { get; set; }
    [JsonPropertyName("displayDiscountAmount")]
    public bool? DisplayDiscountAmount { get; set; }
    [JsonPropertyName("displayPurchaserCode")]
    public bool? DisplayPurchaserCode { get; set; }
    [JsonPropertyName("displayPurchaserOrderDate")]
    public bool? DisplayPurchaserOrderDate { get; set; }
    [JsonPropertyName("purchaserVATRegistrationNumberOnReceipt")]
    public bool? PurchaserVATRegistrationNumberOnReceipt { get; set; }
    [JsonPropertyName("displayShipFromPostalCode")]
    public bool? DisplayShipFromPostalCode { get; set; }
    [JsonPropertyName("displaySummaryCommodityCode")]
    public bool? DisplaySummaryCommodityCode { get; set; }
    [JsonPropertyName("displaySupplierOrderReferenceNumber")]
    public bool? DisplaySupplierOrderReferenceNumber { get; set; }
    [JsonPropertyName("displayTaxable")]
    public bool? DisplayTaxable { get; set; }
    [JsonPropertyName("displayVatInvoiceReferenceNumber")]
    public bool? DisplayVatInvoiceReferenceNumber { get; set; }
    [JsonPropertyName("displayVatTaxAmount")]
    public bool? DisplayVatTaxAmount { get; set; }
    [JsonPropertyName("displayVatTaxRate")]
    public bool? DisplayVatTaxRate { get; set; }
    [JsonPropertyName("displayItemAlternateTaxAmount")]
    public bool? DisplayItemAlternateTaxAmount { get; set; }
    [JsonPropertyName("displayItemAlternateTaxId")]
    public bool? DisplayItemAlternateTaxId { get; set; }
    [JsonPropertyName("displayItemAlternateTaxRate")]
    public bool? DisplayItemAlternateTaxRate { get; set; }
    [JsonPropertyName("displayItemAlternateTaxTypeApp")]
    public bool? DisplayItemAlternateTaxTypeApp { get; set; }
    [JsonPropertyName("displayItemAlternateTaxType")]
    public bool? DisplayItemAlternateTaxType { get; set; }
    [JsonPropertyName("displayItemUnitPrice")]
    public bool? DisplayItemUnitPrice { get; set; }
    [JsonPropertyName("displayItemCommodityCode")]
    public bool? DisplayItemCommodityCode { get; set; }
    [JsonPropertyName("displayItemDiscountAmount")]
    public bool? DisplayItemDiscountAmount { get; set; }
    [JsonPropertyName("displayItemDiscountIndicator")]
    public bool? DisplayItemDiscountIndicator { get; set; }
    [JsonPropertyName("displayItemDiscountRate")]
    public bool? DisplayItemDiscountRate { get; set; }
    [JsonPropertyName("displayItemGrossNetIndicator")]
    public bool? DisplayItemGrossNetIndicator { get; set; }
    [JsonPropertyName("diplayItemLocalTax")]
    public bool? DiplayItemLocalTax { get; set; }
    [JsonPropertyName("displayItemProductSKU")]
    public bool? DisplayItemProductSKU { get; set; }
    [JsonPropertyName("displayItemNationalTax")]
    public bool? DisplayItemNationalTax { get; set; }
    [JsonPropertyName("displayItemProductCode")]
    public bool? DisplayItemProductCode { get; set; }
    [JsonPropertyName("displayItemProductName")]
    public bool? DisplayItemProductName { get; set; }
    [JsonPropertyName("displayItemQuantity")]
    public bool? DisplayItemQuantity { get; set; }
    [JsonPropertyName("displayItemTaxAmount")]
    public bool? DisplayItemTaxAmount { get; set; }
    [JsonPropertyName("displayItemTaxRate")]
    public bool? DisplayItemTaxRate { get; set; }
    [JsonPropertyName("displayItemTaxTypeApplied")]
    public bool? DisplayItemTaxTypeApplied { get; set; }
    [JsonPropertyName("displayItemTotalAmount")]
    public bool? DisplayItemTotalAmount { get; set; }
    [JsonPropertyName("itemUnitOfMeasureOnReceipt")]
    public bool? ItemUnitOfMeasureOnReceipt { get; set; }
    [JsonPropertyName("displayItemVatRate")]
    public bool? DisplayItemVatRate { get; set; }
    [JsonPropertyName("displayItemInvoiceNumber")]
    public bool? DisplayItemInvoiceNumber { get; set; }
    [JsonPropertyName("displayLevel3LineItemOnReceipt")]
    public bool? DisplayLevel3LineItemOnReceipt { get; set; }
    [JsonPropertyName("displayLevel3AlternateTaxOnReceipt")]
    public bool? DisplayLevel3AlternateTaxOnReceipt { get; set; }
}

public sealed class VirtualTerminalReceiptMerchantDefinedFields
{
    [JsonPropertyName("displayMerchantData1")]
    public bool? DisplayMerchantData1 { get; set; }
    [JsonPropertyName("displayMerchantData2")]
    public bool? DisplayMerchantData2 { get; set; }
    [JsonPropertyName("displayMerchantData3")]
    public bool? DisplayMerchantData3 { get; set; }
    [JsonPropertyName("displayMerchantData4")]
    public bool? DisplayMerchantData4 { get; set; }
    [JsonPropertyName("displayMerchantData5")]
    public bool? DisplayMerchantData5 { get; set; }
    [JsonPropertyName("displayMerchantData6")]
    public bool? DisplayMerchantData6 { get; set; }
    [JsonPropertyName("displayMerchantData7")]
    public bool? DisplayMerchantData7 { get; set; }
    [JsonPropertyName("displayMerchantData8")]
    public bool? DisplayMerchantData8 { get; set; }
    [JsonPropertyName("displayMerchantData9")]
    public bool? DisplayMerchantData9 { get; set; }
    [JsonPropertyName("displayMerchantData10")]
    public bool? DisplayMerchantData10 { get; set; }
    [JsonPropertyName("displayMerchantData11")]
    public bool? DisplayMerchantData11 { get; set; }
    [JsonPropertyName("displayMerchantData12")]
    public bool? DisplayMerchantData12 { get; set; }
    [JsonPropertyName("displayMerchantData13")]
    public bool? DisplayMerchantData13 { get; set; }
    [JsonPropertyName("displayMerchantData14")]
    public bool? DisplayMerchantData14 { get; set; }
    [JsonPropertyName("displayMerchantData15")]
    public bool? DisplayMerchantData15 { get; set; }
    [JsonPropertyName("displayMerchantData16")]
    public bool? DisplayMerchantData16 { get; set; }
    [JsonPropertyName("displayMerchantData17")]
    public bool? DisplayMerchantData17 { get; set; }
    [JsonPropertyName("displayMerchantData18")]
    public bool? DisplayMerchantData18 { get; set; }
    [JsonPropertyName("displayMerchantData19")]
    public bool? DisplayMerchantData19 { get; set; }
    [JsonPropertyName("displayMerchantData20")]
    public bool? DisplayMerchantData20 { get; set; }
}

public sealed class VirtualTerminalPrintableData
{
    [JsonPropertyName("printSingleReceipt")]
    public bool? PrintSingleReceipt { get; set; }

    [JsonPropertyName("printDualReceipt")]
    public bool? PrintDualReceipt { get; set; }
}

public sealed class VirtualTerminalReceiptEmvFields
{
    [JsonPropertyName("europayMasterCardVisaAuthCode")]
    public bool? EuropayMasterCardVisaAuthCode { get; set; }

    [JsonPropertyName("europayMasterCardVisaAuthMode")]
    public bool? EuropayMasterCardVisaAuthMode { get; set; }

    [JsonPropertyName("europayMasterCardVisaEntryMode")]
    public bool? EuropayMasterCardVisaEntryMode { get; set; }

    [JsonPropertyName("europayMasterCardVisaSignatureOnCustomerCopy")]
    public bool? EuropayMasterCardVisaSignatureOnCustomerCopy { get; set; }

    [JsonPropertyName("europayMasterCardVisaTerminalIdValue")]
    public bool? EuropayMasterCardVisaTerminalIdValue { get; set; }
}

public sealed class VirtualTerminalReaderInformation
{
    [JsonPropertyName("terminalSupport")]
    public VirtualTerminalTerminalSupport? TerminalSupport { get; set; }

    [JsonPropertyName("terminalSettings")]
    public VirtualTerminalTerminalSettings? TerminalSettings { get; set; }
}

public sealed class VirtualTerminalTerminalSupport
{
    [JsonPropertyName("enableCreditCardCapture")]
    public bool? EnableCreditCardCapture { get; set; }

    [JsonPropertyName("cardReaderPresent")]
    public bool? CardReaderPresent { get; set; }

    [JsonPropertyName("wisepadDecryptionService")]
    public string? WisepadDecryptionService { get; set; }
}

public sealed class VirtualTerminalTerminalSettings
{
    [JsonPropertyName("europayMasterCardVisaReaderUpdateCheck")]
    public bool? EuropayMasterCardVisaReaderUpdateCheck { get; set; }

    [JsonPropertyName("europayMasterCardVisaEnabled")]
    public bool? EuropayMasterCardVisaEnabled { get; set; }

    [JsonPropertyName("europayMasterCardVisaChipEnabled")]
    public bool? EuropayMasterCardVisaChipEnabled { get; set; }
}

public sealed class VirtualTerminalEmbedded
{
    [JsonPropertyName("doesProcessorSupportsForcedCapture")]
    public bool? DoesProcessorSupportsForcedCapture { get; set; }

    [JsonPropertyName("japaneseMerchant")]
    public bool? JapaneseMerchant { get; set; }

    [JsonPropertyName("serviceFeeMerchantConfigFlagEnabled")]
    public bool? ServiceFeeMerchantConfigFlagEnabled { get; set; }

    [JsonPropertyName("profileEnabled")]
    public bool? ProfileEnabled { get; set; }

    [JsonPropertyName("isProfileEnabled")]
    public bool? IsProfileEnabled { get; set; }
}
