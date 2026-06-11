using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.OutboundTransObjects;
using CybsClass.Cybersource.Transactions;
using CybsClass.WebApi.Service.Services.JWTProcessing;
using CybsClass.Cybersource.Authentication;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace CybsClass.WebApi.Service.Services.FlexUcContextProcessing
{
    public static class CallForFlexCaptureContext
    {
        public static async Task<string> RunAsyncCaptureContextCreate(B2cCustomerDto b2cCustomerDto)
        {
            string flexToken = string.Empty;
            string resource = "/microform/v2/sessions";
            string deCodedToken = string.Empty;

            try
            {

                var CaptureContext = new CaptureContext
                {

                    TargetOrigins = new List<string> { b2cCustomerDto.TargetOrigin ?? "https://localhost:7133" },

                    AllowedCardNetworks = new string[] { "VISA", "MASTERCARD", "AMEX", "JCB", "DISCOVER", "DINERSCLUB" },

                    AllowedPaymentTypes = new string[] { "CARD" },

                    ClientVersion = "v2",

                };

                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string jsonString = JsonSerializer.Serialize(CaptureContext, options);

                Console.WriteLine("\n************* CALLING FOR FLEX MICROFORM Capture CONTEXT *****\n");

                flexToken = await CallCyberSource.CallCyberSourceCaptureContext(jsonString, resource);

                Console.WriteLine("******************************DECODED JWT **************************");

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
