using Microsoft.AspNetCore.Http.HttpResults;
using CybsClass.EntityModels;
using CybsClass.WebApi.Service.Services.DBOperations;

namespace CybsClass.WebApi.Service;

public static class B2cCustomers
{
    public static void MapB2cCustomerEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/B2cCustomer").WithTags(nameof(B2cCustomer));

        group.MapGet("/", async () =>
        {
            return await DBCustomerServices.GetB2CCustomers();
        })
        .WithName("GetAllB2cCustomers");

        group.MapGet("/paging/{pageIndex}/{pageSize}", async (int pageIndex, int pageSize) =>
        {
            return await DBCustomerServices.GetB2cCustomerPagedAsync(pageIndex, pageSize);
        })
        .WithName("GetPagingCustomers");

        group.MapGet("/{id}", async Task<Results<Ok<B2cCustomer>, NotFound>> (int b2ccustomerid) =>
        {
            var customer = await DBCustomerServices.GetB2cCustomerByIdAsync(b2ccustomerid);
            return customer is not null ? TypedResults.Ok(customer) : TypedResults.NotFound();
        })
        .WithName("GetB2cCustomerById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int b2ccustomerid, B2cCustomer b2cCustomer) =>
        {
            var affected = await DBCustomerServices.UpdateB2cCustomer(b2ccustomerid, b2cCustomer);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateB2cCustomer");

        group.MapPost("/", async (B2cCustomer b2cCustomer) =>
        {
            var created = await DBCustomerServices.CreateB2cCustomerSimple(b2cCustomer);
            if (created is null)
            {
                return Results.Problem("Failed to create customer.");
            }
            return Results.Created($"/api/B2cCustomer/{created.B2cCustomerId}", created);
        })
        .WithName("CreateB2cCustomer");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int b2ccustomerid) =>
        {
            var affected = await DBCustomerServices.DeleteB2cCustomer(b2ccustomerid);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteB2cCustomer");
    }
}
