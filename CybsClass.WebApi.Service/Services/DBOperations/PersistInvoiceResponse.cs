using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.Json;
using CybsClass.EntityModels;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public static class PersistInvoiceResponse
    {
        private static Dictionary<string, string> dbResults = new Dictionary<string, string>();

        private static string InvoiceNumber = string.Empty;

        public static async Task<Dictionary<string, string>> InvoiceDBOps(JsonObject jsonObject)
        {
            try
            {
                dbResults = new();

                Console.WriteLine("Inserting invoice response data ...");

                JsonDocument document = JsonDocument.Parse(jsonObject.ToString());

                if (document.RootElement.TryGetProperty("InvoiceInformation", out JsonElement InvoiceInformation))
                {

                    InvoiceNumber = InvoiceInformation.GetProperty("InvoiceNumber").GetString()!;

                    
                    if (InvoiceNumber is not null)
                    {
                        Console.WriteLine("Updating InvoiceResponse table ...");
                        Console.WriteLine($"Invoice Number: {InvoiceNumber}");

                        using B2cNorthwindContext db = new();

                        InvoiceResponse i = new InvoiceResponse();
                        i.InvoiceNumber = InvoiceNumber;
                        i.TransactionJson = jsonObject.ToString();

                        db.InvoiceResponses.Add(i);

                        dbResults.Add("Invoice Response ID:", i.InvoiceResponseId.ToString());

                        int affected = await db.SaveChangesAsync();
                        dbResults.Add("Affected: InvoiceResponse Added", affected.ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                dbResults = new();
                dbResults.Add("Exception in Token Persistance", ex.Message);
                Console.WriteLine($"Exception: {ex.Message}");
                return dbResults;
            }
            return dbResults;
        }
    }
}





