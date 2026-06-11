using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace CybsClass.WebApi.Service.Services.PayerAuthenticationProcessing
{
    public static class CallCybsValidate
    {

        // This class populates the Authorization object for serialization
        public static async Task<JsonObject> RunAsyncFlexJsonObject(AftValidateDto aftValidateDto)
        {
            JsonObject jsonObject = new JsonObject();
            string resource = "/risk/v1/authentication-results";

            try
            {

                var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(aftValidateDto, options);

                /* USE IF YOU NEED TO MODIFIY JSON ---------
                 * 
                JsonNode jsonNode = JsonNode.Parse(jsonString);

                // Navigate to the 'ProcessingInformation' object
                JsonNode processingInfo = jsonNode["ProcessingInformation"];

                // Check if the 'Capture' field exists and remove it
                if (processingInfo["Capture"] != null)
                {
                    processingInfo.AsObject().Remove("Capture");
                }

                // Convert the JsonNode back to a string to see the result
                string modifiedJsonString = jsonNode.ToJsonString();
                Console.WriteLine($"MODIFIED STRING WITHOUT Capture: { modifiedJsonString}");
                *
                */

                Console.WriteLine("\n************* CALLING FOR PAYER AUTH VALIDATION *****\n");


                jsonObject = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, false);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                jsonObject = new JsonObject();
                jsonObject["error"] = e.Message;
                return jsonObject;
            }

            return jsonObject;
        }
    }
}