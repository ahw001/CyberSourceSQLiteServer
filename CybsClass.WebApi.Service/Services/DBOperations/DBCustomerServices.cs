using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.WebApi.Service.Services.DBOperations;

public static class DBCustomerServices
{
    public static Dictionary<string, string> dbResults = new();

    public static async Task<Dictionary<string, string>> InsertB2CCustomerAsync(B2cCustomerDto b2cCustomerDto)
    {
        dbResults = new();

        B2cCustomer? b2cCustomer = new()
        {
            FirstName = b2cCustomerDto.FirstName ?? "null",
            LastName = b2cCustomerDto.LastName ?? "null",
            Email = b2cCustomerDto.Email ?? "null",
            Address1 = b2cCustomerDto.Address1 ?? "null",
            Address2 = b2cCustomerDto.Address2 ?? "null",
            City = b2cCustomerDto.City ?? "null",
            Region = b2cCustomerDto.AdministrativeArea ?? "null",
            PostalCode = b2cCustomerDto.PostalCode ?? "null",
            Country = b2cCustomerDto.Country ?? "null",
            Phone = b2cCustomerDto.Phone ?? "null"
        };

        using B2cNorthwindContext db = new();
        await db.B2cCustomers.AddAsync(b2cCustomer);
        var result = db.B2cCustomers.OrderBy(x => x.B2cCustomerId).LastOrDefault();

        EntityEntry<B2cCustomer> entity = db.B2cCustomers.Add(b2cCustomer);
        Console.WriteLine($"B2cCustomer State: {entity.State}, B2cCustomerId: {b2cCustomer.B2cCustomerId}");

        int affected0 = await db.SaveChangesAsync();
        Console.WriteLine($"B2cCustomer State: {entity.State}, B2cCustomerId: {b2cCustomer.B2cCustomerId}");
        dbResults.Add("B2cCustomerId", b2cCustomer.B2cCustomerId.ToString());

        Order o = new();

        o.B2cCustomerId = b2cCustomer.B2cCustomerId;
        o.OrderDate = DateTime.Now;

        EntityEntry<Order> orderEntity = db.Orders.Add(o);
        //Console.WriteLine($"Order State: {orderEntity.State}, OrderId: {o.OrderId}");


        int affected1 = db.SaveChanges();
        //Console.WriteLine($"Order State: {orderEntity.State}, OrderId: {o.OrderId}");

        dbResults.Add("OrderId", o.OrderId.ToString());
        if (b2cCustomerDto is not null && b2cCustomerDto.Cart is not null)
        {
            foreach (var product in b2cCustomerDto.Cart)
            {
                var orderDetails = new OrderDetail();
                orderDetails.OrderId = o.OrderId;
                orderDetails.ProductId = product.ProductId;
                orderDetails.Quantity = 1;
                orderDetails.UnitPrice = product.UnitPrice ?? 0m;

                EntityEntry<OrderDetail> detailEntity = db.OrderDetails.Add(orderDetails);
                //Console.WriteLine($"Order Detail State: {entity.State}, OrderId: {orderDetails.OrderId}");

                int affected2 = db.SaveChanges();

                //Console.WriteLine($"Order Detail State: {entity.State}, OrderId: {o.OrderId}");
            }
        }
        else
        {
            Console.WriteLine("Nothing found in cart");
        }

        return dbResults;

    }

    public static async Task<int> GetCustomerCountAsync()
    {
        using B2cNorthwindContext db = new();
        return await db.B2cCustomers.CountAsync();
    }

    public static async Task<List<B2cCustomer>> GetB2CCustomers()
    {
        using B2cNorthwindContext db = new();
        List<B2cCustomer> b2CCustomers = await db.B2cCustomers.ToListAsync();
        return b2CCustomers;
    }

    public static async Task<List<PaymentCardInfo>> GetB2CCustomerPaymentCards(int b2ccustomerid)
    {
        using B2cNorthwindContext db = new();
        var paymentCardInfos = await db.PaymentCardInfos.Where(p => p.B2cCustomerId == b2ccustomerid).ToListAsync();
        return paymentCardInfos;
    }

    public static async Task<List<B2cCustomer>> GetB2cCustomerPagedAsync(int pageIndex, int pageSize)
    {
        using B2cNorthwindContext db = new();
        return await db.B2cCustomers
            .Distinct()
            .OrderBy(c => c.B2cCustomerId)
            .Skip(pageIndex)
            .Take(pageSize)
            .ToListAsync();
    }

    public static async Task<B2cCustomer?> GetB2cCustomerByIdAsync(int id)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.B2cCustomers.AsNoTracking()
                .FirstOrDefaultAsync(model => model.B2cCustomerId == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting B2cCustomer by ID: {ex}");
            return null;
        }
    }

    public static async Task<int> UpdateB2cCustomer(int id, B2cCustomer b2cCustomer)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.B2cCustomers
                .Where(model => model.B2cCustomerId == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.B2cCustomerId, b2cCustomer.B2cCustomerId)
                    .SetProperty(m => m.FirstName, b2cCustomer.FirstName)
                    .SetProperty(m => m.LastName, b2cCustomer.LastName)
                    .SetProperty(m => m.Email, b2cCustomer.Email)
                    .SetProperty(m => m.Address1, b2cCustomer.Address1)
                    .SetProperty(m => m.Address2, b2cCustomer.Address2)
                    .SetProperty(m => m.City, b2cCustomer.City)
                    .SetProperty(m => m.Region, b2cCustomer.Region)
                    .SetProperty(m => m.PostalCode, b2cCustomer.PostalCode)
                    .SetProperty(m => m.Country, b2cCustomer.Country)
                    .SetProperty(m => m.Phone, b2cCustomer.Phone));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating B2cCustomer: {ex}");
            return 0;
        }
    }

    public static async Task<B2cCustomer?> CreateB2cCustomerSimple(B2cCustomer b2cCustomer)
    {
        try
        {
            using B2cNorthwindContext db = new();
            db.B2cCustomers.Add(b2cCustomer);
            await db.SaveChangesAsync();
            return b2cCustomer;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating B2cCustomer: {ex}");
            return null;
        }
    }

    public static async Task<int> DeleteB2cCustomer(int id)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.B2cCustomers
                .Where(model => model.B2cCustomerId == id)
                .ExecuteDeleteAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting B2cCustomer: {ex}");
            return 0;
        }
    }
}
