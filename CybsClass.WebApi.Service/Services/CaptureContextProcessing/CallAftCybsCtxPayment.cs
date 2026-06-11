using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.OutboundTransObjects;
using CybsClass.Cybersource.Transactions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using CybsClass.Cybersource.Models.BaseData;

namespace CybsClass.WebApi.Service.Services.CaptureContextProcessing
{
    public static class CallAftCybsCtxPayment
    {
        public static async Task<JsonObject> RunAsyncCtxBlobObject(AftRequestDto aftRequestDto)
        {
            JsonObject ctxBlob = new JsonObject(); // Fixed initialization
            string resource = $"/pts/v2/payments";

            try
            {
                aftRequestDto.ClientReferenceInformation = new ClientReferenceInformation
                {
                    Code = "AFT" + DateTime.UtcNow.ToString("yyyyMMddHHmmss")
                };

                if (aftRequestDto.ProcessingInformation is null)
                {
                    aftRequestDto.ProcessingInformation = new();
                }

                if (aftRequestDto.ProcessingInformation.ActionList is null)
                {
                    aftRequestDto.ProcessingInformation.ActionList = new string[] { "TOKEN_CREATE" };
                    aftRequestDto.ProcessingInformation.ActionTokenTypes = new string[] { "paymentInstrument, instrumentIdentifier" };
                }

                aftRequestDto.ProcessingInformation.CommerceIndicator = "internet";
                aftRequestDto.ProcessingInformation.BusinessApplicationId = "AA";
                aftRequestDto.ProcessingInformation.PurposeOfPayment = "ISACCT";

                if (aftRequestDto.ProcessingInformation.AuthorizationOptions is null)
                {
                    aftRequestDto.ProcessingInformation.AuthorizationOptions = new();
                }

                aftRequestDto.ProcessingInformation.AuthorizationOptions.AftIndicator = "y";


                aftRequestDto.MerchantInformation = new MerchantInformation
                {
                    MerchantCategoryCode = "6012",
                    MerchantDescriptor = new MerchantDescriptor
                    {
                        Name = "Test Merchant",
                        Locality = "San Francisco",
                        AdministrativeArea = "CA",
                        Address1 = "123 Test St",
                        PostalCode = "94105"
                    }
                };

                var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

                string jsonString = JsonSerializer.Serialize(aftRequestDto, options);

                JsonObject requestJson = JsonSerializer.Deserialize<JsonObject>(jsonString)!;

                string jsonTokenString = JsonSerializer.Serialize(requestJson, options);

                Console.WriteLine("\n************* CALLING FOR AFT FLEX TRANSACTION *****\n");

                ctxBlob = await CallCyberSource.CallCyberSourceApiJson(jsonTokenString, resource, false);

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR IN Capture CONTEXT PAYMENT");
                Console.WriteLine(e.Message);
                ctxBlob = new JsonObject();
                ctxBlob.Add("error", e.Message); // Fixed key-value pair syntax
                return ctxBlob;
            }

            return ctxBlob;
        }
    }
}
