using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using CybsClass.Blazor.Model.Cybersource.NT;
using CybsClass.Cybersource.Models;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Json;
using CybsClass.EntityModels;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public static class PersistFollowOnTransaction
    {
        private static Dictionary<string, object> dbResults = new Dictionary<string, object>();
        public static async Task<Dictionary<string, object>> InsertFollowOnTransaction(JsonNode jsonNode, FollowOnTransJson followOnTransJson, FollowOnTransDto followOnTransDto)
        {
            dbResults = new();

            try
            { 
                Console.WriteLine("Inserting follow on transaction ...");

                int transActionType = (int)followOnTransDto.FollowOnTransaction.GetValueOrDefault();

                FollowOnTransactions folloOnTransValue = (FollowOnTransactions)transActionType; // Cast the int to the Color enum

                Console.WriteLine($" -------- followOnTransDto.OriginalTransactionId: {followOnTransDto.OriginalTransactionId}");

                if (followOnTransDto.TransactionOrderId == "standalone")
                {
                    followOnTransDto.TransactionOrderId = "0";
                }

                await Console.Out.WriteLineAsync("...");

                using B2cNorthwindContext db = new();

                FollowOnTransResponse followOnTransResponse = new FollowOnTransResponse();

                followOnTransResponse.FollowOnTransResponseId = followOnTransJson.Id!;
                followOnTransResponse.OriginalTransactionId = followOnTransDto.OriginalTransactionId!;
                followOnTransResponse.OrderId = Convert.ToInt32(followOnTransDto.TransactionOrderId!);
                followOnTransResponse.TransactionType = folloOnTransValue.ToString();
                followOnTransResponse.ResponseTransactionJson = jsonNode.ToJsonString();

                EntityEntry<FollowOnTransResponse> entity = db.FollowOnTransResponses.Add(followOnTransResponse);
                Console.WriteLine($"FollowOnTransResponse State: {entity.State}, FollowOnTransResponseId: {followOnTransResponse.FollowOnTransResponseId}");

                int affected0 = await db.SaveChangesAsync();
                Console.WriteLine($"FollowOnTransResponse State: {entity.State}, FollowOnTransResponseId: {followOnTransResponse.FollowOnTransResponseId}");
                dbResults.Add("FollowOnTransResponseId", followOnTransResponse.FollowOnTransResponseId);

                return dbResults;
            }
            catch (Exception ex)
            {
                dbResults = new();
                dbResults.Add("Exception", ex.Message);
                Console.WriteLine($"Exception: {ex.Message}");
                return dbResults;
            }
        }
    }

}

