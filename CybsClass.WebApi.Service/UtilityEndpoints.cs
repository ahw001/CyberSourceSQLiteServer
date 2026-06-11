using Microsoft.AspNetCore.Mvc;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.WebApi.Service.Services.Utilities;
using System.Text.Json.Nodes;

namespace CybsClass.WebApi.Service;

public static class UtilityEndpoints
{
    public static RouteGroupBuilder UtilityGroupEndPoints(this RouteGroupBuilder group)
    {
        group.MapPost("/processor", async ([FromBody] SimpleJsonProcessorDto simpleJsonProcessorDto) =>
        {
            JsonObject jsonObject = new JsonObject();

            var inboundJson = System.Text.Json.JsonSerializer.Serialize(simpleJsonProcessorDto);
            Console.WriteLine($"\n[/api/json/processor] INBOUND: {inboundJson}");

            if (simpleJsonProcessorDto == null || simpleJsonProcessorDto.Value == null)
            {
                return Results.NotFound();
            }

            jsonObject = await SimpleJsonProcessor.CallForSimpleJson(simpleJsonProcessorDto);

            var outboundJson = System.Text.Json.JsonSerializer.Serialize(jsonObject, new System.Text.Json.JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            Console.WriteLine($"\n[/api/json/processor] OUTBOUND: {outboundJson}");

            if (jsonObject == null)
            {
                return Results.NotFound();
            }
            else
            {
                return Results.Ok(jsonObject);
            }

        }).Produces<JsonObject>().WithName("JsonProcessor");

        return group;
    }
}


