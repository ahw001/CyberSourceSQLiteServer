using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CybsClass.Cybersource.Models.BaseData;


namespace CybsClass.Cybersource.Models.OutboundTransObjects
{
    public class FlexApiData
    {
        [JsonPropertyName("fields")]
        public FlexFields Fields { get; set; } = new FlexFields();

    }
}
