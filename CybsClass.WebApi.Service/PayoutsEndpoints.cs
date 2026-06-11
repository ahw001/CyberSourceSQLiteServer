using Microsoft.AspNetCore.Mvc;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.WebApi.Service.Services.CaptureContextProcessing;
using CybsClass.WebApi.Service.Services.AftProcessing;
using System.Text.Json.Nodes;

namespace CybsClass.WebApi.Service;

public static class PayoutsEndpoints
{
    public static RouteGroupBuilder GroupPayoutsEndPoints(this RouteGroupBuilder group)
    {
        group.MapPost("/sendaft", async ([FromBody] AftRequestDto aftRequestDto) =>
        {
            try
            {
                var aftTrans = await CallForAft.RunAsyncJsonObject(aftRequestDto);
                if (aftTrans == null)
                {
                    return Results.NotFound();
                }
                else
                {
                    return Results.Ok(aftTrans);
                }

            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

        }).Produces<JsonObject>().WithName("SendAft");

        group.MapPost("/sendaftflex", async ([FromBody] AftRequestDto aftRequestDto) =>
        {
            JsonObject aftCtxPayment = await CallAftCybsCtxPayment.RunAsyncCtxBlobObject(aftRequestDto);

            if (aftCtxPayment == null)
            {
                return Results.NotFound();
            }
            else
            {
                return Results.Ok(aftCtxPayment);
            }

        }).Produces<string>().WithName("SendAftFlexTransaction");

        return group;
    }
}
