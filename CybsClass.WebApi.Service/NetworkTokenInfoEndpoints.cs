using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using CybsClass.EntityModels;
using CybsClass.WebApi.Service.Services.DBOperations;
using Microsoft.AspNetCore.Mvc;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Mappers;
namespace CybsClass.WebApi.Service;

public static class NetworkTokenInfoEndpoints
{
    public static void MapNetworkTokenInfoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/NetworkTokenInfo").WithTags(nameof(NetworkTokenInfo));

        group.MapGet("/count", async () =>
        {
            return Results.Ok(await DBNetworkTokenServices.GetNetworkTokenInfosCountAsync());
        })
        .WithName("GetNetowrkTokenCount");

        group.MapGet("/", async (HttpContext httpContext) =>
        {
            var networkTokenInfoDtos = await DBNetworkTokenServices.GetNetworkTokens();
            var networkTokenInfoDto = networkTokenInfoDtos.LastOrDefault();
            if (networkTokenInfoDto != null)
            {
                return Results.Ok(networkTokenInfoDto);
            }
            else
            {
                return Results.NotFound();
            }
        })
        .WithName("GetAllNetworkTokenInfos");

        group.MapGet("/{id}", async Task<Results<Ok<List<NetworkTokenInfoDto>>, NotFound>> ([FromRoute] int id) =>
        {
            var networkTokenInfoDtos = await DBNetworkTokenServices.GetNetworkTokenByUsingId(id);
            if (networkTokenInfoDtos == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(networkTokenInfoDtos)!;
        })
        .WithName("GetNetworkTokenInfoById");
    }
}
