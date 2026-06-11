using Microsoft.EntityFrameworkCore.ChangeTracking;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Json;
using CybsClass.EntityModels;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public static class PersistShippingInstAddress
    {
        private static Dictionary<string, string> dbResults = new Dictionary<string, string>();

        public static async Task<Dictionary<string, string>> InsertShippingInstAddress(RootToken rootToken, string customerInstId)
        {
            dbResults = new();

            Console.WriteLine("Inserting shipping instrument ...");
            try
            {
                using B2cNorthwindContext db = new();

                ShippingInstAddress s = new();

                s.CustomerInstId = customerInstId;
                s.ShippingInstId = rootToken!.Id!;
                s.FirstName = rootToken.ShipTo!.FirstName! ?? "null";
                s.LastName = rootToken.ShipTo!.LastName! ?? "null";
                s.Phone = rootToken.ShipTo!.PhoneNumber! ?? "null";
                s.Email = rootToken.ShipTo!.Email! ?? "null";
                s.Address1 = rootToken.ShipTo!.Address1! ?? "null";
                //s.Address2 = Address2;
                s.City = rootToken.ShipTo!.Locality! ?? "null";
                s.Region = rootToken.ShipTo!.AdministrativeArea ?? "null";  
                s.PostalCode = rootToken.ShipTo!.PostalCode! ?? "null";
                s.Country = rootToken.ShipTo!.Country! ?? "null";

                EntityEntry<ShippingInstAddress> entity = db.ShippingInstAddresses.Add(s);
                Console.WriteLine($"ShippingInstAddress State: {entity.State}, ShippingInstAddresses: {s.ShippingInstId}");

                int affected0 = await db.SaveChangesAsync();
                Console.WriteLine($"ShippingInstAddress State: {entity.State}, ShippingInstAddress: {s.ShippingInstId}");
                dbResults.Add("ShippingInstId", s.ShippingInstId);

                return dbResults;
            }
            catch (Exception ex)
            {
                dbResults = new();
                dbResults.Add("Exception", ex.Message);
                Console.WriteLine($"Exception: {ex.Message}");
                return dbResults;
            }
        }

        public static async Task<Dictionary<string, string>> InsertShippingInstAddress(ZeroAuthRootToken zeroAuthRootToken)
        {
            dbResults = new();

            Console.WriteLine("Inserting shipping instrument ...");
            try
            {
                using B2cNorthwindContext db = new();

                ShippingInstAddress s = new();

                s.CustomerInstId = zeroAuthRootToken!.TokenInformation!.Customer!.Id!;
                s.ShippingInstId = zeroAuthRootToken!.TokenInformation!.ShippingAddress!.Id!;


                EntityEntry<ShippingInstAddress> entity = db.ShippingInstAddresses.Add(s);
                Console.WriteLine($"ShippingInstAddress State: {entity.State}, ShippingInstAddresses: {s.ShippingInstId}");

                int affected0 = await db.SaveChangesAsync();
                Console.WriteLine($"ShippingInstAddress State: {entity.State}, ShippingInstAddress: {s.ShippingInstId}");
                dbResults.Add("ShippingInstId", s.ShippingInstId);

                return dbResults;
            }
            catch (Exception ex)
            {
                dbResults = new();
                dbResults.Add("Exception", ex.Message);
                Console.WriteLine($"Exception: {ex.Message}");
                return dbResults;
            }
        }
    }
}
