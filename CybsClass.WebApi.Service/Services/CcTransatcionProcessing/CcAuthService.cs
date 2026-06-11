using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using CybsClass.WebApi.Service.Services.DBOperations;
using System.Text.Json.Nodes;



namespace CybsClass.WebApi.Service.Services.CcTransatcionProcessing
{
    public class CcAuthService
    {

        public async Task<JsonObject> CallForAuth(B2cCustomerDto b2cCustomerDto)
        {

            JsonObject jsonObject = new JsonObject();

            jsonObject = await CallForCybsAuthTokenCreate.RunAsyncJsonObject(b2cCustomerDto);

            return await Task.FromResult(jsonObject);
        }

    }
}
