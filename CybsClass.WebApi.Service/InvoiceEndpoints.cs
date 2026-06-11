using Microsoft.AspNetCore.Mvc;
using CybsClass.Cybersource.Transactions;
using System.Text.Json.Nodes;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.WebApi.Service.Services.DBOperations;

namespace CybsClass.WebApi.Service
{
    public static class InvoiceEndpoints
    {
        public static RouteGroupBuilder GroupInvoiceEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("/createinvoice", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
            {
                Dictionary<string, string> dBResults = new();

                JsonObject invoiceResponse = await CallForCybsInvoiceCreate.RunAsyncJsonObject(b2cCustomerDto);

                if (invoiceResponse is null)
                {
                    return Results.NotFound();
                }
                else if (invoiceResponse is not null)
                {
                    if (invoiceResponse.ToString().Contains("error", StringComparison.OrdinalIgnoreCase) ||
                    invoiceResponse.ToString().Contains("INVALID", StringComparison.OrdinalIgnoreCase))
                    {
                        return Results.Ok(invoiceResponse);
                    }
                    else
                    {
                        dBResults = await PersistInvoiceResponse.InvoiceDBOps(invoiceResponse);
                    }
                }

                return Results.Ok(invoiceResponse);
            }).Produces<JsonObject>().WithName("CreateInvoice");

            return group;
        }
    }
}
