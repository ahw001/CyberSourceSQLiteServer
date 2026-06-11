using Microsoft.AspNetCore.Http.HttpResults;
using CybsClass.EntityModels;
using CybsClass.WebApi.Service.Services.DBOperations;

namespace CybsClass.WebApi.Service;

public static class PayerAuthCardSampleDatumEndpoints
{
    public static void MapPayerAuthCardSampleDatumEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/PayerAuthCardSampleDatum").WithTags(nameof(PayerAuthCardSampleDatum));

        group.MapGet("/", async () =>
        {
            return await DBPayerAuthCardSampleDatumServices.GetAllPayerAuthCardSampleData();
        })
        .WithName("GetAllPayerAuthCardSampleData");

        group.MapGet("/{id}", async Task<Results<Ok<PayerAuthCardSampleDatum>, NotFound>> (int samplepayauthpaymentcardid) =>
        {
            var datum = await DBPayerAuthCardSampleDatumServices.GetPayerAuthCardSampleDatumById(samplepayauthpaymentcardid);
            return datum is not null ? TypedResults.Ok(datum) : TypedResults.NotFound();
        })
        .WithName("GetPayerAuthCardSampleDatumById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int samplepayauthpaymentcardid, PayerAuthCardSampleDatum payerAuthCardSampleDatum) =>
        {
            var affected = await DBPayerAuthCardSampleDatumServices.UpdatePayerAuthCardSampleDatum(samplepayauthpaymentcardid, payerAuthCardSampleDatum);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePayerAuthCardSampleDatum");

        group.MapPost("/", async (PayerAuthCardSampleDatum payerAuthCardSampleDatum) =>
        {
            var created = await DBPayerAuthCardSampleDatumServices.CreatePayerAuthCardSampleDatum(payerAuthCardSampleDatum);
            if (created is null)
            {
                return Results.Problem("Failed to create payer auth card sample datum.");
            }
            return Results.Created($"/api/PayerAuthCardSampleDatum/{created.SamplePayAuthPaymentCardId}", created);
        })
        .WithName("CreatePayerAuthCardSampleDatum");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int samplepayauthpaymentcardid) =>
        {
            var affected = await DBPayerAuthCardSampleDatumServices.DeletePayerAuthCardSampleDatum(samplepayauthpaymentcardid);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePayerAuthCardSampleDatum");
    }
}
