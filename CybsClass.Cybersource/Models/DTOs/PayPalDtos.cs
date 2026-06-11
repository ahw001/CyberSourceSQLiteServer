using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using CybsClass.Cybersource.Models.BaseData;

namespace CybsClass.Cybersource.Models.DTOs
{
    internal class PayPalDtos
    {
    }

    public class PayPalCreateOrderData
    {
        [JsonPropertyName("clientReferenceInformation")]
        public ClientReferenceInformation? ClientReferenceInformation { get; set; }

        [JsonPropertyName("merchantInformation")]
        public PayPalMerchantInformation? MerchantInformation { get; set; }

        [JsonPropertyName("processingInformation")]
        public PayPalProcessingInformation? ProcessingInformation { get; set; }

        [JsonPropertyName("paymentInformation")]
        public PayPalPaymentInformation? PaymentInformation { get; set; }

        [JsonPropertyName("orderInformation")]
        public OrderInformation? OrderInformation { get; set; }
    }

    public class PayPalMerchantInformation
    {
        [JsonPropertyName("merchantDescriptor")]
        public MerchantDescriptor? MerchantDescriptor { get; set; }

        [JsonPropertyName("returnUrl")]
        public string? ReturnUrl { get; set; }

        [JsonPropertyName("successUrl")]
        public string? SuccessUrl { get; set; }
    }

    public class PayPalProcessingInformation
    {
        [JsonPropertyName("authorizationOptions")]
        public PayPalAuthorizationOptions? AuthorizationOptions { get; set; }
    }

    public class PayPalAuthorizationOptions
    {
        [JsonPropertyName("authType")]
        public string? AuthType { get; set; } // Must be "CAPTURE" for SALE flow
    }

    public class PayPalPaymentInformation
    {
        [JsonPropertyName("paymentType")]
        public PayPalPaymentType? PaymentType { get; set; }
    }

    public class PayPalPaymentType
    {
        [JsonPropertyName("method")]
        public PayPalMethod? Method { get; set; }
    }

    public class PayPalMethod
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; } // "payPal"
    }
    public class PayPalSaleData
    {
        [JsonPropertyName("agreementInformation")]
        public AgreementInformation? AgreementInformation { get; set; }

        [JsonPropertyName("paymentInformation")]
        public PayPalSalePaymentInformation? PaymentInformation { get; set; }

        [JsonPropertyName("processingInformation")]
        public PayPalSaleProcessingInformation? ProcessingInformation { get; set; }
    }

    public class AgreementInformation
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }

    public class PayPalSalePaymentInformation
    {
        [JsonPropertyName("paymentType")]
        public PayPalSalePaymentType? PaymentType { get; set; }
    }

    public class PayPalSalePaymentType
    {
        [JsonPropertyName("method")]
        public PayPalMethod? Method { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; } // "eWallet"
    }

    public class PayPalSaleProcessingInformation
    {
        [JsonPropertyName("actionList")]
        public string[]? ActionList { get; set; }

        [JsonPropertyName("intentsId")]
        public string? IntentsId { get; set; }
    }

}
