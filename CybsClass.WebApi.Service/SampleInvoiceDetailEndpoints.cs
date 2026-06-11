using Microsoft.AspNetCore.Http.HttpResults;
using CybsClass.EntityModels;
using CybsClass.WebApi.Service.Services.DBOperations;

namespace CybsClass.WebApi.Service;

public static class SampleInvoiceDetailEndpoints
{
    public static void MapSampleInvoiceDetailEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/SampleInvoiceDetail").WithTags(nameof(SampleInvoiceDetail));

        group.MapGet("/", async () =>
        {
            return await DBSampleInvoiceDetailServices.GetAllSampleInvoiceDetails();
        })
        .WithName("GetAllSampleInvoiceDetails");

        group.MapGet("/{id}", async Task<Results<Ok<SampleInvoiceDetail>, NotFound>> (int sampleinvoiceid) =>
        {
            var detail = await DBSampleInvoiceDetailServices.GetSampleInvoiceDetailById(sampleinvoiceid);
            return detail is not null ? TypedResults.Ok(detail) : TypedResults.NotFound();
        })
        .WithName("GetSampleInvoiceDetailById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int sampleinvoiceid, SampleInvoiceDetail sampleInvoiceDetail) =>
        {
            var affected = await DBSampleInvoiceDetailServices.UpdateSampleInvoiceDetail(sampleinvoiceid, sampleInvoiceDetail);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateSampleInvoiceDetail");

        group.MapPost("/", async (SampleInvoiceDetail sampleInvoiceDetail) =>
        {
            var created = await DBSampleInvoiceDetailServices.CreateSampleInvoiceDetail(sampleInvoiceDetail);
            if (created is null)
            {
                return Results.Problem("Failed to create sample invoice detail.");
            }
            return Results.Created($"/api/SampleInvoiceDetail/{created.SampleInvoiceId}", created);
        })
        .WithName("CreateSampleInvoiceDetail");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int sampleinvoiceid) =>
        {
            var affected = await DBSampleInvoiceDetailServices.DeleteSampleInvoiceDetail(sampleinvoiceid);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteSampleInvoiceDetail");
    }
}
