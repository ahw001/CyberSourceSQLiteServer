using CybsClass.Cybersource.Authentication;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;



namespace CybsClass.WebApi.Service.Services.WssPosProcessing
{
    public static class WssPosReturnService
    {
        public static async Task<JsonObject> CallForPosReturn(B2cCustomerDto b2cCustomerDto)
        {
            JsonObject jsonObject = new JsonObject();
            string resource = "/v1/cloud/transactions";
            Guid guid = Guid.NewGuid();

            try
            {
                string bearerToken = b2cCustomerDto.BearerToken!;

                var posReturnRequest = new PosReturnRequest
                {
                    SerialNumber = PosCredentials.GetAcceptanceDeviceSerialNumber(),
                    Request = new Request
                    {
                        Type = "LinkedRefundRequest",
                        MerchantReferenceCode = guid.ToString(),
                        AmountDetails = new AmountDetails
                        {
                            Amount = b2cCustomerDto.TotalAmount.ToString(),
                            Currency = "USD",
                        }
                    }
                };

                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(posReturnRequest, options);

                jsonObject = await CallCyberSourcePos.CallCyberSourceJwtPosApiJson(jsonString, resource, bearerToken);

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
