using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingVirtualTerminalGlobalPaymentInfo")]
public class BoardingVirtualTerminalGlobalPaymentInfo
{
    [Key]
    public int BoardingVirtualTerminalGlobalPaymentInfoId { get; set; }

    public int BoardingVirtualTerminalSubscriptionId { get; set; }

    // basicInformation
    [StringLength(10)] public string? DefaultCountryCode { get; set; }
    [StringLength(10)] public string? DefaultCurrencyCode { get; set; }
    [StringLength(50)] public string? DefaultPaymentType { get; set; }
    [StringLength(50)] public string? DefaultTransactionSource { get; set; }
    public bool? DisplayRetail { get; set; }
    public bool? DisplayMoto { get; set; }
    public bool? DisplayInternet { get; set; }
    [StringLength(10)] public string? DefaultStandardEntryClassCode { get; set; }

    // paymentInformation (scalars)
    public bool? DisplayAuthIndicator { get; set; }
    public bool? DisplayJapanPayOnly { get; set; }
    public bool? DisplayCreditCards { get; set; }
    public bool? DisplayEchecks { get; set; }
    public bool? DisplayDebtIndicator { get; set; }
    public bool? DisplayBillPayment { get; set; }
    public bool? DisplayECheckEffectiveDate { get; set; }
    public bool? DisplayIgnoreECheckAvsCheckbox { get; set; }
    public bool? DisplayECheckReferenceNumber { get; set; }
    public bool? RequireECheckReferenceNumber { get; set; }
    public bool? EnableEchecks { get; set; }

    // customerInformation
    public bool? DisplayIgnoreAvsCheckbox { get; set; }
    public bool? RequirePhoneNumber { get; set; }
    public bool? RequireEmailAddress { get; set; }
    public bool? RequireCustomerId { get; set; }
    public bool? RequireBillingAddress { get; set; }
    public bool? DisplayEmailAddress { get; set; }
    public bool? DisplayCompany { get; set; }
    public bool? DisplayBillingAddress { get; set; }
    public bool? DisplayCustomerId { get; set; }
    public bool? RequireCustomerCompany { get; set; }
    public bool? DisplayCustomerIdForBilling { get; set; }
    public bool? RequireCustomerIdForBilling { get; set; }
    public bool? RequireFirstNameForBilling { get; set; }
    public bool? DisplayFirstNameForBilling { get; set; }
    public bool? RequireLastNameForBilling { get; set; }
    public bool? DisplayLastNameForBilling { get; set; }
    public bool? RequireCompanyNameForBilling { get; set; }
    public bool? DisplayCompanyNameForBilling { get; set; }
    public bool? RequireStreetAddress1ForBilling { get; set; }
    public bool? DisplayStreetAddress1ForBilling { get; set; }
    public bool? RequireStreetAddress2ForBilling { get; set; }
    public bool? DisplayStreetAddress2ForBilling { get; set; }
    public bool? RequireCityForBilling { get; set; }
    public bool? DisplayCityForBilling { get; set; }
    public bool? RequireStateForBilling { get; set; }
    public bool? DisplayStateForBilling { get; set; }
    public bool? RequireZipPostalCodeForBilling { get; set; }
    public bool? DisplayZipPostalCodeForBilling { get; set; }
    public bool? RequireCountryForBilling { get; set; }
    public bool? DisplayCountryForBilling { get; set; }
    public bool? RequirePhoneNumberForBilling { get; set; }
    public bool? DisplayPhoneNumberForBilling { get; set; }
    public bool? RequireEmailAddressForBilling { get; set; }
    public bool? DisplayEmailAddressForBilling { get; set; }
    public bool? RequireFirstNameForShipping { get; set; }
    public bool? DisplayFirstNameForShipping { get; set; }
    public bool? RequireLastNameForShipping { get; set; }
    public bool? DisplayLastNameForShipping { get; set; }
    public bool? RequireCompanyNameForShipping { get; set; }
    public bool? DisplayCompanyNameForShipping { get; set; }
    public bool? RequireStreetAddress1ForShipping { get; set; }
    public bool? DisplayStreetAddress1ForShipping { get; set; }
    public bool? RequireStreetAddress2ForShipping { get; set; }
    public bool? DisplayStreetAddress2ForShipping { get; set; }
    public bool? RequireCityForShipping { get; set; }
    public bool? DisplayCityForShipping { get; set; }
    public bool? RequireStateForShipping { get; set; }
    public bool? DisplayStateForShipping { get; set; }
    public bool? RequirePostalCodeForShipping { get; set; }
    public bool? DisplayPostalCodeForShipping { get; set; }
    public bool? RequireCountryForShipping { get; set; }
    public bool? DisplayCountryForShipping { get; set; }
    public bool? RequirePhoneNumberForShipping { get; set; }
    public bool? DisplayPhoneNumberForShipping { get; set; }
    public bool? DisplayPhoneNumber { get; set; }
    public bool? FirstNameRequired { get; set; }
    public bool? LastNameRequired { get; set; }
    public bool? DisplayFirstName { get; set; }
    public bool? DisplayLastName { get; set; }
    public bool? FirstNameShowReceipt { get; set; }
    public bool? LastNameShowReceipt { get; set; }

    // orderInformation
    public bool? DisplayShippingAddress { get; set; }
    public bool? RequireShippingAddress { get; set; }
    public bool? ShippingAddressDisplayOnReceipt { get; set; }
    public bool? MerchantDescriptorContact { get; set; }
    public bool? MerchantDescriptorAdditionalInformation { get; set; }
    public bool? DisplayOrderNumber { get; set; }
    public bool? RequireTransactionReferenceNumber { get; set; }
    public bool? RequireOrderNumber { get; set; }
    public bool? DisplayMerchantDescriptor { get; set; }
    public bool? DisplayTransactionReferenceNumber { get; set; }
    public bool? DisplayComment { get; set; }
    public bool? RequireComment { get; set; }
    public bool? CommentShowReceipt { get; set; }
    public bool? OrderNumberDisplayOnReceipt { get; set; }
    public bool? CompanyDisplayOnReceipt { get; set; }
    public bool? AvsResultsDisplayOnReceipt { get; set; }
    public bool? AuthCodeDisplayOnReceipt { get; set; }
    public bool? EnableEcheckServiceFee { get; set; }
    public bool? OrderInfoEmvReaderUpdateCheck { get; set; }

    // levelTwoFields
    public bool? DisplayLevel2Duty { get; set; }
    public bool? Level2DutyRequired { get; set; }
    public bool? DisplayLevel2PurchaseOrderNumber { get; set; }
    public bool? RequireLevel2PurchaseOrderNumber { get; set; }
    public bool? DisplayLevel2Tax { get; set; }
    public bool? RequireLevel2Tax { get; set; }
    public bool? DisplayLevel2TaxExempt { get; set; }
    public bool? RequireLevel2TaxExempt { get; set; }
    public bool? Level2L3Enabled { get; set; }
    public bool? Level2DutyDisplayOnReceipt { get; set; }
    public bool? Level2PurchaseOrderNumberDisplayOnReceipt { get; set; }
    public bool? Level2TaxDisplayOnReceipt { get; set; }
    public bool? Level2TaxExemptDisplayOnReceipt { get; set; }
    [StringLength(50)] public string? Level3Configuration { get; set; }

    // levelThreeFields
    [StringLength(50)]  public string? ItemInvoiceNumberQuantityCustom { get; set; }
    [StringLength(200)] public string? L3VirtualTerminalReceiptHeader { get; set; }
    public bool? Level3AlternateTaxRequired { get; set; }
    public bool? DisplayLevel3AlternateTax { get; set; }
    public bool? ItemAlternateTaxAmountDisplayOnReceipt { get; set; }
    public bool? ItemAlternateTaxIdDisplayOnReceipt { get; set; }
    public bool? ItemAlternateTaxRateDisplayOnReceipt { get; set; }
    public bool? ItemAltTaxTypeAppDisplayOnReceipt { get; set; }
    public bool? ItemAltTaxTypeDisplayOnReceipt { get; set; }
    public bool? ItemUnitPriceDisplayOnReceipt { get; set; }
    public bool? ItemCommodityCodeDisplayOnReceipt { get; set; }
    public bool? ItemDiscountAmountDisplayOnReceipt { get; set; }
    public bool? ItemDiscountIndicatorDisplayOnReceipt { get; set; }
    public bool? ItemDiscountRateDisplayOnReceipt { get; set; }
    public bool? ItemGrossNetIndicatorDisplayOnReceipt { get; set; }
    public bool? ItemLocalTaxDisplayOnReceipt { get; set; }
    public bool? ItemProductSKUDisplayOnReceipt { get; set; }
    public bool? ItemNationalTaxDisplayOnReceipt { get; set; }
    public bool? ItemProductCodeDisplayOnReceipt { get; set; }
    public bool? ItemProductNameDisplayOnReceipt { get; set; }
    public bool? ItemQuantityDisplayOnReceipt { get; set; }
    public bool? ItemTaxAmountDisplayOnReceipt { get; set; }
    public bool? ItemTaxRateDisplayOnReceipt { get; set; }
    public bool? ItemTaxTypeAppliedDisplayOnReceipt { get; set; }
    public bool? ItemTotalAmountDisplayOnReceipt { get; set; }
    public bool? ItemUnitOfMeasureDisplayOnReceipt { get; set; }
    public bool? ItemVatRateDisplayOnReceipt { get; set; }
    public bool? ItemInvoiceNumberDisplayOnReceipt { get; set; }
    public bool? Level3LineItemDisplayOnReceipt { get; set; }
    public bool? L3CopyConsumer { get; set; }
    public bool? PrintSingleDisplayOnReceipt { get; set; }
    public bool? PrintDualDisplayOnReceipt { get; set; }
    public bool? Level3AlternateTaxDisplayOnReceipt { get; set; }
    public bool? AlternateTaxAmountDisplayOnReceipt { get; set; }
    public bool? AlternateTaxIndicatorDisplayOnReceipt { get; set; }
    public bool? AlternateTaxIdDisplayOnReceipt { get; set; }
    public bool? AmexDataTAA1DisplayOnReceipt { get; set; }
    public bool? AmexDataTAA2DisplayOnReceipt { get; set; }
    public bool? AmexDataTAA3DisplayOnReceipt { get; set; }
    public bool? AmexDataTAA4DisplayOnReceipt { get; set; }
    public bool? DutyAmountDisplayOnReceipt { get; set; }
    public bool? FreightAmountDisplayOnReceipt { get; set; }
    public bool? UserPurchaseOrderDisplayOnReceipt { get; set; }
    public bool? LocalTaxAmountDisplayOnReceipt { get; set; }
    public bool? LocalTaxIndicatorDisplayOnReceipt { get; set; }
    public bool? MerchantVatRegNumberDisplayOnReceipt { get; set; }
    public bool? NationalTaxAmountDisplayOnReceipt { get; set; }
    public bool? NationalTaxIndicatorDisplayOnReceipt { get; set; }
    public bool? DiscountAmountDisplayOnReceipt { get; set; }
    public bool? PurchaserCodeDisplayOnReceipt { get; set; }
    public bool? PurchaserOrderDateDisplayOnReceipt { get; set; }
    public bool? PurchaserVatRegNumberDisplayOnReceipt { get; set; }
    public bool? ShipFromPostalCodeDisplayOnReceipt { get; set; }
    public bool? SmryCommodityCodeDisplayOnReceipt { get; set; }
    public bool? SupplierOrderReferenceDisplayOnReceipt { get; set; }
    public bool? TaxableDisplayOnReceipt { get; set; }
    public bool? VatInvoiceReferenceNumberDisplayOnReceipt { get; set; }
    public bool? VatTaxAmountDisplayOnReceipt { get; set; }
    public bool? VatTaxRateDisplayOnReceipt { get; set; }

    // merchantDescriptor receipt toggles
    public bool? MerchantDescriptorReceipt { get; set; }
    public bool? MerchantDescriptorContactReceipt { get; set; }

    // virtualTerminalPage
    public bool? VirtualTerminalLongFormEnabled { get; set; }

    // serviceFeeTermsAndConditions
    public bool? ServiceFeeEnabled { get; set; }
    public bool? ServiceFeeInSubscriptionOtpEnabled { get; set; }
    public bool? ServiceFeeInPaymentTokenOtpEnabled { get; set; }

    // differentialFee
    public bool? DisplaySurchargeAmount { get; set; }
    public bool? DisplayCalculateSurchargeAmount { get; set; }

    [ForeignKey("BoardingVirtualTerminalSubscriptionId")]
    public virtual BoardingVirtualTerminalSubscription BoardingVirtualTerminalSubscription { get; set; } = null!;
}
