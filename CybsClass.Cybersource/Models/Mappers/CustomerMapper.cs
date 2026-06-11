using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.Cybersource.Models.Mappers
{
    public static class CustomerMapper
    {
        public static B2cCustomerDto? Map(B2cCustomer b2cCustomer)
        {
            return b2cCustomer != null ? new B2cCustomerDto
            {
                FirstName = b2cCustomer.FirstName,
                LastName = b2cCustomer.LastName,
                Address1 = b2cCustomer.Address1,
                Address2 = b2cCustomer.Address2,
                City = b2cCustomer.City,
                AdministrativeArea = b2cCustomer.Region,
                PostalCode = b2cCustomer.PostalCode,
                Country = b2cCustomer.Country,
                Phone = b2cCustomer.Phone,
                Email = b2cCustomer.Email
            } : null;
        }

        public static B2cCustomer? Map(B2cCustomerDto b2cCustomerDto)
        {
            return b2cCustomerDto != null ? new B2cCustomer
            {
                FirstName = b2cCustomerDto.FirstName,
                LastName = b2cCustomerDto.LastName,
                Email = b2cCustomerDto.Email,
                Address1 = b2cCustomerDto.Address1,
                Address2 = b2cCustomerDto.Address2,
                City = b2cCustomerDto.City,
                Region = b2cCustomerDto.AdministrativeArea,
                Phone = b2cCustomerDto.Phone,
                PostalCode = b2cCustomerDto.PostalCode,
                Country = b2cCustomerDto.Country,
            } : null;
        }

        public static List<B2cCustomer> Map(List<B2cCustomerDto> b2cCustomerDtos)
        {
            return b2cCustomerDtos.Select(Map).ToList()!;
        }

        public static List<B2cCustomerDto> Map(List<B2cCustomer> b2cCustomers)
        {
            return b2cCustomers.Select(Map).ToList()!;
        }
    }
}
