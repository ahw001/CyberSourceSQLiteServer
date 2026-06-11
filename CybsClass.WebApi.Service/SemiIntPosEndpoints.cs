using Microsoft.AspNetCore.Mvc;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.WebApi.Service.Services.WssPosProcessing;
using System.Text.Json.Nodes;

namespace CybsClass.WebApi.Service;

public static class SemiIntPosEndpoints
{
    public static RouteGroupBuilder SemiIntGroupPosEndPoints(this RouteGroupBuilder group)
    {
        group.MapPost("/setup", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            if (b2cCustomerDto == null || b2cCustomerDto.PosActivationCode == null)
            {
                return Results.NotFound();
            }

            string activationResponse = await WssPosActivate.CallActivation(b2cCustomerDto);

            if (activationResponse == null)
            {
                return Results.NotFound();
            }
            else
            {
                return Results.Ok(activationResponse);
            }

        }).Produces<string>().WithName("WssActivation");

        group.MapPost("/sale", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            try
            {
                var posSale = await WssPosSaleService.CallForPosSale(b2cCustomerDto);
                if (posSale == null)
                {
                    return Results.NotFound();
                }
                else
                {
                    return Results.Ok(posSale);
                }

            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

        }).Produces<JsonObject>().WithName("WssPerformPosSale");

        group.MapPost("/simplesale", async () =>
        {
            try
            {
                var posSale = await SimpleWssPosSaleService.SimpleCallForPosSale();
                if (posSale == null)
                {
                    return Results.NotFound();
                }
                else
                {
                    return Results.Ok(posSale);
                }

            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

        }).Produces<JsonObject>().WithName("SimpleWssPerformPosSale");

        group.MapPost("/linkedreturn", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            try
            {
                var posReturn = await WssPosReturnService.CallForPosReturn(b2cCustomerDto);
                if (posReturn == null)
                {
                    return Results.NotFound();
                }
                else
                {
                    return Results.Ok(posReturn);
                }

            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

        }).Produces<JsonObject>().WithName("WssPerformPosLinkedReturn");

        group.MapPost("/followon", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            try
            {
                var posFollowOn = await WssPosFollowOn.CallForFollowOn(b2cCustomerDto);
                if (posFollowOn == null)
                {
                    return Results.NotFound();
                }
                else
                {
                    return Results.Ok(posFollowOn);
                }

            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

        }).Produces<JsonObject>().WithName("WssPerformPosFollowOn");

        return group;
    }
}


