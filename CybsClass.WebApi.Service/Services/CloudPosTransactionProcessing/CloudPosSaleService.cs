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
    public static class CloudPosSaleService
    {
        public static async Task<JsonObject> CallForPosSale(B2cCustomerDto b2cCustomerDto)
        {
            JsonObject jsonObject = new JsonObject();
            string resource = "/v1/cloud/transactions";
            Guid guid = Guid.NewGuid();

            try
            {

                string bearerToken = b2cCustomerDto.BearerToken!;

                var posSaleRequest = new PosTransactionRequest
                {
                    SerialNumber = PosCredentials.GetAcceptanceDeviceSerialNumber(),

                    Request = new Request
                    {
                        Type = "PaymentRequest",
                        MerchantReferenceCode = guid.ToString(),
                        AmountDetails = new AmountDetails
                        {
                            Amount = b2cCustomerDto.TotalAmount?.ToString("F2") ?? "0.00",
                            Currency = "USD",
                        }
                    }
                    /*
                    MerchantInformation = new MerchantInformation
                    {
                        MerchantDescriptor = new MerchantDescriptor
                        {
                            Name = "CybsClassTraders"
                        }
                    },
                    */
                };

                if (b2cCustomerDto is not null && b2cCustomerDto.CloudPaymentMode is not null && b2cCustomerDto.CloudPaymentMode == "MOTO")
                {
                    posSaleRequest.Request.PaymentMode = b2cCustomerDto.CloudPaymentMode;
                }

                if (b2cCustomerDto is not null && b2cCustomerDto.OnDeviceTip)
                {
                    posSaleRequest.Request.AskForTip = "ON_DEVICE";
                    posSaleRequest.Request.Capture = true;
                }
                else if (b2cCustomerDto is not null && b2cCustomerDto.PreAuthTip)
                {
                    posSaleRequest.Request.AskForTip = "ON_RECEIPT";
                    posSaleRequest.Request.Capture = false;
                }
                else if (b2cCustomerDto is not null && b2cCustomerDto.PreAuthOnly)
                {
                    posSaleRequest.Request.Capture = false;
                }
                else if (b2cCustomerDto is not null)
                {
                    posSaleRequest.Request.Capture = true;
                }

                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(posSaleRequest, options);

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
