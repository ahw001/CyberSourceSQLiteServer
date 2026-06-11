using CybsClass.Cybersource.Authentication;
using CybsClass.Cybersource.Models;
using CybsClass.Cybersource.Transactions;
using System.Text.Json.Nodes;


namespace CybsClass.WebApi.Service.Services.TokenProcessing
{
    public class CallNtDecrypt
    {
        public async Task<string> CallForNtDecrypt(string instId)
        {
            // CallForNtBlob now uses CallCyberSourceApiJsonMle which handles MLE JWT
            // authentication and decrypts the encryptedResponse internally.
            // The string returned is already plaintext JSON — no further decryption needed.
            string ntResponse = await CallForNtBlob.RunAsyncNtBlobObject(instId);
            return ntResponse;
        }
    }
}
