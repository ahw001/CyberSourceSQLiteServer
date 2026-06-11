using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Json;
using CybsClass.Cybersource.Transactions;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace CybsClass.WebApi.Service.Services.PayPal
{
    public static class CallForCybsPayPalCreateOrder
    {
        public static async Task<JsonObject> RunAsyncJsonObject(B2cCustomerDto dto)
        {
            JsonSerialOptions options = new JsonSerialOptions();

            var resource = "/pts/v2/intents";

            var createOrder = new PayPalCreateOrderData
            {
                ClientReferenceInformation = new ClientReferenceInformation
                {
                    Code = dto.MerchantReferenceCode,
                },

                MerchantInformation = new PayPalMerchantInformation
                {
                    MerchantDescriptor = new MerchantDescriptor
                    {
                        Name = "My Store"
                    },
                    ReturnUrl = dto.PayPalTransactionDetails?.PayPalReturnUrl ?? "",
                    SuccessUrl = dto.PayPalTransactionDetails?.PayPalSuccessUrl ?? ""
                },

                ProcessingInformation = new PayPalProcessingInformation
                {
                    AuthorizationOptions = new PayPalAuthorizationOptions
                    {
                        AuthType = "CAPTURE"
                    }
                },

                PaymentInformation = new PayPalPaymentInformation
                {
                    PaymentType = new PayPalPaymentType
                    {
                        Method = new PayPalMethod
                        {
                            Name = "payPal"
                        }
                    }
                },

                OrderInformation = new OrderInformation
                {
                    AmountDetails = new AmountDetails
                    {
                        Currency = "USD",
                        TotalAmount = (dto.TotalAmount ?? 0).ToString("0.00")
                    },
                    BillTo = new BillTo
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Address1 = dto.Address1,
                        Locality = dto.City,
                        AdministrativeArea = dto.AdministrativeArea,
                        PostalCode = dto.PostalCode,
                        Country = "US",
                        Email = dto.Email,
                        PhoneNumber = dto.Phone
                    }
                }
            };

            string jsonString = JsonSerializer.Serialize(createOrder, options.SerializerOptions);



            return await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, false);
        }
    }
}