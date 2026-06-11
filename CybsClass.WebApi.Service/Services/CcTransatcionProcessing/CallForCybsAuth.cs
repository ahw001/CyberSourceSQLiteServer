using CybsClass.Cybersource.Models;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.OutboundTransObjects;
using CybsClass.Cybersource.Transactions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using CybsClass.Cybersource.Models.BaseData;


namespace CybsClass.WebApi.Service.Services.CcTransatcionProcessing
{
    public static class CallForCybsAuth
    {

        // This class populates the Authorization object for serialization
        public static async Task<JsonObject> RunAsyncJsonObject(B2cCustomerDto b2cCustomerDto)
        {
            JsonObject jsonObject = new JsonObject();
            string resource = "/pts/v2/payments";
            bool markedForCapture = b2cCustomerDto.MarkedForCapture;
            bool allowPartialAuth = b2cCustomerDto.AllowPartialAuth;

            try
            {

                var authorizeData = new AuthorizeData
                {


                    ClientReferenceInformation = new ClientReferenceInformation
                    {
                        Code = "ABC123"

                    },
                    PaymentInformation = new Dictionary<string, FullCard>
                    {

                        ["card"] = new FullCard
                        {
                            Number = b2cCustomerDto.AccountNumber,
                            ExpirationMonth = b2cCustomerDto.ExpMonth,
                            ExpirationYear = b2cCustomerDto.ExpYear,
                            SecurityCode = b2cCustomerDto.Cvv,
                            Type = b2cCustomerDto.CardType
                        },

                    },
                    OrderInformation = new OrderInformation
                    {

                        AmountDetails = new AmountDetails { Currency = "USD", TotalAmount = Convert.ToString(b2cCustomerDto.TotalAmount) }, //Convert.ToString(b2cCustomerDto.TotalAmount)

                        BillTo = new BillTo { FirstName = b2cCustomerDto.FirstName, LastName = b2cCustomerDto.LastName, Address1 = b2cCustomerDto.Address1, Locality = b2cCustomerDto.City, AdministrativeArea = b2cCustomerDto.AdministrativeArea, PostalCode = b2cCustomerDto.PostalCode, Country = "US", Email = b2cCustomerDto.Email, PhoneNumber = b2cCustomerDto.Phone }

                    },
                    ProcessingInformation = new ProcessingInformation
                    {
                        CommerceIndicator = "",
                        ActionList = new[] { "TOKEN_CREATE" },
                        ActionTokenTypes = new[] { "paymentInstrument, instrumentIdentifier" },
                        Capture = markedForCapture.ToString(),
                        AuthorizationOptions = allowPartialAuth
                            ? new AuthorizationOptions { PartialAuthIndicator = "true", IgnoreAvsResult = true }
                            : new AuthorizationOptions { IgnoreAvsResult = true }
                    }
                };

                var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(authorizeData, options);

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

                Console.WriteLine("\n************* CALLING FOR STANDARD AUTH *****\n");

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