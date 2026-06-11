using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CybsClass.Cybersource.Models.BaseData;

namespace CybsClass.Cybersource.Models.OutboundTransObjects
{
    public class ReversalData
    {
        //[JsonPropertyName("id")]
        //public string? Id { get; set; }

        [JsonPropertyName("clientReferenceInformation")]
        public ClientReferenceInformation? ClientReferenceInformation { get; set; }
        
        [JsonPropertyName("reversalInformation")]
        public ReversalInformation? ReversalInformation { get; set; }

    }
}
