using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.Cybersource.Models.Mappers
{
    public static class OrderDetailMapper
    {
        public static OrderDetailDto? Map(OrderDetail orderDetail)
        {
            return orderDetail != null ? new OrderDetailDto
            {
                OrderId = orderDetail.OrderId,
                Discount = orderDetail.Discount,
                ProductId = orderDetail.ProductId,
                UnitPrice = orderDetail.UnitPrice,
                Quantity = orderDetail.Quantity
            } : null;
        }

        public static OrderDetail? Map(OrderDetailDto orderDetailDto)
        {
            return orderDetailDto != null ? new OrderDetail
            {
                OrderId = orderDetailDto.OrderId,
                Discount = orderDetailDto.Discount,
                ProductId = orderDetailDto.ProductId,
                UnitPrice = orderDetailDto.UnitPrice,
                Quantity = orderDetailDto.Quantity
            } : null;
        }

        public static List<OrderDetail> Map(List<OrderDetailDto> orderDetailDtos)
        {
            return orderDetailDtos.Select(Map).ToList()!;
        }

        public static List<OrderDetailDto> Map(List<OrderDetail> orderDetails)
        {
            return orderDetails.Select(Map).ToList()!;
        }
    }
}
