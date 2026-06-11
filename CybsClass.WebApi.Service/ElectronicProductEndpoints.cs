using Microsoft.AspNetCore.Http.HttpResults;
using CybsClass.EntityModels;
using CybsClass.WebApi.Service.Services.DBOperations;

namespace CybsClass.WebApi.Service;

public static class ElectronicProductEndpoints
{
    public static void MapElectronicProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ElectronicProduct").WithTags(nameof(ElectronicProduct));

        group.MapGet("/", async () =>
        {
            return await DBElectronicProductServices.GetAllElectronicProducts();
        })
        .WithName("GetAllElectronicProducts");

        group.MapGet("/{id}", async Task<Results<Ok<ElectronicProduct>, NotFound>> (int id) =>
        {
            var product = await DBElectronicProductServices.GetElectronicProductById(id);
            return product is not null ? TypedResults.Ok(product) : TypedResults.NotFound();
        })
        .WithName("GetElectronicProductById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, ElectronicProduct electronicProduct) =>
        {
            var affected = await DBElectronicProductServices.UpdateElectronicProduct(id, electronicProduct);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateElectronicProduct");

        group.MapPost("/", async (ElectronicProduct electronicProduct) =>
        {
            var created = await DBElectronicProductServices.CreateElectronicProduct(electronicProduct);
            if (created is null)
            {
                return Results.Problem("Failed to create electronic product.");
            }
            return Results.Created($"/api/ElectronicProduct/{created.ElectronicProductId}", created);
        })
        .WithName("CreateElectronicProduct");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id) =>
        {
            var affected = await DBElectronicProductServices.DeleteElectronicProduct(id);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteElectronicProduct");
    }
}
