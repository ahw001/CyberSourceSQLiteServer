using CybsClass.Cybersource.Authentication;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using CybsClass.Cybersource.PosTransactions;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;



namespace CybsClass.WebApi.Service.Services.WssPosProcessing
{
    public static class WssPosSaleService
    {
        public static async Task<string> CallForPosSale(B2cCustomerDto b2cCustomerDto)
        {
            JsonObject jsonObject = new JsonObject();
            Guid guid = Guid.NewGuid();

            string wssPosResponse = string.Empty;

            try
            {
                var posSaleRequest = new SemiPosTransactionRequest
                {
                    Type = "PaymentRequest",
                    MerchantReferenceCode = guid.ToString(),
                    AmountDetails = new AmountDetails
                    {
                        Amount = b2cCustomerDto.TotalAmount?.ToString("F2") ?? "0.00",
                        Currency = "USD",
                    }
                };

                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(posSaleRequest, options);

                //jsonObject = await CallCyberSourcePos.CallCyberSourceJwtPosApiJson(jsonString, bearerToken);

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
