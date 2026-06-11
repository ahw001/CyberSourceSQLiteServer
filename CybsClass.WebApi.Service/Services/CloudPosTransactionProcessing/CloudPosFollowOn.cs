using CybsClass.Cybersource.Authentication;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CybsClass.WebApi.Service.Services.CloudPosTransactionProcessing
{
    public static class CloudPosFollowOn
    {
        public static async Task<JsonObject> CallForFollowOn(B2cCustomerDto b2cCustomerDto)
        {
            JsonObject jsonObject = new JsonObject();
            string resource = "/v1/cloud/transactions";
            Guid guid = Guid.NewGuid();

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

                    Console.WriteLine("\n************* CALLING FOR CLOUD MODE TRANSACTION LOOKUP REQUEST *****\n");
                }

                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(posTransactionRequest, options);

                // Parse the JSON string into a JsonNode
                JsonNode? jsonNode = JsonNode.Parse(jsonString);

                if (jsonNode is JsonObject obj)
                {
                    // Remove the "Capture" element
                    jsonNode?["request"]?.AsObject()?.Remove("capture");
                }

                // Convert back to a JSON string
                string transJson = jsonNode?.ToJsonString()!;

                jsonObject = await CallCyberSourcePos.CallCyberSourceJwtPosApiJson(transJson, resource, bearerToken);

                return await Task.FromResult(jsonObject);
            }
            catch (Exception ex)
            {
                jsonObject = new JsonObject();
                jsonObject.Add("error", ex.Message);
                return jsonObject;  // Return the error JsonObject
            }
        }
    }
}
