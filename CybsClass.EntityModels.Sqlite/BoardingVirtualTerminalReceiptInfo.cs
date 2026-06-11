using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingVirtualTerminalReceiptInfo")]
public class BoardingVirtualTerminalReceiptInfo
{
    [Key]
    public int BoardingVirtualTerminalReceiptInfoId { get; set; }

    public int BoardingVirtualTerminalSubscriptionId { get; set; }

    // header / description
    [StringLength(200)] public string? VirtualTerminalReceiptHeader { get; set; }
    public bool? DisplayTransactionDescription { get; set; }

    // orderInformation
    public bool? DisplayMerchReferenceNumber { get; set; }
    public bool? DisplayCommentOnReceipt { get; set; }
    public bool? DisplayShippingAddressOnReceipt { get; set; }

    // customerInformation
    public bool? DisplayPhoneNumberOnReceipt { get; set; }
    public bool? DisplayEmailAddressOnReceipt { get; set; }
    public bool? DisplayCustomerIdOnReceipt { get; set; }
    public bool? DisplayCompanyOnReceipt { get; set; }
    public bool? DisplayCustomerIdOnReceiptForBilling { get; set; }
    public bool? DisplayFirstNameOnReceiptForBilling { get; set; }
    public bool? DisplayLastNameOnReceiptForBilling { get; set; }
    public bool? DisplayCompanyOnReceiptForBilling { get; set; }
    public bool? DisplayStreetAddr1OnReceiptForBilling { get; set; }
    public bool? DisplayStreetAddr2OnReceiptForBilling { get; set; }
    public bool? DisplayCityOnReceiptForBilling { get; set; }
    public bool? DisplayStateOnReceiptForBilling { get; set; }
    public bool? DisplayZipPostalCodeOnRcptForBilling { get; set; }
    public bool? DisplayCountryOnReceiptForBilling { get; set; }
    public bool? DisplayPhoneNumberOnReceiptForBilling { get; set; }
    public bool? DisplayEmailAddressOnReceiptForBilling { get; set; }
    public bool? DisplayFirstNameOnReceiptForShipping { get; set; }
    public bool? DisplayLastNameOnReceiptForShipping { get; set; }
    public bool? DisplayCompanyOnReceiptForShipping { get; set; }
    public bool? DisplayStreetAddr1OnReceiptForShipping { get; set; }
    public bool? DisplayStreetAddr2OnReceiptForShipping { get; set; }
    public bool? DisplayCityOnReceiptForShipping { get; set; }
    public bool? DisplayStateOnReceiptForShipping { get; set; }
    public bool? DisplayZipPostalCodeOnRcptForShipping { get; set; }
    public bool? DisplayCountryOnReceiptForShipping { get; set; }
    public bool? DisplayPhoneNumberOnReceiptForShipping { get; set; }

    // resultFields
    public bool? DisplayAvsResults { get; set; }
    public bool? DisplayAuthCode { get; set; }

    // emailReceipt
    public bool? CopyConsumer { get; set; }

    // levelTwoFields (receipt)
    public bool? DisplayLevel2DutyOnReceipt { get; set; }
    public bool? DisplayLevel2PurchOrderNumber { get; set; }
    public bool? DisplayLevel2TaxOnReceipt { get; set; }
    public bool? DisplayLevel2TaxExemptOnReceipt { get; set; }

    // levelTwoAndLevelThreeFields (receipt)
    public bool? DisplayAlternateTaxAmount { get; set; }
    public bool? DisplayAlternateTaxIndicator { get; set; }
    public bool? DisplayAlternateTaxId { get; set; }
    public bool? DisplayAmexDataTAA1 { get; set; }
    public bool? DisplayAmexDataTAA2 { get; set; }
    public bool? DisplayAmexDataTAA3 { get; set; }
    public bool? DisplayAmexDataTAA4 { get; set; }
    public bool? DisplayDutyAmount { get; set; }
    public bool? DisplayFreightAmount { get; set; }
    public bool? DisplayUserPurchaseOrder { get; set; }
    public bool? DisplayLocalTaxAmount { get; set; }
    public bool? DisplayLocalTaxIndicator { get; set; }
    public bool? DisplayMerchantVatRegistrationNumber { get; set; }
    public bool? DisplayNationalTaxAmount { get; set; }
    public bool? DisplayNationalTaxIndicator { get; set; }
    public bool? DisplayDiscountAmount { get; set; }
    public bool? DisplayPurchaserCode { get; set; }
    public bool? DisplayPurchaserOrderDate { get; set; }
    public bool? PurchaserVatRegNumberOnReceipt { get; set; }
    public bool? DisplayShipFromPostalCode { get; set; }
    public bool? DisplaySummaryCommodityCode { get; set; }
    public bool? DisplaySupplierOrderReferenceNumber { get; set; }
    public bool? DisplayTaxable { get; set; }
    public bool? DisplayVatInvoiceReferenceNumber { get; set; }
    public bool? DisplayVatTaxAmount { get; set; }
    public bool? DisplayVatTaxRate { get; set; }
    public bool? DisplayItemAlternateTaxAmount { get; set; }
    public bool? DisplayItemAlternateTaxId { get; set; }
    public bool? DisplayItemAlternateTaxRate { get; set; }
    public bool? DisplayItemAlternateTaxTypeApp { get; set; }
    public bool? DisplayItemAlternateTaxType { get; set; }
    public bool? DisplayItemUnitPrice { get; set; }
    public bool? DisplayItemCommodityCode { get; set; }
    public bool? DisplayItemDiscountAmount { get; set; }
    public bool? DisplayItemDiscountIndicator { get; set; }
    public bool? DisplayItemDiscountRate { get; set; }
    public bool? DisplayItemGrossNetIndicator { get; set; }
    public bool? DiplayItemLocalTax { get; set; }
    public bool? DisplayItemProductSKU { get; set; }
    public bool? DisplayItemNationalTax { get; set; }
    public bool? DisplayItemProductCode { get; set; }
    public bool? DisplayItemProductName { get; set; }
    public bool? DisplayItemQuantity { get; set; }
    public bool? DisplayItemTaxAmount { get; set; }
    public bool? DisplayItemTaxRate { get; set; }
    public bool? DisplayItemTaxTypeApplied { get; set; }
    public bool? DisplayItemTotalAmount { get; set; }
    public bool? ItemUnitOfMeasureOnReceipt { get; set; }
    public bool? DisplayItemVatRate { get; set; }
    public bool? DisplayItemInvoiceNumber { get; set; }
    public bool? DisplayLevel3LineItemOnReceipt { get; set; }
    public bool? DisplayLevel3AlternateTaxOnReceipt { get; set; }

    // printableData
    public bool? PrintSingleReceipt { get; set; }
    public bool? PrintDualReceipt { get; set; }

    // europayMasterCardVisaFields
    public bool? EmvAuthCode { get; set; }
    public bool? EmvAuthMode { get; set; }
    public bool? EmvEntryMode { get; set; }
    public bool? EmvSignatureOnCustomerCopy { get; set; }
    public bool? EmvTerminalIdValue { get; set; }

    [ForeignKey("BoardingVirtualTerminalSubscriptionId")]
    public virtual BoardingVirtualTerminalSubscription BoardingVirtualTerminalSubscription { get; set; } = null!;
}
