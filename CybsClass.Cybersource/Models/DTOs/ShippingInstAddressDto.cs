using CybsClass.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybsClass.Cybersource.Models.DTOs
{
    public class ShippingInstAddressDto
    {
        public string? ShippingInstId { get; set; }

        public string? FirstName { get; set; } 

        public string? LastName { get; set; } 

        public string? Email { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }
        public string? Company { get; set; }

        public string? Error { get; set; }
    }
}
