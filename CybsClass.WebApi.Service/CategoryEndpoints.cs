using Microsoft.AspNetCore.Http.HttpResults;
using CybsClass.EntityModels;
using CybsClass.WebApi.Service.Services.DBOperations;

namespace CybsClass.WebApi.Service;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Category").WithTags(nameof(Category));

        group.MapGet("/", async () =>
        {
            return await DBCategoryServices.GetAllCategories();
        })
        .WithName("GetAllCategories");

        group.MapGet("/{id}", async Task<Results<Ok<Category>, NotFound>> (int categoryid) =>
        {
            var category = await DBCategoryServices.GetCategoryById(categoryid);
            return category is not null ? TypedResults.Ok(category) : TypedResults.NotFound();
        })
        .WithName("GetCategoryById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int categoryid, Category category) =>
        {
            var affected = await DBCategoryServices.UpdateCategory(categoryid, category);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCategory");

        group.MapPost("/", async (Category category) =>
        {
            var created = await DBCategoryServices.CreateCategory(category);
            if (created is null)
            {
                return Results.Problem("Failed to create category.");
            }
            return Results.Created($"/api/Category/{created.CategoryId}", created);
        })
        .WithName("CreateCategory");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int categoryid) =>
        {
            var affected = await DBCategoryServices.DeleteCategory(categoryid);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCategory");
    }
}
