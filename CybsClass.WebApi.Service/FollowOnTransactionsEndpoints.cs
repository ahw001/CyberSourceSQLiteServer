using Microsoft.AspNetCore.Http.HttpResults;
using CybsClass.EntityModels;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.WebApi.Service.Services.DBOperations;
using Microsoft.AspNetCore.Mvc;
namespace CybsClass.WebApi.Service;

public static class FollowOnTransactionsEndpoints
{
    public static void MapFollowOnTransResponseEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/FollowOnTransResponse").WithTags(nameof(FollowOnTransResponse));

        group.MapGet("/count", async () =>
        {
            return Results.Ok(await DBFollowOnTransResponseServices.GetFollowOnTransResponseCountAsync());
        })
        .WithName("GetFollowOnTransCount");

        group.MapGet("/", async (HttpContext httpContext) =>
        {
            var followOnTransResponseDtos = await DBFollowOnTransResponseServices.GetFollowOnTransResponses();
            var followOnTransResponseDto = followOnTransResponseDtos.LastOrDefault();
            if (followOnTransResponseDto != null)
            {
                return Results.Ok(followOnTransResponseDto);
            }
            else
            {
                return Results.NotFound();
            }
        })
        .WithName("GetAllFollowOnTransactions");

        group.MapGet("/{id}", async Task<Results<Ok<FollowOnTransResponseDto>, NotFound>> ([FromRoute] int id) =>
        {
            var followOnTransResponseDto = await DBFollowOnTransResponseServices.GetFollowOnTransResponseByUsingId(id);
            if (followOnTransResponseDto == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(followOnTransResponseDto);
        })
        .WithName("GetFollowOnTransResponseById");

    }
}
