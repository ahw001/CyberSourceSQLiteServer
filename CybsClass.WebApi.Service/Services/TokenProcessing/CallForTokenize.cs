using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Transactions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CybsClass.WebApi.Service.Services.TokenProcessing
{
    public static class CallForTokenize
    {
        public static async Task<JsonObject> RunAsync(B2cCustomerDto dto)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            Console.WriteLine($"\n[CallForTokenize] INBOUND DTO:\n{JsonSerializer.Serialize(dto, options)}");

            try
            {
                // Shipping falls back to billing when not explicitly provided
                string shipFn = string.IsNullOrWhiteSpace(dto.ShippingFirstName) ? dto.FirstName : dto.ShippingFirstName;
                string shipLn = string.IsNullOrWhiteSpace(dto.ShippingLastName) ? dto.LastName : dto.ShippingLastName;
                string? shipAddr = string.IsNullOrWhiteSpace(dto.ShippingAddress1) ? dto.Address1 : dto.ShippingAddress1;
                string? shipCity = string.IsNullOrWhiteSpace(dto.ShippingCity) ? dto.City : dto.ShippingCity;
                string? shipState = string.IsNullOrWhiteSpace(dto.ShippingAdministrativeArea) ? dto.AdministrativeArea : dto.ShippingAdministrativeArea;
                string? shipZip = string.IsNullOrWhiteSpace(dto.ShippingPostalCode) ? dto.PostalCode : dto.ShippingPostalCode;
                string? shipCountry = string.IsNullOrWhiteSpace(dto.ShippingCountry) ? dto.Country : dto.ShippingCountry;
                string? shipEmail = string.IsNullOrWhiteSpace(dto.ShippingEmail) ? dto.Email : dto.ShippingEmail;
                string? shipPhone = string.IsNullOrWhiteSpace(dto.ShippingPhone) ? dto.Phone : dto.ShippingPhone;

                var request = new TokenizeV2Request
                {
                    ProcessingInformation = new TokenizeV2ProcessingInfo
                    {
                        ActionList = new[] { "TOKEN_CREATE" },
                        ActionTokenTypes = new[] { "customer", "shippingAddress", "paymentInstrument", "instrumentIdentifier" }
                    },
                    TokenInformation = new TokenizeV2TokenInfo
                    {
                        Customer = new TokenizeV2CustomerInfo
                        {
                            BuyerInformation = new TokenizeV2BuyerInfo
                            {
                                MerchantCustomerID = string.IsNullOrWhiteSpace(dto.MerchantCustomerID)
                                    ? (dto.B2cCustomerId > 0 ? dto.B2cCustomerId.ToString() : Guid.NewGuid().ToString())
                                    : dto.MerchantCustomerID,
                                Email = dto.Email
                            },
                            ClientReferenceInformation = new TokenizeV2ClientRef
                            {
                                Code = dto.MerchantReferenceCode ?? "TC50171_3"
                            },
                            MerchantDefinedInformation = new List<TokenizeV2MerchantDefinedInfo>
                            {
                                new() { Name = "data1", Value = dto.B2cCustomerId > 0 ? dto.B2cCustomerId.ToString() : "customer data" }
                            }
                        },
                        ShippingAddress = new TokenizeV2ShippingAddress
                        {
                            Default = "true",
                            ShipTo = new TokenizeV2ShipTo
                            {
                                FirstName = shipFn,
                                LastName = shipLn,
                                Company = dto.CompanyName,
                                Address1 = shipAddr,
                                Locality = shipCity,
                                AdministrativeArea = shipState,
                                PostalCode = shipZip,
                                Country = shipCountry,
                                Email = shipEmail,
                                PhoneNumber = shipPhone
                            }
                        },
                        PaymentInstrument = new TokenizeV2PaymentInstrument
                        {
                            Default = "true",
                            Card = new TokenizeV2Card
                            {
                                ExpirationMonth = dto.ExpMonth,
                                ExpirationYear = dto.ExpYear,
                                Type = dto.CardType
                            },
                            BillTo = new TokenizeV2BillTo
                            {
                                FirstName = dto.FirstName,
                                LastName = dto.LastName,
                                Company = dto.CompanyName,
                                Address1 = dto.Address1,
                                Locality = dto.City,
                                AdministrativeArea = dto.AdministrativeArea,
                                PostalCode = dto.PostalCode,
                                Country = dto.Country,
                                Email = dto.Email,
                                PhoneNumber = dto.Phone
                            }
                        },
                        InstrumentIdentifier = new TokenizeV2InstrumentIdentifier
                        {
                            Type = "enrollable card",
                            Card = new TokenizeV2IdentifierCard
                            {
                                Number = dto.AccountNumber,
                                ExpirationMonth = dto.ExpMonth,
                                ExpirationYear = dto.ExpYear
                            }
                        }
                    }
                };

                string jsonString = JsonSerializer.Serialize(request, options);
                Console.WriteLine($"\n[CallForTokenize] REQUEST TO CYBERSOURCE:\n{jsonString}");



                var jsonObject = await CallCyberSource.CallCyberSourceApiJsonMle(jsonString, "/tms/v2/tokenize");

                Console.WriteLine($"\n[CallForTokenize] RESPONSE FROM CYBERSOURCE:\n{JsonSerializer.Serialize(jsonObject, options)}");

                return jsonObject;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                string exJson = $"{{\"Exception\":\"{e.Message}\"}}";
                JsonDocument doc = JsonDocument.Parse(exJson);
                return JsonObject.Create(doc.RootElement)!;
            }
        }
    }
}
