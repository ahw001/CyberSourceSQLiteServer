using Microsoft.AspNetCore.Mvc;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;
using CybsClass.WebApi.Service.Services.CcTransatcionProcessing;
using CybsClass.WebApi.Service.Services.DBOperations;
using CybsClass.WebApi.Service.Services.FlexUcContextProcessing;
using CybsClass.WebApi.Service.Services.TokenProcessing;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CybsClass.WebApi.Service;

public static class TokenEndpoints
{
    public static RouteGroupBuilder GroupTokens(this RouteGroupBuilder group)
    {
        group.MapPost("/retrieval", async ([FromBody] FollowOnTransDto followOnTransDto) =>
        {
            var tokens = await CallForCybsTms.TokenRetrievals(followOnTransDto);
            return Results.Ok(tokens);
        }).Produces<JsonObject>().WithName("PerformTokenRetrievals");

        group.MapPost("/capturecontext", static async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            Dictionary<string, string> dbResults = new Dictionary<string, string>();

            CaptureContextDto ctxDto = new CaptureContextDto();

            string ctx = await CallForCaptureContext.RunAsyncCaptureContextCreate(b2cCustomerDto);

            if (ctx == null)
            {
                return Results.NotFound();
            }
            else
            {
                dbResults = await DBCustomerServices.InsertB2CCustomerAsync(b2cCustomerDto);

                if (dbResults is not null && dbResults.Count() > 0)
                {
                    ctxDto.B2cCustomerId = dbResults["B2cCustomerId"];
                    ctxDto.OrderId = dbResults["OrderId"];
                    ctxDto.Ctx = ctx;
                }

            }

            return Results.Ok(ctxDto);
        }).Produces<string>().WithName("CreateCaptureContext");

        group.MapPost("/flexcapturecontext", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            CaptureContextDto ctxDto = new CaptureContextDto();

            string ctx = await CallForFlexCaptureContext.RunAsyncCaptureContextCreate(b2cCustomerDto);

            if (ctx == null)
            {
                return Results.NotFound();
            }
            else
            {
                ctxDto.Ctx = ctx;
            }

            return Results.Ok(ctxDto);
        }).Produces<string>().WithName("CreateFlexCaptureContext");

        group.MapPost("/combined", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            Dictionary<string, string> dbResult = new Dictionary<string, string>();

            try
            {
                var combinedToken = await CallCybsTokenService.CallForCybsCombinedTokenService(b2cCustomerDto);
                if (combinedToken == null)
                {
                    return Results.NotFound();
                }
                else
                {
                    b2cCustomerDto.CustomerInstrumentId = (string)combinedToken!["id"]! ?? "null";
                    if (combinedToken is not null && (!combinedToken.ToString().Contains("error",
                        StringComparison.OrdinalIgnoreCase) &&
                        !combinedToken.ToString().Contains("exception", StringComparison.OrdinalIgnoreCase)))
                    {
                        dbResult = await PersistCybsTokenData.TokenDBOps(b2cCustomerDto.B2cCustomerId, combinedToken);
                    }
                    else
                    {
                        Console.WriteLine("Error: Customer token is null or contains an error or exception. DB RESULTS ARE SKIPPED.");
                    }
                }

                var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(combinedToken, options);

                Console.WriteLine("**************************************************");
                Console.WriteLine($"RESPONSE JSON BEING SENT TO CLIENT:  {jsonString}");

                return Results.Ok(combinedToken);
            }
            catch (Exception e)
            {
                string jsonString = e.Message;
                JsonObject jsonObject = new JsonObject();
                Console.WriteLine(e.Message);
                jsonString = $"{{ \"Exception\": \"{e}\" }}";
                JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
                JsonElement rootElement = jsonDocument.RootElement;
                jsonObject = JsonObject.Create(rootElement)!;
                return Results.Json(jsonObject);
            }

        }).WithName("CreateCustomerToken");

        group.MapPost("/zeroauthtoken", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            Dictionary<string, string> dbResult = new Dictionary<string, string>();

            try
            {
                var zeroAuthToken = await CallForCybsAuthTokenCreate.RunAsyncJsonObject(b2cCustomerDto);
                if (zeroAuthToken == null)
                {
                    return Results.NotFound();
                }
                else
                {
                    b2cCustomerDto.CustomerInstrumentId = (string)zeroAuthToken!["id"]! ?? "null";
                    if (zeroAuthToken is not null && (!zeroAuthToken.ToString().Contains("error",
                        StringComparison.OrdinalIgnoreCase) ||
                        !zeroAuthToken.ToString().Contains("exception", StringComparison.OrdinalIgnoreCase)
                        || !zeroAuthToken.ToString().Contains("INVALID", StringComparison.OrdinalIgnoreCase)
                        || !zeroAuthToken.ToString().Contains("DECLINED", StringComparison.OrdinalIgnoreCase)))
                    {
                        dbResult = await PersistCybsTokenData.TokenDBOps(b2cCustomerDto.B2cCustomerId, zeroAuthToken);
                    }
                    else
                    {
                        Console.WriteLine("Error: Customer token is null or contains an error or exception. DB RESULTS ARE SKIPPED.");
                    }
                }

                var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(zeroAuthToken, options);

                return Results.Ok(zeroAuthToken);
            }
            catch (Exception e)
            {
                string jsonString = e.Message;
                JsonObject jsonObject = new JsonObject();
                Console.WriteLine(e.Message);
                jsonString = $"{{ \"Exception\": \"{e}\" }}";
                JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
                JsonElement rootElement = jsonDocument.RootElement;
                jsonObject = JsonObject.Create(rootElement)!;
                return Results.Json(jsonObject);
            }

        }).WithName("CreateZeroAuthToken");

        group.MapPost("/nettokencount", async ([FromBody] PaymentCardDto paymentCardDto) =>
        {
            Dictionary<string, string> dbResult = new Dictionary<string, string>();

            int paymentCardId = 0;
            paymentCardId = paymentCardDto.PaymentCardId;

            if (paymentCardId > 0)
            {
                try
                {
                    dbResult = await DBCybsTokenServices.GetNetworkTokenCountById(paymentCardId);
                    if (dbResult == null)
                    {
                        return Results.NotFound();
                    }
                    else
                    {
                        var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                        string jsonString = JsonSerializer.Serialize(dbResult, options);

                        return Results.Ok(jsonString);
                    }

                }
                catch (Exception ex)
                {
                    dbResult.Add("Exception: ", ex.Message);
                    return Results.Json(dbResult);
                }
            }
            else
            {
                dbResult.Add("Error", "Payment Card ID is invalid.");
                return Results.Json(dbResult);
            }

        }).WithName("GetNetTokenCountById");

        group.MapPost("/tokenize", async ([FromBody] B2cCustomerDto tokenizeDto) =>
        {
            var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
            Console.WriteLine($"\n[TokenizeEndpoint] INBOUND:\n{JsonSerializer.Serialize(tokenizeDto, options)}");

            try
            {
                var result = await CallForTokenize.RunAsync(tokenizeDto);

                Console.WriteLine($"\n[TokenizeEndpoint] OUTBOUND:\n{JsonSerializer.Serialize(result, options)}");

                if (result is not null && !result.ToString().Contains("error", StringComparison.OrdinalIgnoreCase)
                    && !result.ToString().Contains("Exception", StringComparison.OrdinalIgnoreCase)
                    && tokenizeDto.B2cCustomerId > 0)
                {
                    await PersistTokenizeData.PersistTokenize(tokenizeDto.B2cCustomerId, result);
                }

                return Results.Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                var errorObj = new JsonObject();
                errorObj["Exception"] = e.Message;
                return Results.Ok(errorObj);
            }
        }).WithName("TokenizeCard");

        group.MapGet("/sample-nt-cards", async ([FromServices] B2cNorthwindContext db) =>
        {
            var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
            Console.WriteLine("\n[TokenEndpoints] GET /api/tokens/sample-nt-cards");
            var cards = await DBPaymentCardSampleDataServices.GetNtCardsAsync(db);
            Console.WriteLine($"\n[TokenEndpoints] OUTBOUND: {cards.Count} NT cards returned");
            return Results.Ok(cards);
        }).WithName("GetSampleNtCards");

        group.MapPost("/tokenized-cards", async ([FromBody] TokenizedCardNetworkRequestDto dto) =>
        {
            var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
            Console.WriteLine($"\n[TokenEndpoints] POST /api/tokens/tokenized-cards INBOUND:\n{JsonSerializer.Serialize(dto, options)}");

            if (string.IsNullOrWhiteSpace(dto.CardNumber))
            {
                var errorObj = new JsonObject();
                errorObj["error"] = new JsonObject
                {
                    ["message"] = "CardNumber is required."
                };
                return Results.Ok(errorObj);
            }

            try
            {
                var result = await CallForTokenizedCards.RunAsync(dto);
                Console.WriteLine($"\n[TokenEndpoints] OUTBOUND:\n{JsonSerializer.Serialize(result, options)}");
                return Results.Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                var errorObj = new JsonObject();
                errorObj["Exception"] = e.Message;
                return Results.Ok(errorObj);
            }
        }).WithName("SubmitTokenizedCard");

        group.MapPost("/tokenized-cards-mle", async ([FromBody] TokenizedCardNetworkRequestDto dto) =>
        {
            var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
            Console.WriteLine($"\n[TokenEndpoints] POST /api/tokens/tokenized-cards-mle INBOUND:\n{JsonSerializer.Serialize(dto, options)}");

            if (string.IsNullOrWhiteSpace(dto.CardNumber))
            {
                var errorObj = new JsonObject();
                errorObj["error"] = new JsonObject
                {
                    ["message"] = "CardNumber is required."
                };
                return Results.Ok(errorObj);
            }

            try
            {
                var result = await CallForTokenizedCardsMle.RunAsync(dto);
                Console.WriteLine($"\n[TokenEndpoints] OUTBOUND:\n{JsonSerializer.Serialize(result, options)}");
                return Results.Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                var errorObj = new JsonObject();
                errorObj["Exception"] = e.Message;
                return Results.Ok(errorObj);
            }
        }).WithName("SubmitTokenizedCardMle");

        return group;
    }
}

