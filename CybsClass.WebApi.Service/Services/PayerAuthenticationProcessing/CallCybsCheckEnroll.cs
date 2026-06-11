using CybsClass.Cybersource.Models;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Transactions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace CybsClass.WebApi.Service.Services.PayerAuthenticationProcessing
{
    public static class CallCybsCheckEnroll
    {

        // This class populates the Authorization object for serialization
        public static async Task<JsonObject> RunAsyncFlexJsonObject(CheckEnrollDto checkEnrollDto)
        {
            JsonObject jsonObject = new JsonObject();
            string resource = "/risk/v1/authentications";

            try
            {
                if (checkEnrollDto.ProcessingInformation is not null)
                {
                    checkEnrollDto.ProcessingInformation.ActionList = new string[] { "TOKEN_CREATE" };
                    checkEnrollDto.ProcessingInformation.ActionTokenTypes = new string[] { "paymentInstrument, instrumentIdentifier" };
                }

                var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(checkEnrollDto, options);

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

                Console.WriteLine("\n************* CALLING FOR PAYER AUTH CHECK ENROLLMENT *****\n");


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

        public static async Task<JsonObject> RunAsyncFlexAftJsonPaymentObject(AftCheckEnrollDto aftCheckEnrollDto)
        {
            JsonObject jsonObject = new JsonObject();
            string resource = "/pts/v2/payments";

            if (aftCheckEnrollDto.ProcessingInformation is not null)
            {
                aftCheckEnrollDto.ProcessingInformation.ActionList = new string[] { "TOKEN_CREATE" };
                aftCheckEnrollDto.ProcessingInformation.ActionTokenTypes = new string[] { "paymentInstrument, instrumentIdentifier" };
            }

            aftCheckEnrollDto.ClientReferenceInformation = new ClientReferenceInformation
            {
                Code = "AFT" + DateTime.UtcNow.ToString("yyyyMMddHHmmss")
            };

            aftCheckEnrollDto.ProcessingInformation = new ProcessingInformation
            {
                AuthorizationOptions = new AuthorizationOptions
                {
                    AftIndicator = "y",
                },

                CommerceIndicator = "internet",
                BusinessApplicationId = "AA",
                PurposeOfPayment = "ISACCT",
                ActionList = new[] { "CONSUMER_AUTHENTICATION", "TOKEN_CREATE" },
                ActionTokenTypes = new string[] { "paymentInstrument, instrumentIdentifier" }
            };

            aftCheckEnrollDto.MerchantInformation = new MerchantInformation
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
                string jsonString = JsonSerializer.Serialize(aftCheckEnrollDto, options);

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

                Console.WriteLine("\n************* CALLING FOR PAYER AUTH CHECK ENROLLMENT WITH AUTHORIZATION *****\n");


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