using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using CybsClass.Cybersource.Models.BaseData; 
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace CybsClass.WebApi.Service.Services.AftProcessing
{
    public static class CallForAft
    {

        // This class populates the Authorization Fund Transfer object for serialization
        public static async Task<JsonObject> RunAsyncJsonObject(AftRequestDto aftRequestDto)
        {
            if (aftRequestDto == null)
            {
                throw new ArgumentNullException(nameof(aftRequestDto), "AftRequestDto cannot be null.");
            }

            JsonObject jsonObject = new JsonObject();
            string resource = "/pts/v2/payments";

            aftRequestDto.ClientReferenceInformation = new ClientReferenceInformation
            {
                Code = "AFT" + DateTime.UtcNow.ToString("yyyyMMddHHmmss")
            };

            aftRequestDto.ProcessingInformation = new ProcessingInformation
            {
                AuthorizationOptions = new AuthorizationOptions
                {
                    AftIndicator = "y",
                },
                CommerceIndicator = "internet",
                BusinessApplicationId = "AA",
                PurposeOfPayment = "ISACCT",
            };

            if (aftRequestDto.ProcessingInformation is not null && aftRequestDto.CurrentTransaction is not null
                && aftRequestDto.CurrentTransaction == "STANDALONE_AFT_TRANSACTION")
            {
                aftRequestDto.ProcessingInformation.ActionList = new string[] { "TOKEN_CREATE" };
                aftRequestDto.ProcessingInformation.ActionTokenTypes = new string[] { "paymentInstrument, instrumentIdentifier" };
            }

            aftRequestDto.MerchantInformation = new MerchantInformation
            {
                MerchantCategoryCode = "6012",
                MerchantDescriptor = new MerchantDescriptor
                {
                    Name = "Test Merchant",
                    Locality = "San Francisco",
                    AdministrativeArea = "CA",
                    Address1 = "123 Test St",
                    PostalCode = "94105"
                }
            };

            try
            {

                var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(aftRequestDto, options);

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

                Console.WriteLine("\n************* CALLING FOR AFT PULL *****\n");

                Console.WriteLine($"JSON REQUEST STRING: {jsonString}");    

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