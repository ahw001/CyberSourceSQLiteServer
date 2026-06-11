using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Runtime.InteropServices;
using CybsClass.Cybersource.Models.OutboundTransObjects;
using CybsClass.Cybersource.Transactions;
using CybsClass.WebApi.Service.Services.JWTProcessing;
using CybsClass.Cybersource.Models.BaseData;
using System.Text.Json.Serialization;



namespace CybsClass.WebApi.Service.Services.FlexUcContextProcessing
{
    public static class CallForFlexTokenApi
    {
        public static async Task<string> RunAsyncFlexApiTokenCreate()
        {
            string flexToken = string.Empty;
            string resource = "/microform/v2/sessions";
            string deCodedToken = string.Empty;

            try
            {
                var flexApiData = new FlexApiData
                {
                    Fields = new FlexFields
                    {
                        PaymentInformation = new PaymentInformationFlex
                        {
                            Card = new FlexCard
                            {
                                Number = "",

                                ExpirationMonth = new ExpirationMonth
                                {
                                    Required = false
                                },
                                ExpirationYear = new ExpirationYear
                                {
                                    Required = false
                                },
                                SecurityCode = new SecurityCode
                                {
                                    Required = false
                                },
                            }
                        }
                    },
                };


                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(flexApiData, options);

                Console.WriteLine("\n************* CALLING FOR FLEX TOKEN *****\n");
                Console.WriteLine($"FLEX INPUT FOR CAPTURE CONTEXT: {jsonString}");

                flexToken = await CallCyberSource.CallCyberSourceAPIFlex(jsonString, resource);

                Console.WriteLine("******************************DECODED JWT **************************\n");

                deCodedToken = JWTDeCode.DeCodeJWT(flexToken);

                Console.WriteLine(deCodedToken);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }

            return flexToken;
        }
    }
}
