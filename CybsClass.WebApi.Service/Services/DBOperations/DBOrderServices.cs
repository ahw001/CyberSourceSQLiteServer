using Microsoft.EntityFrameworkCore;
using CybsClass.EntityModels;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Mappers;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public class DBOrdersServices
    {
        public static async Task<int> GetOrdersCountAsync()
        {
            using B2cNorthwindContext db = new();
            return await db.Orders.CountAsync();
        }
        public static async Task<List<OrderDto>> GetOrders()
        {
            try
            {
                Console.WriteLine("Geting full list of Orders ...");
                using B2cNorthwindContext db = new();
                var order = await db.Orders.ToListAsync();
                List<OrderDto> orderDtos = OrderMapper.Map(order)!;
                return orderDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting full list of Orders: {ex.ToString()}");
                var order = new List<OrderDto>();
                OrderDto orderDto = new OrderDto();
                orderDto.Error = ex.ToString();
                order.Add(orderDto);
                return order;
            }

        }

        public static async Task<OrderDto?> GetOrdersByUsingId(int orderid)
        {
            try
            {
                Console.WriteLine($"Geting Order for: {orderid}");
                using B2cNorthwindContext db = new();
                Task<Order?> task = db.Orders.AsNoTracking()
                            .FirstOrDefaultAsync(model => model.OrderId == orderid);

                var order = await task;
                if (order == null)
                {
                    return null;
                }
                return OrderMapper.Map(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Order for ID: {ex.ToString()}");
                var orderDto = new OrderDto();
                orderDto.Error = ex.ToString();
                return orderDto;
            }
        }
        public static async Task<int> CreateOrders(OrderDto orderDto)
        {
            try
            {
                Order order = OrderMapper.Map(orderDto)!;
                using B2cNorthwindContext db = new();
                db.Orders.Add(order);
                var affected = await db.SaveChangesAsync();
                return affected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Creating Payment Card - {ex.ToString()}");
                return 0;
            }
        }
    }
}
