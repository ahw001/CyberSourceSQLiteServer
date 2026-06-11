using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using CybsClass.EntityModels;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.WebApi.Service.Services.DBOperations;
namespace CybsClass.WebApi.Service;

public static class IndividualTransactionEndpoints
{
    public static void MapIndividualTransactionEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/IndividualTransaction").WithTags(nameof(IndividualTransaction));

        group.MapGet("/count", async () =>
        {
            return Results.Ok(await DBIndividualTransactionServices.GetIndividualTransactionCountAsync());
        })
        .WithName("GetIndividualTransactionCount");

        group.MapGet("/", async (HttpContext httpContext) =>
        {
            var individualTransactionDtos = await DBIndividualTransactionServices.GetIndividualTransactions();
            var individualTransactionDto = individualTransactionDtos.LastOrDefault();
            if (individualTransactionDto != null)
            {
                return Results.Ok(individualTransactionDtos);
            }
            else
            {
                return Results.NotFound();
            }
        })
        .WithName("GetAllIndividualTransactions");

        group.MapGet("/{id}", async Task<Results<Ok<IndividualTransactionDto>, NotFound>> (int id) =>
        {
            var individualTransactionDto = await DBIndividualTransactionServices.GetIndividualTransactionByUsingId(id);
            if (individualTransactionDto == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(individualTransactionDto);
        })
        .WithName("GetIndividualTransactionById");

    }
}
