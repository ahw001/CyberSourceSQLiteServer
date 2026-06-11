using CybsClass.Cybersource.Authentication;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.PosTransactions;
using CybsClass.Cybersource.Transactions;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CybsClass.WebApi.Service.Services.WssPosProcessing
{
    public static class WssPosFollowOn
    {
        public static async Task<string> CallForFollowOn(B2cCustomerDto b2cCustomerDto)
        {
            Guid guid = Guid.NewGuid();
            string wssPosResponse = string.Empty;

            try
            {

                string bearerToken = b2cCustomerDto.BearerToken!;

                var posTransactionRequest = new PosTransactionRequest
                {
                    SerialNumber = PosCredentials.GetAcceptanceDeviceSerialNumber(),
                    Request = new Request
                    {
                        Type = b2cCustomerDto.CloudPosType ?? null,
                    }
                };

                if (b2cCustomerDto is not null && b2cCustomerDto.CloudPosType is not null && b2cCustomerDto.CloudPosType == "PaymentRequest")
                {
                    posTransactionRequest.Request.TransactionId = b2cCustomerDto.PosTransId ?? null;
                    posTransactionRequest.Request.AmountDetails = new AmountDetails
                    {
                        Amount = b2cCustomerDto.TotalAmount?.ToString("F2") ?? "0.00",
                        Currency = "USD",
                    };

                    Console.WriteLine("\n************* CALLING FOR CLOUD MODE PAYMENT REQUEST *****\n");
                }
                if (b2cCustomerDto is not null && b2cCustomerDto.CloudPosType is not null && b2cCustomerDto.CloudPosType == "CaptureRequest")
                {
                    posTransactionRequest.Request.OriginalTransactionId = b2cCustomerDto.PosTransId ?? null;
                    posTransactionRequest.Request.AmountDetails = new AmountDetails
                    {
                        Amount = b2cCustomerDto.TotalAmount?.ToString("F2") ?? "0.00",
                        Currency = "USD",
                    };

                    Console.WriteLine("\n************* CALLING FOR CLOUD MODE Capture REQUEST *****\n");
                }
                else if (b2cCustomerDto is not null && b2cCustomerDto.CloudPosType is not null && b2cCustomerDto!.CloudPosType == "StandaloneRefundRequest")
                {
                    posTransactionRequest.Request.MerchantReferenceCode = guid.ToString() ?? null;
                    posTransactionRequest.Request.AmountDetails = new AmountDetails
                    {
                        Amount = b2cCustomerDto.TotalAmount?.ToString("F2") ?? "0.00",
                        Currency = "USD",
                    };

                    Console.WriteLine("\n************* CALLING FOR CLOUD MODE STAND ALONE REFUND REQUEST *****\n");
                }
                else if (b2cCustomerDto is not null && b2cCustomerDto.CloudPosType is not null && b2cCustomerDto!.CloudPosType == "LinkedRefundRequest")
                {
                    posTransactionRequest.Request.OriginalTransactionId = b2cCustomerDto.PosTransId ?? null;
                    posTransactionRequest.Request.AmountDetails = new AmountDetails
                    {
                        Amount = b2cCustomerDto.TotalAmount?.ToString("F2") ?? "0.00",
                        Currency = "USD",

                    };

                    Console.WriteLine("\n************* CALLING FOR CLOUD MODE LINKED REFUND REQUEST *****\n");
                }
                else if (b2cCustomerDto is not null && b2cCustomerDto.CloudPosType is not null && b2cCustomerDto!.CloudPosType == "TokenRefundRequest")
                {
                    posTransactionRequest.Request.MerchantReferenceCode = guid.ToString();
                    posTransactionRequest.Request.InstrumentId = b2cCustomerDto.InstrumentIdentifier;
                    posTransactionRequest.Request.AmountDetails = new AmountDetails
                    {

                        Amount = b2cCustomerDto.TotalAmount?.ToString("F2") ?? "0.00",
                        Currency = "USD",

                    };
                }
                else if (b2cCustomerDto is not null && b2cCustomerDto.CloudPosType is not null && b2cCustomerDto.CloudPosType == "TransactionLookupRequest"
                    && b2cCustomerDto.PosTransId is not null && b2cCustomerDto.IdType == "TRANSACTION_ID")
                {
                    posTransactionRequest.Request.IdType = b2cCustomerDto.IdType ?? null;
                    posTransactionRequest.Request.TransactionId = b2cCustomerDto.PosTransId ?? null;

                    Console.WriteLine("\n************* CALLING FOR WEB SOCKET TRANSACTION LOOKUP REQUEST *****\n");
                }

                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(posTransactionRequest, options);

                // Parse the JSON string into a JsonNode
                JsonNode? jsonNode = JsonNode.Parse(jsonString);

                if (jsonNode is JsonObject obj)
                {
                    // Remove the "Capture" element
                    jsonNode?["request"]?.AsObject()?.Remove("Capture");
                }

                // Convert back to a JSON string
                string transJson = jsonNode?.ToJsonString()!;

                wssPosResponse = await WssPosTransaction.WssCybsTransaction(jsonString);

                return await Task.FromResult(wssPosResponse);
            }
            catch (Exception ex)
            {
                string errorMessage = "error" + ex.Message;
                return errorMessage;  // Return the error Message
            }
        }
    }
}
