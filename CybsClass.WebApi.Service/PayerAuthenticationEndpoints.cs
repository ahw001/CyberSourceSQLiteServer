using Microsoft.AspNetCore.Mvc;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.WebApi.Service.Services.PayerAuthenticationProcessing;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CybsClass.WebApi.Service
{
    public static class PayerAuthenticationEndpoints
    {
        public static Dictionary<string, string> dbResults = new Dictionary<string, string>();

        public static RouteGroupBuilder GroupPayerAuthEndpoints(this RouteGroupBuilder group)
        {

            group.MapPost("/payerauthsetup", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
            {

                var paymentResponse = await CallCybsPASetup.RunAsyncJsonObject(b2cCustomerDto);

                return Results.Ok(paymentResponse);

            }).Produces<JsonObject>().WithName("PayerAuthSetup");

            group.MapPost("/flexpayerauthsetup", async ([FromBody] CaptureContextDto captureContextDto) =>
            {

                var paymentResponse = await CallCybsPASetup.RunAsyncFlexJsonObject(captureContextDto);

                return Results.Ok(paymentResponse);

            }).Produces<JsonObject>().WithName("FlexPayerAuthSetup");

            group.MapPost("/flexpacheckenroll", async ([FromBody] CheckEnrollDto checkEnrollDto) =>
            {

                var checkEnrollResponse = await CallCybsCheckEnroll.RunAsyncFlexJsonObject(checkEnrollDto);

                return Results.Ok(checkEnrollResponse);

            }).Produces<JsonObject>().WithName("FlexPaCheckEnrollment");

            group.MapPost("/flexaftpacheckenroll", async ([FromBody] AftCheckEnrollDto aftCheckEnrollDto) =>
            {

                var checkEnrollResponse = await CallCybsCheckEnroll.RunAsyncFlexAftJsonPaymentObject(aftCheckEnrollDto);

                FollowOnTransResponseDto followOnTransResponseDto = new FollowOnTransResponseDto();

                if (checkEnrollResponse != null)
                {
                    followOnTransResponseDto = JsonSerializer.Deserialize<FollowOnTransResponseDto>(checkEnrollResponse.ToJsonString())!;
                }

                return Results.Ok(followOnTransResponseDto);

            }).Produces<JsonObject>().WithName("FlexAftPaCheckEnrollment");

            group.MapPost("/flexaftpavalidate", async ([FromBody] AftValidateDto aftValidateDto) =>
            {

                var validateResponse = await CallCybsValidate.RunAsyncFlexJsonObject(aftValidateDto);

                FollowOnTransResponseDto followOnTransResponseDto = new FollowOnTransResponseDto();

                if (validateResponse != null)
                {
                    followOnTransResponseDto = JsonSerializer.Deserialize<FollowOnTransResponseDto>(validateResponse.ToJsonString())!;
                }

                return Results.Ok(followOnTransResponseDto);

            }).Produces<JsonObject>().WithName("FlexAftPaValidate");

            group.MapPost("/flexaftpavalidateauth", async ([FromBody] AftValidateDto aftValidateDto) =>
            {

                var validateAuthResponse = await CallCybsValidateWithAuth.RunAsyncFlexJsonObject(aftValidateDto);

                FollowOnTransResponseDto followOnTransResponseDto = new FollowOnTransResponseDto();

                if (validateAuthResponse != null)
                {
                    followOnTransResponseDto = JsonSerializer.Deserialize<FollowOnTransResponseDto>(validateAuthResponse.ToJsonString())!;
                }

                return Results.Ok(followOnTransResponseDto);

            }).Produces<JsonObject>().WithName("FlexAftAuthPaValidate");


            return group;
        }
    }
}
