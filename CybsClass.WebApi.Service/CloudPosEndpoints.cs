using Microsoft.AspNetCore.Mvc;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using CybsClass.WebApi.Service.Services.DBOperations;
using CybsClass.WebApi.Service.Services.CloudPosTransactionProcessing;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CybsClass.WebApi.Service;

public static class CloudPosEndpoints
{
    public static RouteGroupBuilder GroupCloudPosEndPoints(this RouteGroupBuilder group)
    {
        group.MapPost("/bearer", async () =>
        {
            var bearerTokenDto = new BearerTokenDto();

            var bearerJson = await CloudPosBearerCreate.CallForBearerToken();

            using JsonDocument doc = JsonDocument.Parse(bearerJson.ToString());
            JsonElement root = doc.RootElement;

            if (root.TryGetProperty("token", out JsonElement tokenProp) && tokenProp.ValueKind == JsonValueKind.String)
            {
                bearerTokenDto.BearerToken = tokenProp.GetString()!;
                return Results.Ok(bearerTokenDto);
            }
            else if (root.TryGetProperty("errors", out JsonElement errorsProp) && errorsProp.ValueKind == JsonValueKind.Array)
            {
                string errorMessage = errorsProp[0].GetString() ?? "Unknown error.";
                return Results.BadRequest(errorMessage);
            }
            else
            {
                return Results.BadRequest("Unexpected response format.");
            }
        })
        .Produces<BearerTokenDto>()
        .Produces<string>(StatusCodes.Status400BadRequest)
        .WithName("CreateBearerToken");


        group.MapPost("/sale", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            try 
            { 
                var posSale = await CloudPosSaleService.CallForPosSale(b2cCustomerDto);
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

        }).Produces<JsonObject>().WithName("PerformCloudSale");

        group.MapPost("/linkedreturn", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            try
            {
                var posReturn = await CloudPosReturnService.CallForPosReturn(b2cCustomerDto);
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

        }).Produces<JsonObject>().WithName("PerformCloudPosLinkedReturn");

        group.MapPost("/followon", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            try
            {
                var posFollowOn = await CloudPosFollowOn.CallForFollowOn(b2cCustomerDto);
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

        }).Produces<JsonObject>().WithName("PerformCloudPosFollowOn");

        return group;
    }
}


