using CybsClass.Cybersource.Models;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.OutboundTransObjects;
using CybsClass.Cybersource.Transactions;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace CybsClass.WebApi.Service.Services.CcTransatcionProcessing
{
    public static class CallForCybsAuthTokenCreate
    {
        // This class populates the Authorization object for serialization
        public static async Task<JsonObject> RunAsyncJsonObject(B2cCustomerDto b2cCustomerDto)
        {
            JsonObject jsonObject = new JsonObject();
            string resource = "/pts/v2/payments";
            bool markedForCapture = b2cCustomerDto.MarkedForCapture;
            bool allowPartialAuth = b2cCustomerDto.AllowPartialAuth;
            Guid guid = Guid.NewGuid();
            string formattedAmount = string.Format("{0:F2}", b2cCustomerDto.TotalAmount.ToString());

            if (b2cCustomerDto.CardType == "002")
            { 
                b2cCustomerDto.Reason = "7";
            }

            try
            {

                var authorizeData = new AuthorizeData
                {

                    ClientReferenceInformation = new ClientReferenceInformation
                    {
                        Code = guid.ToString()

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

                        AmountDetails = new AmountDetails { Currency = "USD", TotalAmount = formattedAmount }, //Convert.ToString(b2cCustomerDto.TotalAmount)

                        BillTo = new BillTo { FirstName = b2cCustomerDto.FirstName, LastName = b2cCustomerDto.LastName, Address1 = b2cCustomerDto.Address1, Locality = b2cCustomerDto.City, AdministrativeArea = b2cCustomerDto.AdministrativeArea, PostalCode = b2cCustomerDto.PostalCode, Country = "US", Email = b2cCustomerDto.Email, PhoneNumber = b2cCustomerDto.Phone },
                        ShipTo = new BillTo { FirstName = b2cCustomerDto.FirstName, LastName = b2cCustomerDto.LastName, Address1 = b2cCustomerDto.Address1, Locality = b2cCustomerDto.City, AdministrativeArea = b2cCustomerDto.AdministrativeArea, PostalCode = b2cCustomerDto.PostalCode, Country = "US" }

                    },
                    ProcessingInformation = new ProcessingInformation { ActionList = new[] { "TOKEN_CREATE" }, ActionTokenTypes = b2cCustomerDto.ActionTokenTypes, Capture = markedForCapture.ToString().ToLower(), AuthorizationOptions = allowPartialAuth ? new AuthorizationOptions { PartialAuthIndicator = "true", IgnoreAvsResult = true, Initiator = new Initiator { Type = "customer", CredentialStoredOnFile = "true", MerchantInitiatedTransaction = new MerchantInitiatedTransaction { Reason = b2cCustomerDto.Reason } } } : new AuthorizationOptions { Initiator = new Initiator { Type = "customer", CredentialStoredOnFile = "true", MerchantInitiatedTransaction = new MerchantInitiatedTransaction { Reason = b2cCustomerDto.Reason } } } }
                    //ProcessingInformation = new ProcessingInformation { Capture = markedForCapture.ToString().ToLower() }
                };

                var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull};
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

                Console.WriteLine("\n************* CALLING FOR AUTH AND TOKEN CREATE *****\n");

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