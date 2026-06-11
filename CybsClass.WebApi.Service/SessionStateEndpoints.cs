using Microsoft.AspNetCore.Mvc;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.WebApi.Service.Services.DBOperations;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CybsClass.WebApi.Service
{
    public static class SessionStateEndpoints
    {
        public static Dictionary<string, string> dbResults = new Dictionary<string, string>();

        public static RouteGroupBuilder GroupSessionState(this RouteGroupBuilder group)
        {

            group.MapPost("/sessionstore", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
            {
                Dictionary<string, string> dbResults = new();
                JsonObject sessionStoreCreate = new JsonObject();

                try
                {
                    dbResults = await PersistSessionState.SessionStateDBOps(b2cCustomerDto);

                    var errorObject = new CybsClass.Cybersource.Models.BaseData.ErrorObject();
                    string id = string.Empty;

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
                        else if (key.Contains("guid"))
                        { 
                            id = value;
                            sessionStoreCreate["guid"] = id;
                        }

                    }

                    if (errorObject.Error != null || errorObject.Message != null || errorObject.Reason != null ||
                        errorObject.Action != null || errorObject.TransactionJson != null)
                    {
                        sessionStoreCreate["errorObject"] = JsonSerializer.SerializeToNode(errorObject)!;
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }

                return Results.Ok(sessionStoreCreate);

            }).Produces<JsonObject>().WithName("StoreSessionState");



            group.MapGet("/sessionretrieve/{id:guid}", async (Guid id) =>
            {
                var sessionStateDto = await PersistSessionState.GetSessionStateByIdAsync(id);
                if (sessionStateDto == null) return Results.NotFound();
                return Results.Json(sessionStateDto);
            }).Produces<JsonObject>().WithName("GetSessionState");

            return group;
        }
    }
}

