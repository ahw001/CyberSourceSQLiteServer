using Microsoft.AspNetCore.Http.HttpResults;
using CybsClass.EntityModels;
using Microsoft.AspNetCore.Mvc;
using CybsClass.WebApi.Service.Services.DBOperations;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Mappers;
namespace CybsClass.WebApi.Service;

public static class PaymentCardInfoEndpoints
{

    public static void MapPaymentCardInfoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/PaymentCardInfo").WithTags(nameof(PaymentCardInfo));

        group.MapGet("/count", async () =>
        {
            return Results.Ok(await DBPaymentCardServices.GetPaymentCardCountAsync());
        })
        .WithName("GetPaymentCardCount");

        group.MapGet("/", async () =>
        {
            return await DBPaymentCardServices.GetAllPaymentCardInfoEntities();
        })
        .WithName("GetAllPaymentCardInfos");

        group.MapGet("/{paymentcardid:int}", async Task<Results<Ok<PaymentCardDto>, NotFound>> ([FromRoute] int paymentcardid) =>
        {
            var paymentCardDto = await DBPaymentCardServices.GetPaymentCardInfoByUsingId(paymentcardid);
            if (paymentCardDto == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(paymentCardDto);
        })
        .WithName("GetPaymentCardInfoByUsingId");


        group.MapGet("/customer/{b2ccustomerid:int}", async ([FromRoute] int b2ccustomerid) =>
        {
            var paymentCardInfos = await DBCustomerServices.GetB2CCustomerPaymentCards(b2ccustomerid);

            if (paymentCardInfos == null || !paymentCardInfos.Any())
            {
                return Results.NotFound();
            }
            List<PaymentCardDto> paymentCardDtos = new List<PaymentCardDto>();
            paymentCardDtos = PaymentCardMapper.Map(paymentCardInfos);
            return Results.Ok(paymentCardDtos);
        })
        .WithName("GetPaymentCardsByCustomer");


        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> ([FromRoute] int id, PaymentCardDto paymentCardDto) =>
        {
            var affected = await DBPaymentCardServices.UpdatePaymentCardInfo(id, paymentCardDto);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePaymentCardInfo");

        group.MapPost("/", async (PaymentCardDto paymentCardDto) =>
        {
            var dbResults = await DBPaymentCardServices.CreatePaymentCardInfo(paymentCardDto);
            return Results.Created("New PaymentCard ID:", dbResults.LastOrDefault().Value);
        })
        .WithName("CreatePaymentCardInfo");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int paymentcardid) =>
        {
            var affected = await DBPaymentCardServices.DeletePaymentCardInfo(paymentcardid);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePaymentCardInfo");
    }
}
