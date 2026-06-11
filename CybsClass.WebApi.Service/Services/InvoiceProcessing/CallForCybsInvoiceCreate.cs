using CybsClass.Cybersource.Models;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.OutboundTransObjects;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.WebApi.Service.Services.Utilities;
using CybsClass.EntityModels;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace CybsClass.Cybersource.Transactions
{
    public static class CallForCybsInvoiceCreate
    {

        // This class populates the Authorization object for serialization
        public static async Task<JsonObject> RunAsyncJsonObject(B2cCustomerDto b2cCustomerDto)
        {
            JsonObject jsonObject = new JsonObject();
            LineItems LocalLineItem = new LineItems();
            List<LineItems> LineItemsLocal = new List<LineItems>();
            string resource = "/invoicing/v2/invoices";
            DBProductDto product = new();
            string localMinimumPartialAmount = string.Empty;
            bool sendImmediately = false;

            try
            {
                if (b2cCustomerDto!.Cart!.FirstOrDefault() is not null)
                {
                    product = b2cCustomerDto!.Cart!.FirstOrDefault()!;
                }

                if (b2cCustomerDto.InvoiceType == "Draft")
                {
                    sendImmediately = false;
                }
                else if (b2cCustomerDto.InvoiceType == "SendImmediately")
                {
                    sendImmediately = true;
                }


                DateTime parsedDate = DateTime.ParseExact(b2cCustomerDto!.InvoiceInformation!.DueDate!, "M/d/yyyy", null);
                string formattedDate = parsedDate.ToString("yyyy-MM-dd");

                if (b2cCustomerDto != null && b2cCustomerDto.LineItem != null)
                {

                    LocalLineItem.ProductSku = b2cCustomerDto!.LineItem!.ProductSku ?? null;
                    LocalLineItem.ProductName = product.ProductName ?? null;
                    LocalLineItem.Quantity = b2cCustomerDto!.Cart!.Count()!;
                    LocalLineItem.UnitPrice = DecimalTruncate.Truncate(product.UnitPrice, 2);
                    LocalLineItem.DiscountRate = DecimalTruncate.Truncate(b2cCustomerDto.LineItem.DiscountRate, 2);
                    LocalLineItem.TaxRate = DecimalTruncate.Truncate(b2cCustomerDto.LineItem.TaxRate, 2);
                    LocalLineItem.DiscountAmount = DecimalTruncate.Truncate(b2cCustomerDto.LineItem.DiscountAmount, 2);
                    LocalLineItem.TaxAmount = DecimalTruncate.Truncate(b2cCustomerDto.LineItem.TaxAmount, 2);
                    LocalLineItem.TotalAmount = DecimalTruncate.Truncate(b2cCustomerDto.LineItem.TotalAmount, 2);

                }

                LineItemsLocal.Add(LocalLineItem);

                var LineItems = LineItemsLocal;

                var invoiceData = new InvoiceData
                {
                    CustomerInformation = new CustomerInformation
                    {
                        Name = b2cCustomerDto!.FirstName + " " + b2cCustomerDto.LastName,
                        Email = b2cCustomerDto.Email,
                    },
                    InvoiceInformation = new InvoiceInformation
                    {
                        InvoiceNumber = b2cCustomerDto!.InvoiceInformation!.InvoiceNumber,
                        Description = b2cCustomerDto!.InvoiceInformation!.Description,
                        DueDate = formattedDate,
                        SendImmediately = sendImmediately,
                        AllowPartialPayments = b2cCustomerDto.InvoiceInformation.AllowPartialPayments,
                        DeliveryMode = b2cCustomerDto.InvoiceInformation.DeliveryMode,

                    },
                    OrderInformation = new OrderInformation
                    {
                        AmountDetails = new AmountDetails { Currency = "USD", TotalAmount = b2cCustomerDto.TotalAmount?.ToString("0.##") ?? "0.00", MinimumPartialAmount = b2cCustomerDto!.AmountDetails!.MinimumPartialAmount },
                        DiscountAmount = Convert.ToString(b2cCustomerDto!.AmountDetails!.DiscountAmount),
                        DiscountPercent = Convert.ToString(b2cCustomerDto.AmountDetails.DiscountPercent),
                        SubAmount = Convert.ToString(b2cCustomerDto.AmountDetails.SubAmount),
                        //Freight = new Freight { Amount = Convert.ToString(b2cCustomerDto.FreightAmount), Taxable = Convert.ToString(b2cCustomerDto.TaxableFreightAmount) },
                        LineItems = LineItemsLocal,
                    },
                };

                var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull};
                string jsonString = JsonSerializer.Serialize(invoiceData, options);

                Console.WriteLine("\n************* CALLING FOR INVOICE CREATE *****\n");

                jsonObject = await CallCyberSource.CallCyberSourceApiJson(jsonString, resource, false);

                var jsonResponseString = JsonSerializer.Serialize(jsonObject, options);

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR INVOICE CREATE");
                Console.WriteLine(e.Message);
                JsonNode jsonNode = jsonObject;
                jsonNode["Exception"] = "ERROR INVOICE CREATE" + e.Message;
                jsonObject = (JsonObject)jsonNode;
                return jsonObject;
            }

            return jsonObject;

        }
    }
}