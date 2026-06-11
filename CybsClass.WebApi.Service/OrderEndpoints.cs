using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using CybsClass.EntityModels;
using CybsClass.WebApi.Service.Services.DBOperations;
using Microsoft.AspNetCore.Mvc;
using CybsClass.Cybersource.Models.DTOs;
namespace CybsClass.WebApi.Service;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Order").WithTags(nameof(Order));

        group.MapGet("/count", async () =>
        {
            return Results.Ok(await DBOrdersServices.GetOrdersCountAsync());
        })
        .WithName("GetOrderCount");

        group.MapGet("/", async (HttpContext httpContext) =>
        {
            var orderDtos = await DBOrdersServices.GetOrders();
            var orderDto = orderDtos.LastOrDefault();
            if (orderDto != null)
            {
                return Results.Ok(orderDto);
            }
            else
            {
                return Results.NotFound();
            }
        })
        .WithName("GetAllOrders");

        group.MapGet("/{id}", async Task<Results<Ok<OrderDto>, NotFound>> ([FromRoute] int id) =>
        {
            var orderDto = await DBOrdersServices.GetOrdersByUsingId(id);
            if (orderDto == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(orderDto);
        })
        .WithName("GetOrderById");

    }
}
