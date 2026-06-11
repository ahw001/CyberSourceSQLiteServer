using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using CybsClass.EntityModels;
using CybsClass.WebApi.Service.Services.DBOperations;

namespace CybsClass.WebApi.Service;

public static class MerchantSampleDatumEndpoints
{
    public static void MapMerchantSampleDatumEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/merchantsampledatum").WithTags(nameof(MerchantSampleDatum));

        group.MapGet("/", async () =>
        {
            var merchant = await DBMerchantSampleDatumServices.GetRandomMerchantSampleDatum();
            if (merchant == null) return Results.NotFound();
            return Results.Json(merchant);
        })
        .WithName("GetRandomMerchant")
        .Produces<MerchantSampleDatum>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{samplemerchantid}", async Task<Results<Ok<MerchantSampleDatum>, NotFound>> (int samplemerchantid) =>
        {
            var merchant = await DBMerchantSampleDatumServices.GetMerchantSampleDatumById(samplemerchantid);
            return merchant is not null ? TypedResults.Ok(merchant) : TypedResults.NotFound();
        })
        .WithName("GetMerchantSampleDatumById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int samplemerchantid, MerchantSampleDatum merchantSampleDatum) =>
        {
            var affected = await DBMerchantSampleDatumServices.UpdateMerchantSampleDatum(samplemerchantid, merchantSampleDatum);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateMerchantSampleDatum");

        group.MapPost("/", async (MerchantSampleDatum merchantSampleDatum) =>
        {
            var created = await DBMerchantSampleDatumServices.CreateMerchantSampleDatum(merchantSampleDatum);
            if (created is null)
            {
                return Results.Problem("Failed to create merchant sample datum.");
            }
            return Results.Created($"/api/MerchantSampleDatum/{created.SampleMerchantId}", created);
        })
        .WithName("CreateMerchantSampleDatum");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int samplemerchantid) =>
        {
            var affected = await DBMerchantSampleDatumServices.DeleteMerchantSampleDatum(samplemerchantid);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteMerchantSampleDatum");
    }
}
