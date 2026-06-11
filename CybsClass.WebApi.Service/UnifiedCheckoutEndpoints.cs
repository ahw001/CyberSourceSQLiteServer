using Microsoft.AspNetCore.Mvc;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;
using CybsClass.WebApi.Service.Services.CaptureContextProcessing;
using CybsClass.WebApi.Service.Services.DBOperations;
using CybsClass.WebApi.Service.Services.TokenProcessing;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CybsClass.WebApi.Service
{
    public static class UnifiedCheckoutEndpoints
    {
        public static Dictionary<string, string> dbResults = new Dictionary<string, string>();

        public static RouteGroupBuilder GroupUnifiedEndpoints(this RouteGroupBuilder group)
        {

            group.MapPost("/unifiedpayment", async (B2cNorthwindContext db, [FromBody] CtxPaymentDto ctxPaymentDto) =>
            {
                Dictionary<string, string> dbResults = new();
                JsonObject paymentResponse = await CallCybsCtxPayment.RunAsyncCtxBlobObject(ctxPaymentDto);

                bool isErrorResponse = paymentResponse.TryGetPropertyValue("status", out var statusNode) &&
                                       statusNode?.ToString()?.ToUpperInvariant() == "ERROR";

                if (!isErrorResponse)
                {
                    try
                    {
                        dbResults = await PersistUnifiedAuth.UnifiedAuthDBOps(ctxPaymentDto, paymentResponse);

                        var errorObject = new CybsClass.Cybersource.Models.BaseData.ErrorObject();

                        foreach (var result in dbResults)
                        {
                            var key = result.Key.ToLowerInvariant();
                            var value = result.Value;

                            if (key.Contains("error"))
                                errorObject.Error = value;
                            else if (key.Contains("message"))
                                errorObject.Message = value;
                            else if (key.Contains("reason"))
                                errorObject.Reason = value;
                            else if (key.Contains("action"))
                                errorObject.Action = value;
                            else if (key.Contains("transactionjson"))
                                errorObject.TransactionJson = value;
                        }

                        if (errorObject.Error != null || errorObject.Message != null || errorObject.Reason != null ||
                            errorObject.Action != null || errorObject.TransactionJson != null)
                        {
                            paymentResponse["dbError"] = JsonSerializer.SerializeToNode(errorObject)!;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
                else
                {
                    // Persist the error response as an ErrorObject
                    var errorObject = new CybsClass.EntityModels.ErrorObject
                    {
                        Error = paymentResponse["error"]?.ToString(),
                        Message = paymentResponse["message"]?.ToString(),
                        Reason = paymentResponse["reason"]?.ToString(),
                        Action = paymentResponse["action"]?.ToString(),
                        TransactionJson = paymentResponse.ToJsonString()
                    };

                    db.ErrorObjects.Add(errorObject);
                    await db.SaveChangesAsync();
                }

                return Results.Ok(paymentResponse);

            }).Produces<JsonObject>().WithName("PerformPaymentWithTransToken");



            group.MapPost("/transtokeninfo", async ([FromBody] CtxPaymentDto ctxPaymentDto) =>
            {
                if (ctxPaymentDto is not null && ctxPaymentDto.TokenInformation is not null && ctxPaymentDto.TokenInformation.TransientTokenJwt is not null)
                {
                    string jwtCtx = ctxPaymentDto.TokenInformation.TransientTokenJwt;
                    var paymentResponse = await CallTransTokenInfo.RunAsyncTransTokenInfo(jwtCtx);
                    return Results.Ok(paymentResponse);
                }
                else
                {
                    string paymentResponse = "No JWT found.";
                    return Results.Ok(paymentResponse);
                }

            }).Produces<JsonObject>().WithName("GetTransTokenInfo");

            return group;
        }
    }
}
