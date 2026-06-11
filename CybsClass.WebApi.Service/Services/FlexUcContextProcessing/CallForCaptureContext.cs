using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.OutboundTransObjects;
using CybsClass.Cybersource.Transactions;
using CybsClass.WebApi.Service.Services.JWTProcessing;
using CybsClass.Cybersource.Authentication;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using CybsClass.Cybersource.Models.BaseData;


namespace CybsClass.WebApi.Service.Services.FlexUcContextProcessing
{
    public static class CallForCaptureContext
    {
        public static async Task<string> RunAsyncCaptureContextCreate(B2cCustomerDto b2cCustomerDto)
        {
            string flexToken = string.Empty;
            string resource = "/up/v1/capture-contexts";
            string deCodedToken = string.Empty;
            string modifiedJsonString = string.Empty;

            try
            {
                var CaptureContext = new CaptureContext
                {
                    TargetOrigins = new List<string> { b2cCustomerDto.TargetOrigin ?? "https://localhost:7133"},
                    AllowedCardNetworks = new string[] { "VISA", "MASTERCARD", "AMEX", "JCB", "DISCOVER", "DINERSCLUB" },
                    AllowedPaymentTypes = new string[] { "PANENTRY" },
                    Country = "US",
                    Locale = "en_US",
                    ClientVersion = "0.30",
                    OrderInformation = new OrderInformation
                    {
                        AmountDetails = new AmountDetails
                        {
                            TotalAmount = b2cCustomerDto.TotalAmount.ToString(),
                            Currency = "USD"
                        },
                        ShipTo = new BillTo
                        {
                            Address1 = b2cCustomerDto.Address1,
                            Address2 = b2cCustomerDto.Address2,
                            Address3 = b2cCustomerDto.Address3,
                            AdministrativeArea = b2cCustomerDto.AdministrativeArea,
                            BuildingNumber = b2cCustomerDto.BuildingNumber,
                            Country = b2cCustomerDto.Country,
                            Locality = b2cCustomerDto.City,
                            PostalCode = b2cCustomerDto.PostalCode,
                            FirstName = b2cCustomerDto.PostalCode,
                            LastName = b2cCustomerDto.LastName
                        },
                        BillTo = new BillTo
                        {
                            Address1 = b2cCustomerDto.Address1,
                            Address2 = b2cCustomerDto.Address2,
                            Address3 = b2cCustomerDto.Address3,
                            AdministrativeArea = b2cCustomerDto.AdministrativeArea,
                            BuildingNumber = b2cCustomerDto.BuildingNumber,
                            Country = b2cCustomerDto.Country,
                            Locality = b2cCustomerDto.City,
                            PostalCode = b2cCustomerDto.PostalCode,
                            FirstName = b2cCustomerDto.FirstName,
                            LastName = b2cCustomerDto.LastName,
                            Email = b2cCustomerDto.Email,
                            MiddleName = b2cCustomerDto.MiddleName,
                            Title = b2cCustomerDto.Title,
                            PhoneNumber = b2cCustomerDto.Phone,
                            PhoneType = b2cCustomerDto.PhoneType,
                        },
                    },
                    CaptureMandate = new CaptureMandate
                    {
                        BillingType = "NONE",
                        RequestEmail = true,
                        RequestPhone = true,
                        RequestShipping = true,
                        ShipToCountries = new string[] { "US" },
                        ShowAcceptedNetworkIcons = true
                    }
                };

                Console.WriteLine("\n************* CALLING FOR UNIFIED CHECKOUT Capture CONTEXT *****\n");

                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(CaptureContext, options);

                JsonNode? jsonNode = JsonNode.Parse(jsonString);

                if (jsonNode != null &&
                    jsonNode["orderInformation"] is JsonObject orderInfoObject &&
                    orderInfoObject["freight"] != null)
                {
                    orderInfoObject.Remove("freight");

                    modifiedJsonString = jsonNode.ToJsonString();
                    Console.WriteLine($"MODIFIED STRING WITHOUT FREIGHT: {modifiedJsonString}");
                }
                else
                {
                    Console.WriteLine("Freight node is missing or null.");
                }


                flexToken = await CallCyberSource.CallCyberSourceCaptureContext(modifiedJsonString, resource);

                Console.WriteLine("******************************DECODED JWT **************************");

                deCodedToken = JWTDeCode.DeCodeJWT(flexToken);

                Console.WriteLine(deCodedToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                flexToken = string.Empty;
                flexToken = e.Message;
                return e.Message;
            }

            return flexToken;
        }
    }
}
