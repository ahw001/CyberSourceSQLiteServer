
using CybsClass.Cybersource.Authentication;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Transactions;
using System.Text.Encodings.Web;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.WebApi.Service.Services.Configs;

namespace CybsClass.WebApi.Service.Services.Utilities
{
    public static class SimpleJsonProcessor
    {
        public static async Task<JsonObject> CallForSimpleJson(SimpleJsonProcessorDto simpleJsonProcessorDto)
        {
            JsonObject jsonObject = new JsonObject();

            string resource = simpleJsonProcessorDto.Resource;
            string value = simpleJsonProcessorDto.Value;
            bool boardingAPI = simpleJsonProcessorDto.IsBoarding;

            string method = !string.IsNullOrWhiteSpace(simpleJsonProcessorDto.HttpMethod)
                ? simpleJsonProcessorDto.HttpMethod.ToUpperInvariant()
                : UrlEndpointConfig.ResolveMethod(resource);

            string requestEncryption = (simpleJsonProcessorDto.RequestEncryption ?? "none").ToLowerInvariant();
            string responseEncryption = (simpleJsonProcessorDto.ResponseEncryption ?? "none").ToLowerInvariant();

            Console.WriteLine("\n************* CALLING FOR SIMPLE JSON PROCESSING *****\n");
            Console.WriteLine($"RequestEncryption: {requestEncryption}, ResponseEncryption: {responseEncryption}, Method: {method}, Resource: {resource}, Boarding: {boardingAPI}");

            if (requestEncryption == "none" && responseEncryption == "none")
            {
                // No encryption — preserve full HTTP method routing.
                jsonObject = method switch
                {
                    "GET"    => await CallCyberSource.CallCyberSourceApiGet(resource, boardingAPI),
                    "PATCH"  => await CallCyberSource.CallCyberSourceApiJsonPatch(value, resource, boardingAPI),
                    "DELETE" => await CallCyberSource.CallCyberSourceApiDelete(resource, boardingAPI),
                    _        => await CallCyberSource.CallCyberSourceApiJson(value, resource, boardingAPI)
                };
            }
            else
            {
                // Any encryption combination routes through the two-axis grid (POST only).
                jsonObject = await CallCyberSource.CallCyberSourceApiJsonMleGrid(value, resource, requestEncryption, responseEncryption, boardingAPI);
            }

            return await Task.FromResult(jsonObject);
        }
    }
}
