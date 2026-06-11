using Microsoft.EntityFrameworkCore;
using CybsClass.EntityModels;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Mappers;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public class DBOrderDetailServices
    {
        public static async Task<int> GetPaymentCardCountAsync()
        {
            using B2cNorthwindContext db = new();
            return await db.OrderDetails.CountAsync();
        }
        public static async Task<List<OrderDetailDto>> GetOrderDetails()
        {
            try
            {
                Console.WriteLine("Geting full list of Order Details ...");
                using B2cNorthwindContext db = new();
                var orderDetails = await db.OrderDetails.ToListAsync();
                List<OrderDetailDto> orderDetailDtos = OrderDetailMapper.Map(orderDetails)!;
                return orderDetailDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting full list of Order Details: {ex.ToString()}");
                var orderDetails = new List<OrderDetailDto>();
                OrderDetailDto orderDetailDto = new OrderDetailDto();
                orderDetailDto.Error = ex.ToString();
                orderDetails.Add(orderDetailDto);
                return orderDetails;
            }

        }

        public static async Task<OrderDetailDto?> GetOrderDetailByUsingId(int orderdetailid)
        {
            try
            {
                Console.WriteLine($"Geting Order Detail for: {orderdetailid}");
                using B2cNorthwindContext db = new();
                Task<OrderDetail?> task = db.OrderDetails.AsNoTracking()
                            .FirstOrDefaultAsync(model => model.OrderId == orderdetailid);

                var orderDetail = await task;
                if (orderDetail == null)
                {
                    return null;
                }
                return OrderDetailMapper.Map(orderDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Order Detail for ID: {ex.ToString()}");
                var orderDetailDto = new OrderDetailDto();
                orderDetailDto.Error = ex.ToString();
                return orderDetailDto;
            }
        }
        public static async Task<int> CreateOrderDetail(OrderDetailDto orderDetailDto)
        {
            try
            {
                OrderDetail orderDetail = OrderDetailMapper.Map(orderDetailDto)!;
                using B2cNorthwindContext db = new();
                db.OrderDetails.Add(orderDetail);
                var affected = await db.SaveChangesAsync();
                return affected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Creating Order Detail - {ex.ToString()}");
                return 0;
            }

        }
    }
}
