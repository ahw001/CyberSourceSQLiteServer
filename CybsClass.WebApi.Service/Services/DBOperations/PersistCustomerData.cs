using Microsoft.EntityFrameworkCore.ChangeTracking;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Json;
using CybsClass.EntityModels;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public static class PersistCustomerData
    {
        private static B2cCustomer b2CCustomer = new B2cCustomer();
        private static Dictionary<string, object> dbResults = new Dictionary<string, object>();

        public static async Task<Dictionary<string, object>> InsertCustomers(B2cCustomerDto b2cCustomerDto, JsonNode authTransNode)
        {
            dbResults = new();

            Console.WriteLine("Inserting customer data ...");
            try
            {
                int orderDetailInsert = 0;

                AuthTransResponseJson authTransResponseJson = new AuthTransResponseJson();

                authTransResponseJson = JsonSerializer.Deserialize<AuthTransResponseJson>(authTransNode.ToString())!;

                using B2cNorthwindContext db = new();

                //if (db.B2cCustomers is null) return (0);

                B2cCustomer c = new();

                c.FirstName = b2cCustomerDto.FirstName!;
                c.LastName = b2cCustomerDto.LastName!;
                c.Phone = b2cCustomerDto.Phone!;
                c.Email = b2cCustomerDto.Email!;
                c.Address1 = b2cCustomerDto.Address1!;
                //c.Address2 = Address2;
                c.City = b2cCustomerDto.City!;
                c.Region = b2cCustomerDto.AdministrativeArea!;
                c.PostalCode = b2cCustomerDto.PostalCode!;
                c.Country = b2cCustomerDto.Country!;


                var paymentCardInfo = new PaymentCardInfo();

                paymentCardInfo.AccountNumber = b2cCustomerDto.AccountNumber!;
                paymentCardInfo.ExpMonth = b2cCustomerDto.ExpMonth!;
                paymentCardInfo.ExpYear = b2cCustomerDto.ExpYear!;
                paymentCardInfo.Cvv = b2cCustomerDto.Cvv!;
                if (authTransResponseJson.TokenInformation is not null)
                {
                    paymentCardInfo.InstrumentIdentifierId = authTransResponseJson.TokenInformation?.
                        InstrumentIdentifier?.Id! ?? "0";
                    paymentCardInfo.InstrumentIdentifierState = authTransResponseJson.TokenInformation?.
                        InstrumentIdentifier?.State! ?? "0";
                    paymentCardInfo.InstrumentidentifierNew = Convert.ToString(authTransResponseJson.TokenInformation?.InstrumentIdentifierNew!) ?? "0";
                    paymentCardInfo.PaymentInstrumentId = authTransResponseJson.TokenInformation?
                        .PaymentInstrument?.Id! ?? "0";
                    paymentCardInfo.PaymentAccountReferenceNumber = authTransResponseJson.ProcessorInformation?
                        .PaymentAccountReferenceNumber! ?? "0";
                    paymentCardInfo.ResponseTransactionJson = authTransNode.ToString() ?? "0";
                }


                c.PaymentCardInfos.Add(paymentCardInfo);

                EntityEntry<B2cCustomer> entity = db.B2cCustomers.Add(c);
                Console.WriteLine($"B2cCustomer State: {entity.State}, B2cCustomerId: {c.B2cCustomerId}");

                int affected0 = await db.SaveChangesAsync();
                Console.WriteLine($"B2cCustomer State: {entity.State}, B2cCustomerId: {c.B2cCustomerId}");
                dbResults.Add("B2cCustomerId", c.B2cCustomerId);

                var pcInfo = entity.Entity.PaymentCardInfos.LastOrDefault();

                int? payCardInfo = pcInfo!.PaymentCardId;

                dbResults.Add("PaymentCardId", Convert.ToString(payCardInfo)!);

                Order o = new();

                o.B2cCustomerId = c.B2cCustomerId;
                o.OrderDate = DateTime.Now;

                EntityEntry<Order> orderEntity = db.Orders.Add(o);
                //Console.WriteLine($"Order State: {orderEntity.State}, OrderId: {o.OrderId}");


                int affected1 = db.SaveChanges();
                //Console.WriteLine($"Order State: {orderEntity.State}, OrderId: {o.OrderId}");

                dbResults.Add("OrderId", o.OrderId);

                if (b2cCustomerDto is not null && b2cCustomerDto.Cart is not null)
                {
                    foreach (var product in b2cCustomerDto.Cart)
                    {
                        var orderDetails = new OrderDetail();
                        orderDetails.OrderId = o.OrderId;
                        orderDetails.ProductId = product.ProductId;
                        orderDetails.Quantity = 1;
                        orderDetails.UnitPrice = product.UnitPrice ?? 0m;

                        EntityEntry<OrderDetail> detailEntity = db.OrderDetails.Add(orderDetails);
                        //Console.WriteLine($"Order Detail State: {entity.State}, OrderId: {orderDetails.OrderId}");

                        int affected2 = db.SaveChanges();
                        orderDetailInsert++;
                        //Console.WriteLine($"Order Detail State: {entity.State}, OrderId: {o.OrderId}");
                    }
                }

                Console.WriteLine("Inserting auth transaction data ...");

                AuthTransResponse authDb = new AuthTransResponse();

                authDb.OrderId = o.OrderId!;
                authDb.Id = authTransResponseJson.Id!;
                authDb.Status = authTransResponseJson.Status;
                authDb.SubmitTimeUtc = authTransResponseJson.SubmitTimeUtc!;
                authDb.ReconciliationId = authTransResponseJson.ReconciliationId!;

                if (authTransResponseJson.OrderInformation is not null && authTransResponseJson.OrderInformation?.AmountDetails is not null)
                {
                    authDb.AuthorizedAmount = Convert.ToDecimal(authTransResponseJson.OrderInformation?.AmountDetails?.AuthorizedAmount);
                    //Console.WriteLine($"authTransResponseJson.OrderInformation.AuthAmount is NOT null: {authDb.AuthorizedAmount}");
                }
                else
                {
                    //Console.WriteLine("authTransResponseJson.OrderInformation.AuthAmount is NULL");
                    authDb.AuthorizedAmount = 0.00M;
                }

                //Change to JSON Node then ToString()
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(b2CCustomer, options);
                JsonNode jsonLinks = JsonNode.Parse(jsonString)!;

                authDb.Links = authTransNode!["_links"]!.ToString()!;

                authDb.ProcInfoApprovalCode = authTransResponseJson.ProcessorInformation?.ApprovalCode ?? "0";
                authDb.ProcInfoNetworkTransactionId = authTransResponseJson.ProcessorInformation?.NetworkTransactionId ?? "0";
                authDb.ProcInfoResponseCode = authTransResponseJson.ProcessorInformation?.ResponseCode ?? "0";
                authDb.ProcInfoTransactionId = authTransResponseJson.ProcessorInformation?.TransactionId ?? "0";

                authDb.TokenInformationInstId = authTransResponseJson.TokenInformation?.InstrumentIdentifier?.Id ?? "0";
                authDb.TokenInformationInstIdNew = Convert.ToString(authTransResponseJson.TokenInformation?.InstrumentIdentifierNew) ?? "0";

                EntityEntry<AuthTransResponse> authEntity = db.AuthTransResponses.Add(authDb);
                //Console.WriteLine($"Auth Trans State: {entity.State}, ID: {authDb.AuthTransResponsesId}");

                int affected3 = db.SaveChanges();
                //Console.WriteLine($"Auth Trans State: {entity.State}, ID: {authDb.AuthTransResponsesId}");

                return dbResults;
            }
            catch (Exception ex)
            {
                dbResults = new();
                dbResults.Add("Exception", ex.Message);
                Console.WriteLine($"Exception: {ex.Message}");
                return dbResults;
            }
        }
    }
}
