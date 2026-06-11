using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CybsClass.Cybersource.Models.BaseData;


namespace CybsClass.Cybersource.Models.OutboundTransObjects
{
    public class CustomerTokenData
    {
        [JsonPropertyName("type")]
        public BuyerInformation? BuyerInformation { get; set; }

    }

    public class ShippingInstrumentData
    {
        [JsonPropertyName("defaultInfo")]
        public string? DefaultInfo { get; set; }

        [JsonPropertyName("shipTo")]
        public BillTo? ShipTo { get; set; }
    }

    public class CustomerPaymentInstrumentData
    {
        [JsonPropertyName("card")]
        public FullCard? Card { get; set; }

        [JsonPropertyName("billTo")]
        public BillTo? BillTo { get; set; }
    }

    public class InstrumentTokenData
    {
        //public clientReferenceInformation? clientReferenceInformation { get; set; }
        //public Dictionary<string, Card>? paymentInformation { get; set; }
        //public orderInformation? orderInformation { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("card")]
        public FullCard? Card { get; set; }
    }

    public class PaymentInstrumentData
    {
        [JsonPropertyName("clientReferenceInformation")]
        public ClientReferenceInformation? ClientReferenceInformation { get; set; }

        [JsonPropertyName("paymentInformation")]
        public PaymentInformation? PaymentInformation { get; set; }

        [JsonPropertyName("orderInformation")]
        public OrderInformation? OrderInformation { get; set; }

        [JsonPropertyName("processingInformation")]
        public ProcessingInformation? ProcessingInformation { get; set; }

    }

    public class TransientTokenPayment
    {
        [JsonPropertyName("tokenInformation")]
        public TokenInformation? TokenInformation { get; set; }     
    }


}
