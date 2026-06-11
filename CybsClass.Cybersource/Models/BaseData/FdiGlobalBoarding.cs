using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable

namespace CybsClass.Cybersource.Models.BaseData
{
    public sealed class FdiGlobalProcessor
    {
        // empty object {}
        [JsonPropertyName("acquirer")]
        public Dictionary<string, object?>? Acquirer { get; set; }

        [JsonPropertyName("currencies")]
        public ProcessorCurrencies? Currencies { get; set; }

        [JsonPropertyName("paymentTypes")]
        public FdiPaymentTypes? PaymentTypes { get; set; }

        [JsonPropertyName("batchGroup")]
        public string? BatchGroup { get; set; }

        [JsonPropertyName("creditAuthUnsupportedCardTypes")]
        public object? CreditAuthUnsupportedCardTypes { get; set; }

        [JsonPropertyName("enhancedData")]
        public object? EnhancedData { get; set; }

        [JsonPropertyName("merchantTaxId")]
        public string? MerchantTaxId { get; set; }

        [JsonPropertyName("enablePosNetworkSwitching")]
        public bool? EnablePosNetworkSwitching { get; set; }

        [JsonPropertyName("enableTransactionReferenceNumber")]
        public bool? EnableTransactionReferenceNumber { get; set; }
    }

    #region Currencies

    public sealed class ProcessorCurrencies
    {
        [JsonPropertyName("USD")]
        public Currency? USD { get; set; }
    }

    #endregion

    #region Payment Types

    public sealed class FdiPaymentTypes
    {
        [JsonPropertyName("MASTERCARD")]
        public EnabledFlag? MasterCard { get; set; }

        [JsonPropertyName("DISCOVER")]
        public EnabledFlag? Discover { get; set; }

        [JsonPropertyName("VISA")]
        public EnabledFlag? Visa { get; set; }

        [JsonPropertyName("PIN_DEBIT")]
        public PinDebit? PinDebit { get; set; }

        [JsonPropertyName("AMERICAN_EXPRESS")]
        public EnabledFlag? AmericanExpress { get; set; }

        [JsonPropertyName("DINERS_CLUB")]
        public EnabledFlag? DinersClub { get; set; }

        [JsonPropertyName("CUP")]
        public EnabledFlag? Cup { get; set; }
    }


    public sealed class EnabledFlag
    {
        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }
    }

    public sealed class PinDebit
    {
        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }

        [JsonPropertyName("currencies")]
        public PinDebitCurrencies? Currencies { get; set; }
    }

    public sealed class PinDebitCurrencies
    {
        [JsonPropertyName("USD")]
        public PinDebitUsd? USD { get; set; }
    }

    public sealed class PinDebitUsd
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

    #endregion

    #region Features

    public sealed class FeatureConfigurations
    {
        [JsonPropertyName("cardPresent")]
        public CardPresentFeature? CardPresent { get; set; }

        [JsonPropertyName("cardNotPresent")]
        public CardNotPresentFeature? CardNotPresent { get; set; }
    }

    public sealed class CardPresentFeature
    {
        [JsonPropertyName("processors")]
        public CardPresentProcessors? Processors { get; set; }

        [JsonPropertyName("cardPresentSolutionType")]
        public string? CardPresentSolutionType { get; set; }
    }

    public sealed class CardPresentProcessors
    {
        [JsonPropertyName("fdiglobal")]
        public CardPresentFdiGlobal? FdiGlobal { get; set; }
    }

    public sealed class CardPresentFdiGlobal
    {
        [JsonPropertyName("pinDebitEnablePartialAuth")]
        public bool? PinDebitEnablePartialAuth { get; set; }
    }

    public sealed class CardNotPresentFeature
    {
        [JsonPropertyName("processors")]
        public CardNotPresentProcessors? Processors { get; set; }

        [JsonPropertyName("visaStraightThroughProcessingOnly")]
        public bool? VisaStraightThroughProcessingOnly { get; set; }

        [JsonPropertyName("amexTransactionAdviceAddendum1")]
        public object? AmexTransactionAdviceAddendum1 { get; set; }

        [JsonPropertyName("ignoreAddressVerificationSystem")]
        public bool? IgnoreAddressVerificationSystem { get; set; }
    }

    public sealed class CardNotPresentProcessors
    {
        [JsonPropertyName("fdiglobal")]
        public CardNotPresentFdiGlobal? FdiGlobal { get; set; }
    }

    public sealed class CardNotPresentFdiGlobal
    {
        [JsonPropertyName("relaxAddressVerificationSystem")]
        public bool? RelaxAddressVerificationSystem { get; set; }

        [JsonPropertyName("relaxAddressVerificationSystemAllowExpiredCard")]
        public bool? RelaxAddressVerificationSystemAllowExpiredCard { get; set; }

        [JsonPropertyName("relaxAddressVerificationSystemAllowZipWithoutCountry")]
        public bool? RelaxAddressVerificationSystemAllowZipWithoutCountry { get; set; }
    }

    #endregion

}
