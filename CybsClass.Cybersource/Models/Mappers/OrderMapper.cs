using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.Cybersource.Models.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto? Map(Order order)
        {
            return order != null ? new OrderDto
            {
                OrderId = order.OrderId,
                B2cCustomerId = order.B2cCustomerId,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                ShipVia = order.ShipVia,
                Freight = order.Freight,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipRegion = order.ShipRegion,
                ShipPostalCode = order.ShipPostalCode,
                ShipCountry = order.ShipCountry
            } : null;
        }

        public static Order? Map(OrderDto orderDto)
        {
            return orderDto != null ? new Order
            {
                OrderId = orderDto.OrderId,
                B2cCustomerId = orderDto.B2cCustomerId,
                OrderDate = orderDto.OrderDate,
                RequiredDate = orderDto.RequiredDate,
                ShippedDate = orderDto.ShippedDate,
                ShipVia = orderDto.ShipVia,
                Freight = orderDto.Freight,
                ShipName = orderDto.ShipName,
                ShipAddress = orderDto.ShipAddress,
                ShipCity = orderDto.ShipCity,
                ShipRegion = orderDto.ShipRegion,
                ShipPostalCode = orderDto.ShipPostalCode,
                ShipCountry = orderDto.ShipCountry
            } : null;
        }

        public static List<Order> Map(List<OrderDto> orderDtos)
        {
            return orderDtos.Select(Map).ToList()!;
        }

        public static List<OrderDto> Map(List<Order> orderInfos)
        {
            return orderInfos.Select(Map).ToList()!;
        }
    }
}
