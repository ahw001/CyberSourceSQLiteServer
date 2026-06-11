using Microsoft.AspNetCore.Mvc;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.WebApi.Service.Services.PayPal;
using System.Text.Json.Nodes;

namespace CybsClass.WebApi.Service;

public static class PayPalEndpoints
{
    public static RouteGroupBuilder GroupPayPalEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/createorder", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            try
            {
                var result = await CallForCybsPayPalCreateOrder.RunAsyncJsonObject(b2cCustomerDto);
                if (result is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }).Produces<JsonObject>().WithName("CreatePayPalOrder");

        return group;
    }
}
