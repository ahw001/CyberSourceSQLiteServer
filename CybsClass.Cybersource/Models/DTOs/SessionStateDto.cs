using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CybsClass.Cybersource.Models.OutboundTransObjects;

namespace CybsClass.Cybersource.Models.DTOs
{
    public class SessionStateDto
    {
        public Guid Id { get; set; }

        public string SerializedData { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
