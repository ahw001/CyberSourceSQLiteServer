using CybsClass.Cybersource.Transactions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CybsClass.WebApi.Service.Services.TokenProcessing
{
    public static class CallForNtBlob
    {
        public static async Task<string> RunAsyncNtBlobObject(string id)
        {
            string resource = $"/tms/v2/tokens/{id}/payment-credentials";

            try
            {
                Console.WriteLine("\n************* CALLING FOR NETWORK TOKEN BLOB *****\n");

                // /tms/v2/tokens/{id}/payment-credentials is a TMS endpoint that requires
                // MLE JWT authentication — the same as /tms/v2/tokenized-cards.
                // isResponseMle=true so CallCyberSourceApiJsonMle will:
                //   1. Encrypt the request body as a JWE
                //   2. Build the bearer JWT with iss=portfolio and v-c-merchant-id=transacting
                //   3. Decrypt the encryptedResponse using MleCredentials.ResponseMlePrivateKey
                var jsonObject = await CallCyberSource.CallCyberSourceApiJsonMle("{}", resource, isResponseMle: true);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                return JsonSerializer.Serialize(jsonObject, options);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.StackTrace ?? e.Message;
            }
        }
    }
}
