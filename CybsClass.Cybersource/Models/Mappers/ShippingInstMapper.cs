using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.Cybersource.Models.Mappers
{
    public class ShippingInstMapper
    {
        public static ShippingInstAddressDto? Map(ShippingInstAddress shippingInstAddress)
        {
            return shippingInstAddress != null ? new ShippingInstAddressDto
            {
                ShippingInstId = shippingInstAddress.ShippingInstId,
                FirstName = shippingInstAddress.FirstName,
                LastName = shippingInstAddress.LastName,
                Email = shippingInstAddress.Email,
                Address1 = shippingInstAddress.Address1,
                Address2 = shippingInstAddress.Address2,
                City = shippingInstAddress.City,
                Region = shippingInstAddress.Region,
                PostalCode = shippingInstAddress.PostalCode,
                Country = shippingInstAddress.Country,
                Phone = shippingInstAddress.Phone,
                Company = shippingInstAddress.Company,
            } : null;
        }

        public static ShippingInstAddress? Map(ShippingInstAddressDto shippingAddressInstDto)
        {
            return shippingAddressInstDto != null ? new ShippingInstAddress
            {
                ShippingInstId = shippingAddressInstDto.ShippingInstId!,
                FirstName = shippingAddressInstDto.FirstName,
                LastName = shippingAddressInstDto.LastName,
                Email = shippingAddressInstDto.Email,
                Address1 = shippingAddressInstDto.Address1,
                Address2 = shippingAddressInstDto.Address2,
                City = shippingAddressInstDto.City,
                Region = shippingAddressInstDto.Region,
                PostalCode = shippingAddressInstDto.PostalCode,
                Country = shippingAddressInstDto.Country,
                Phone = shippingAddressInstDto.Phone,
                Company = shippingAddressInstDto.Company,
            } : null;
        }

        public static List<ShippingInstAddress> Map(List<ShippingInstAddressDto> shippingAddressInstDtos)
        {
            return shippingAddressInstDtos.Select(Map).ToList()!;
        }

        public static List<ShippingInstAddressDto> Map(List<ShippingInstAddress> shippingInstAddress)
        {
            return shippingInstAddress.Select(Map).ToList()!;
        }
    }
}
