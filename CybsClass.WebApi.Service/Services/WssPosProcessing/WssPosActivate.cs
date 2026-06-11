using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Transactions;
using CybsClass.Cybersource.Authentication;
using CybsClass.Cybersource.Models.Json;
using CybsClass.Cybersource.PosTransactions;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using CybsClass.Cybersource.Models.DTOs;





namespace CybsClass.WebApi.Service.Services.WssPosProcessing
{
    public static class WssPosActivate
    {
        public static async Task<string> CallActivation(B2cCustomerDto b2cCustomerDto)
        {
            string response = string.Empty;

            PosSetup setup = new()
            {
                PosId = b2cCustomerDto.PosSetupCode,
                SetupCode = b2cCustomerDto.PosActivationCode
            };

            string jsonString = JsonSerializer.Serialize(setup);

            response = await WssPosTransaction.WssCybsTransaction(jsonString);

            return await Task.FromResult(response);
        }

    }
}
