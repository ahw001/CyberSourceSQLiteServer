using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CybsClass.Cybersource.Models.BaseData;


namespace CybsClass.Cybersource.Models.OutboundTransObjects
{


    public class InvoiceData
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("customerInformation")]
        public CustomerInformation? CustomerInformation { get; set; }

        [JsonPropertyName("orderInformation")]
        public OrderInformation? OrderInformation { get; set; }

        [JsonPropertyName("processingInformation")]
        public ProcessingInformation? ProcessingInformation { get; set; }
        
        [JsonPropertyName("invoiceInformation")]
        public InvoiceInformation? InvoiceInformation { get; set; }

    }

}



