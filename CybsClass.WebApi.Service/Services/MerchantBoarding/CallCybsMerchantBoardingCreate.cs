using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace CybsClass.WebApi.Service.Services.MerchantBoarding
{
    public static class CallCybsMerchantBoardingCreate
    {

        // This class populates the Authorization object for serialization
        public static async Task<JsonObject> RunAsyncJsonObject(MerchantModelDto merchantModelDto)
        {
            JsonObject jsonObject = new JsonObject();
            string resource = "/boarding/v1/registrations";
            Random random = new Random();
            int randomNumber = random.Next(0, 10000);


            try
            {

                var outboundBoardingRoot = new OutboundBoardingRoot
                {
                    RegistrationInformation = new RegistrationInformation
                    {
                        BoardingFlow = "ENTERPRISE",
                        Mode = "COMPLETE",
                        BoardingPackageId = merchantModelDto.BoardingPackageId ?? ""
                    },
                    OrganizationInformation = new OrganizationInformation
                    {
                        OrganizationId = merchantModelDto.OrganizationId,
                        Status = merchantModelDto.Status,
                        BusinessInformation = new BusinessInformation
                        {
                            Address = new AddressClass
                            {
                                Country = merchantModelDto.Country,
                                Address1 = merchantModelDto.Address1,
                                PostalCode = merchantModelDto.PostalCode,
                                AdministrativeArea = merchantModelDto.AdministrativeArea,
                                Locality = merchantModelDto.Locality,
                            },
                            BusinessContact = new Contact
                            {
                                FirstName = merchantModelDto.BusinessContactFirstName,
                                LastName = merchantModelDto.BusinessContactLastName,
                                PhoneNumber = merchantModelDto.BusinessContactPhoneNumber,
                                Email = merchantModelDto.BusinessContactEmail,
                            },
                            TechnicalContact = new Contact
                            {
                                FirstName = merchantModelDto.TechnicalContactFirstName,
                                LastName = merchantModelDto.TechnicalContactLastName,
                                PhoneNumber = merchantModelDto.TechnicalContactPhoneNumber,
                                Email = merchantModelDto.TechnicalContactEmail,
                            },
                            EmergencyContact = new Contact
                            {
                                FirstName = merchantModelDto.EmergencyContactFirstName,
                                LastName = merchantModelDto.EmergencyContactLastName,
                                PhoneNumber = merchantModelDto.EmergencyContactPhoneNumber,
                                Email = merchantModelDto.EmergencyContactEmail,
                            },
                            Name = merchantModelDto.Name,
                            WebsiteUrl = merchantModelDto.WebsiteUrl,
                            PhoneNumber = merchantModelDto.BusinessInformationPhoneNumber,
                            TimeZone = merchantModelDto.BusinessInformationTimeZone,
                            MerchantCategoryCode = merchantModelDto.MerchantCategoryCode?.ToString() ?? "5999"
                        },
                        ParentOrganizationId = merchantModelDto.ParentOrganizationId,
                        Type = merchantModelDto.Type,
                        Configurable = string.Equals(merchantModelDto.Configurable, "TRUE", StringComparison.OrdinalIgnoreCase),
                    },
                };


                var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(outboundBoardingRoot, options);

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

                Console.WriteLine("\n************* CALLING FOR ORG LEVEL MERCHANT BOARDING *****\n");

                jsonObject = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, true);

                if (jsonObject != null && jsonObject.ToString().Contains($"SUCCESS") && merchantModelDto.TransactingMerchant)
                {
                    var jsonNode = JsonNode.Parse(jsonObject!.ToString());
                    MerchantBoardingResponse merchantBoardingResponse = JsonSerializer.Deserialize<MerchantBoardingResponse>(jsonNode!.ToString()!)!;
                    merchantModelDto.ParentOrganizationId = merchantBoardingResponse!.OrganizationInformation!.OrganizationId;
                    merchantModelDto.OrganizationId = merchantBoardingResponse!.OrganizationInformation!.OrganizationId + "001";

                    jsonObject = await CallCybsMerchantTransactingCreate.RunAsyncJsonObject(merchantModelDto);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                jsonObject = new JsonObject();
                jsonObject["error"] = e.Message;
                return jsonObject;
            }

            return jsonObject!;
        }
    }
}