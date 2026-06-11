using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.OutboundTransObjects;
using CybsClass.Cybersource.Transactions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using CybsClass.Cybersource.Models.BaseData;

namespace CybsClass.WebApi.Service.Services.CaptureContextProcessing
{
    public static class CallCybsCtxPayment
    {
        public static async Task<JsonObject> RunAsyncCtxBlobObject(CtxPaymentDto ctxPaymentDto)
        {
            JsonObject ctxBlob = new JsonObject(); // Fixed initialization
            string resource = $"/pts/v2/payments";

            try
            {
                // Ensure OrderInformation and BillTo are not null before accessing them
                if (ctxPaymentDto.OrderInformation == null || ctxPaymentDto.OrderInformation.BillTo == null)
                {
                    throw new ArgumentNullException("OrderInformation or BillTo is null in the provided CtxPaymentDto.");
                }

                var ctx = new CtxPaymentDto
                {
                    ClientReferenceInformation = ctxPaymentDto.ClientReferenceInformation,
                    OrderInformation = ctxPaymentDto.OrderInformation,
                    BillTo = ctxPaymentDto.OrderInformation.BillTo, // Safe access ensured
                    TokenInformation = ctxPaymentDto.TokenInformation,
                    ProcessingInformation = new ProcessingInformation { ActionList = new[] { "TOKEN_CREATE" } }
                };

                var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

                string jsonString = JsonSerializer.Serialize(ctx, options);

                JsonObject requestJson = JsonSerializer.Deserialize<JsonObject>(jsonString)!;

                string jsonTokenString = JsonSerializer.Serialize(requestJson, options);

                Console.WriteLine("\n************* CALLING FOR Capture CONTEXT PAYMENT *****\n");

                ctxBlob = await CallCyberSource.CallCyberSourceApiJson(jsonTokenString, resource, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR IN Capture CONTEXT PAYMENT");
                Console.WriteLine(e.Message);
                ctxBlob = new JsonObject();
                ctxBlob.Add("error", e.Message); // Fixed key formatting
                return ctxBlob;
            }

            return ctxBlob;
        }
    }
}
