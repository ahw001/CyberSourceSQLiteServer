using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Json;
using CybsClass.EntityModels;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public static class PersistUnifiedAuth
    {
        private static Dictionary<string, string> dbResults = new Dictionary<string, string>();

        private static FollowOnTransJson followOnTransJson = new();

        public static async Task<Dictionary<string, string>> UnifiedAuthDBOps(CtxPaymentDto ctxPaymentDto, JsonObject jsonObject)
        {
            try
            {
                dbResults = new();

                Console.WriteLine("Inserting unified auth payment card data ...");

                followOnTransJson = JsonSerializer.Deserialize<FollowOnTransJson>(jsonObject)!;

                using B2cNorthwindContext db = new();

                var paymentCardInfo = new PaymentCardInfo();

                paymentCardInfo.B2cCustomerId = Convert.ToInt32(ctxPaymentDto.B2cCustomerId);
                paymentCardInfo.InstrumentIdentifierId = followOnTransJson.TokenInformation?.InstrumentIdentifier?.Id!;
                paymentCardInfo.InstrumentIdentifierState = followOnTransJson.TokenInformation?.InstrumentIdentifier?.State!;

                EntityEntry<PaymentCardInfo> entity = db.PaymentCardInfos.Add(paymentCardInfo);
                Console.WriteLine($"PaymentCard State: {entity.State}, PaymentCardId: {paymentCardInfo.PaymentCardId}");

                int affected0 = await db.SaveChangesAsync();
                Console.WriteLine($"PaymentCard State: {entity.State}, PaymentCardId: {paymentCardInfo.PaymentCardId}");
                dbResults.Add("PaymentCardId", paymentCardInfo.PaymentCardId.ToString());

                int? payCardInfo = paymentCardInfo!.PaymentCardId;

                Console.WriteLine("Inserting auth transaction data ...");

                AuthTransResponse authDb = new AuthTransResponse();

                authDb.Status = followOnTransJson.Status;
                authDb.SubmitTimeUtc = followOnTransJson.SubmitTimeUtc!;
                authDb.ReconciliationId = followOnTransJson.ReconciliationId!;

                if (followOnTransJson.OrderInformation is not null && followOnTransJson.OrderInformation?.AmountDetails is not null)
                {
                    authDb.AuthorizedAmount = Convert.ToDecimal(followOnTransJson.OrderInformation?.AmountDetails?.AuthorizedAmount);
                    Console.WriteLine($"authTransResponseJson.OrderInformation.AuthAmount is NOT null: {authDb.AuthorizedAmount}");
                }
                else
                {
                    Console.WriteLine("authTransResponseJson.OrderInformation.AuthAmount is NULL");
                    authDb.AuthorizedAmount = 0.00M;
                }

                authDb.Links = followOnTransJson.Links?.ToString() ?? "0";

                authDb.Id = followOnTransJson.Id ?? "0";
                authDb.OrderId = Convert.ToInt32(ctxPaymentDto.OrderId);
                authDb.ProcInfoApprovalCode = followOnTransJson.ProcessorInformation?.ApprovalCode ?? "0";
                authDb.ProcInfoNetworkTransactionId = followOnTransJson.ProcessorInformation?.NetworkTransactionId ?? "0";
                authDb.ProcInfoResponseCode = followOnTransJson.ProcessorInformation?.ResponseCode ?? "0";
                authDb.ProcInfoTransactionId = followOnTransJson.ProcessorInformation?.TransactionId ?? "0";

                authDb.TokenInformationInstId = followOnTransJson.TokenInformation?.InstrumentIdentifier?.Id ?? "0";
                authDb.TokenInformationInstIdNew = Convert.ToString(followOnTransJson.TokenInformation?.InstrumentIdentifierNew) ?? "0";

                EntityEntry<AuthTransResponse> authEntity = db.AuthTransResponses.Add(authDb);
                Console.WriteLine($"Auth Trans State: {entity.State}, ID: {authDb.AuthTransResponsesId}");

                int affected3 = db.SaveChanges();
                Console.WriteLine($"Auth Trans State: {entity.State}, ID: {authDb.AuthTransResponsesId}");

            }
            catch (Exception ex)
            {
                dbResults = new();
                dbResults.Add("Exception in Token Persistance", ex.Message);
                Console.WriteLine($"Exception: {ex.Message}");
                return dbResults;
            }
            return dbResults;
        }
    }
}






