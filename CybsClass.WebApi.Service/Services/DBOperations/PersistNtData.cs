using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using CybsClass.Blazor.Model.Cybersource.NT;
using CybsClass.Cybersource.Models;
using CybsClass.Cybersource.Models.Json;
using CybsClass.EntityModels;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public static class PersistNtData
    {
        private static Dictionary<string, object> dbResults = new();
        public static async Task<Dictionary<string, object>> InsertNt(JsonNode decryptedNt)
        {
            dbResults = new();

            Console.WriteLine("Inserting network token data ...");

            RootToken rootToken = new();
            if (rootToken is not null)
            { 
                Console.WriteLine($"decryptedNt.ToString(): {decryptedNt.ToString()}");
                await Console.Out.WriteLineAsync("...");
            }

            try
            {
                rootToken = JsonSerializer.Deserialize<RootToken>(decryptedNt.ToString())!;
            }
            catch (Exception ex) { Console.WriteLine(ex); }

            try
            { 
                using B2cNorthwindContext db = new();

                NetworkTokenInfo networkTokenInfo = new NetworkTokenInfo();

                networkTokenInfo.PaymentCardId = Convert.ToInt32(rootToken?.PaymentCardId);
                networkTokenInfo.TokenExpMonth = rootToken?.TokenizedCard?.ExpirationMonth ?? "00";
                networkTokenInfo.TokenExpYear = rootToken?.TokenizedCard?.ExpirationYear ?? "00";
                networkTokenInfo.TokenAccountNumber = rootToken?.TokenizedCard?.Number ?? "00";
                networkTokenInfo.EnrollmentId = rootToken?.TokenizedCard?.EnrollmentId ?? "00";
                networkTokenInfo.OriginalAccountSuffix = rootToken?.TokenizedCard?.Card?.Suffix ?? "00";
                networkTokenInfo.OriginalAccountExpMonth = rootToken?.TokenizedCard?.Card?.ExpirationMonth ?? "00";
                networkTokenInfo.OriginalAccountExpYear = rootToken?.TokenizedCard?.Card?.ExpirationYear ?? "00";
                networkTokenInfo.OriginalAccountNumber = rootToken?.Card?.Number ?? "00";
                networkTokenInfo.PaymentAccountReferenceNumber = rootToken?.Issuer?.PaymentAccountReference ?? "00";
                networkTokenInfo.MitpreviousTransactionId = rootToken?.ProcessingInformation?.AuthorizationOptions?.Initiator?.MerchantInitiatedTransaction?.PreviousTransactionId ?? "00";
                networkTokenInfo.TokenState = rootToken?.TokenizedCard?.State ?? "00";
                networkTokenInfo.TokenizedCardType = rootToken?.TokenizedCard?.Type ?? "none";
                networkTokenInfo.TokenRequestorId = rootToken?.TokenizedCard?.RequestorId ?? "00";
                networkTokenInfo.ResponseTransactionJson = decryptedNt.ToString();

                EntityEntry<NetworkTokenInfo> entity = db.NetworkTokenInfos.Add(networkTokenInfo);
                Console.WriteLine($"NetworkTokenInfo State: {entity.State}, NetworkTokenInfoId: {networkTokenInfo.PaymentTokenId}");

                int affected0 = await db.SaveChangesAsync();
                Console.WriteLine($"NetworkTokenInfo State: {entity.State}, NetworkTokenInfoId: {networkTokenInfo.PaymentTokenId}");
                dbResults.Add("PaymentTokenId", networkTokenInfo.PaymentTokenId);

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

