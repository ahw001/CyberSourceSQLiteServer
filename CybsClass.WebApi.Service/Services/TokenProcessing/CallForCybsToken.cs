using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.OutboundTransObjects;
using CybsClass.Cybersource.Transactions;
using CybsClass.Cybersource.Models.BaseData;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace CybsClass.WebApi.Service.Services.TokenProcessing
{
    public static class CallForCybsToken
    {
        private static JsonObject jsonObject = new();
        private static JsonObject jsonItem = new();
        private static string resource = string.Empty;
        private static string? jsonError;
        private static string? jsonString;
        private static string error = string.Empty;
        private static string customerInstrumentId = string.Empty;
        private static string instrumentId = string.Empty;
        private static string currentTransactionType = string.Empty;

        public static async Task<JsonObject> RunAsyncJsonObject(B2cCustomerDto b2cCustomerDto)
        {
            var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
            jsonObject = new();
            try
            {
                List<string> ActionTokenTypes = b2cCustomerDto!.ActionTokenTypes!.ToList();

                if (b2cCustomerDto is not null && b2cCustomerDto.ActionTokenTypes is not null && ActionTokenTypes.Count > 1)
                {
                    if (b2cCustomerDto is not null && b2cCustomerDto.ActionTokenTypes is not null && ActionTokenTypes.Contains("customer"))
                    {
                        currentTransactionType = "customer";
                        resource = "/tms/v2/customers";

                        var customerTokenData = new CustomerTokenData
                        {

                            BuyerInformation = new BuyerInformation
                            {
                                MerchantCustomerID = b2cCustomerDto.MerchantCustomerID,
                                Email = b2cCustomerDto.Email,
                            }
                        };

                        jsonString = JsonSerializer.Serialize(customerTokenData, options);

                        Console.WriteLine($"TOKEN CREATE FOR {currentTransactionType}:\n {jsonString}");

                        jsonItem = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, false);

                        jsonObject.Add("customer", jsonItem);

                        customerInstrumentId = (string)jsonItem!["id"]! ?? "null";

                        ActionTokenTypes.Remove("customer");

                    }
                    if (b2cCustomerDto is not null && b2cCustomerDto.ActionTokenTypes is not null && ActionTokenTypes.Contains("shippingAddress"))
                    {
                        currentTransactionType = "shippingAddress";
                        if (b2cCustomerDto.CustomerInstrumentId is not null)
                        {
                            resource = $"/tms/v2/customers/{b2cCustomerDto.CustomerInstrumentId}/shipping-addresses";
                        }
                        else
                        {
                            resource = $"/tms/v2/customers/{customerInstrumentId}/shipping-addresses";
                        }


                        var customerTokenShippingAddress = new CustomerTokenShippingAddress
                        {
                            Default = "true",

                            ShipTo = new BillTo
                            {
                                FirstName = b2cCustomerDto.FirstName,
                                LastName = b2cCustomerDto.LastName,
                                Address1 = b2cCustomerDto.Address1,
                                Locality = b2cCustomerDto.City,
                                AdministrativeArea = b2cCustomerDto.AdministrativeArea,
                                PostalCode = b2cCustomerDto.PostalCode,
                                Country = b2cCustomerDto.Country,
                                Email = b2cCustomerDto.Email,
                                PhoneNumber = b2cCustomerDto.Phone

                            }
                        };

                        jsonString = JsonSerializer.Serialize(customerTokenShippingAddress, options);

                        Console.WriteLine($"TOKEN CREATE FOR {currentTransactionType}:\n {jsonString}");

                        jsonItem = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, false);

                        jsonObject.Add("shippingAddress", jsonItem);

                        ActionTokenTypes.Remove("shippingAddress");

                    }
                    if (b2cCustomerDto is not null && b2cCustomerDto.ActionTokenTypes is not null && ActionTokenTypes.Contains("instrumentIdentifier"))
                    {
                        currentTransactionType = "instrumentIdentifier";
                        string resource = "/tms/v1/instrumentidentifiers";

                        var intstrumentTokenData = new InstrumentTokenData
                        {

                            Card = new FullCard
                            {
                                Number = b2cCustomerDto.AccountNumber,
                            }

                        };

                        
                        jsonString = JsonSerializer.Serialize(intstrumentTokenData, options);

                        Console.WriteLine($"TOKEN CREATE FOR {currentTransactionType}:\n {jsonString}");

                        jsonItem = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, false);

                        instrumentId = (string)jsonItem!["id"]! ?? "null";

                        jsonObject.Add("instrumentIdentifier", jsonItem);

                        ActionTokenTypes.Remove("instrumentIdentifier");
                    }
                    if (b2cCustomerDto is not null && b2cCustomerDto.ActionTokenTypes is not null && ActionTokenTypes.Contains("paymentInstrument"))
                    {
                        currentTransactionType = "paymentInstrument";
                        if (b2cCustomerDto.CustomerInstrumentId is not null)
                        {
                            resource = $"/tms/v2/customers/{b2cCustomerDto.CustomerInstrumentId}/payment-instruments";
                        }
                        else
                        {
                            resource = $"/tms/v2/customers/{customerInstrumentId}/payment-instruments";
                        }

                        string methodId = string.Empty;

                        if (b2cCustomerDto.InstrumentIdentifier is not null)
                        {
                            methodId = b2cCustomerDto.InstrumentIdentifier;
                        }
                        else
                        {
                            methodId = instrumentId;
                        }

                        var customerPaymentInstrument = new CustomerPaymentInstrument
                        {
                            Default = "true",

                            Card = new FullCard
                            {
                                ExpirationMonth = b2cCustomerDto.ExpMonth,
                                ExpirationYear = b2cCustomerDto.ExpYear,
                                Type = b2cCustomerDto.CardType
                            },

                            InstrumentIdentifier = new InstrumentIdentifier
                            {
                                Id = methodId
                            },

                            BillTo = new BillTo
                            {
                                FirstName = b2cCustomerDto.FirstName,
                                LastName = b2cCustomerDto.LastName,
                                Address1 = b2cCustomerDto.Address1,
                                Locality = b2cCustomerDto.City,
                                AdministrativeArea = b2cCustomerDto.AdministrativeArea,
                                PostalCode = b2cCustomerDto.PostalCode,
                                Country = b2cCustomerDto.Country,
                                Email = b2cCustomerDto.Email,
                                PhoneNumber = b2cCustomerDto.Phone

                            }
                        };

                        jsonString = JsonSerializer.Serialize(customerPaymentInstrument, options);

                        Console.WriteLine($"TOKEN CREATE FOR {currentTransactionType}:\n {jsonString}");

                        jsonItem = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, false);

                        jsonObject.Add("paymentInstrument", jsonItem);
                    }
                }
                else if (b2cCustomerDto is not null && b2cCustomerDto.ActionTokenTypes is not null && ActionTokenTypes.Contains("customer"))
                {
                    currentTransactionType = "customer";

                    resource = "/tms/v2/customers";

                    var customerTokenData = new CustomerTokenData
                    {

                        BuyerInformation = new BuyerInformation
                        {
                            MerchantCustomerID = b2cCustomerDto.MerchantCustomerID,
                            Email = b2cCustomerDto.Email,

                        }
                    };

                    jsonString = JsonSerializer.Serialize(customerTokenData, options);

                    Console.WriteLine($"TOKEN CREATE FOR {currentTransactionType}:\n {jsonString}");

                    jsonItem = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, false);

                    jsonObject.Add("customer", jsonItem);

                }
                else if (b2cCustomerDto is not null && b2cCustomerDto.ActionTokenTypes is not null && ActionTokenTypes.Contains("instrumentIdentifier"))
                {
                    currentTransactionType = "instrumentIdentifier";
                    string resource = "/tms/v1/instrumentidentifiers";

                    var intstrumentTokenData = new InstrumentTokenData
                    {

                        Card = new FullCard
                        {
                            Number = b2cCustomerDto.AccountNumber,
                        }

                    };

                    jsonString = JsonSerializer.Serialize(intstrumentTokenData, options);

                    Console.WriteLine($"TOKEN CREATE FOR {currentTransactionType}:\n {jsonString}");

                    jsonItem = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, false);

                    instrumentId = (string)jsonItem!["id"]! ?? "null";

                    jsonObject.Add("instrumentIdentifier", jsonItem);
                }
                else if (b2cCustomerDto is not null && b2cCustomerDto.ActionTokenTypes is not null && ActionTokenTypes.Contains("shippingAddress"))
                {
                    currentTransactionType = "shippingAddress";
                    resource = $"/tms/v2/customers/{b2cCustomerDto.CustomerInstrumentId}/shipping-addresses";

                    var customerTokenShippingAddress = new CustomerTokenShippingAddress
                    {
                        Default = "true",

                        ShipTo = new BillTo
                        {
                            FirstName = b2cCustomerDto.FirstName,
                            LastName = b2cCustomerDto.LastName,
                            Address1 = b2cCustomerDto.Address1,
                            Locality = b2cCustomerDto.City,
                            AdministrativeArea = b2cCustomerDto.AdministrativeArea,
                            PostalCode = b2cCustomerDto.PostalCode,
                            Country = b2cCustomerDto.Country,
                            Email = b2cCustomerDto.Email,
                            PhoneNumber = b2cCustomerDto.Phone

                        }
                    };

                    jsonString = JsonSerializer.Serialize(customerTokenShippingAddress, options);

                    Console.WriteLine($"TOKEN CREATE FOR {currentTransactionType}:\n {jsonString}");

                    jsonItem = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, false);

                    jsonObject.Add("shippingAddress", jsonItem);

                }
                else if (b2cCustomerDto is not null && b2cCustomerDto.ActionTokenTypes is not null && ActionTokenTypes.Contains("paymentInstrument"))
                {
                    currentTransactionType = "paymentInstrument";

                    resource = $"/tms/v2/customers/{b2cCustomerDto.MerchantCustomerID}/payment-instruments";

                    var customerPaymentInstrument = new CustomerPaymentInstrument
                    {
                        Default = "true",

                        Card = new FullCard
                        {
                            ExpirationMonth = b2cCustomerDto.ExpMonth,
                            ExpirationYear = b2cCustomerDto.ExpYear,
                            Type = b2cCustomerDto.CardType
                        },

                        InstrumentIdentifier = new InstrumentIdentifier
                        {
                            Id = b2cCustomerDto.InstrumentIdentifier
                        },

                        BillTo = new BillTo
                        {
                            FirstName = b2cCustomerDto.FirstName,
                            LastName = b2cCustomerDto.LastName,
                            Address1 = b2cCustomerDto.Address1,
                            Locality = b2cCustomerDto.City,
                            AdministrativeArea = b2cCustomerDto.AdministrativeArea,
                            PostalCode = b2cCustomerDto.PostalCode,
                            Country = b2cCustomerDto.Country,
                            Email = b2cCustomerDto.Email,
                            PhoneNumber = b2cCustomerDto.Phone

                        }
                    };

                    jsonString = JsonSerializer.Serialize(customerPaymentInstrument, options);

                    Console.WriteLine($"TOKEN CREATE FOR {currentTransactionType}:\n {jsonString}");

                    jsonItem = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, false);

                    jsonObject.Add("paymentInstrument", jsonItem);

                }
                else
                {
                    jsonError = "NO VALID TOKEN ACTION ITEM";
                    error = "ERROR";
                }

                if (error == "ERROR")
                {
                    jsonString = $"{{ \"Exception\": \"{jsonError}\" }}";
                    JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
                    JsonElement rootElement = jsonDocument.RootElement;
                    jsonObject = JsonObject.Create(rootElement)!;
                    return jsonObject;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                jsonString = $"{{ \"Exception\": \"{e}\" }}";
                JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
                JsonElement rootElement = jsonDocument.RootElement;
                jsonObject = JsonObject.Create(rootElement)!;
                return jsonObject;
            }

            return jsonObject;
        }
    }
}