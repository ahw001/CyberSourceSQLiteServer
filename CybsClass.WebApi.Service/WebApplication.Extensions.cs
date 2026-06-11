//using AspNetCoreRateLimit; // To use IClientPolicyStore and so on.
using Microsoft.AspNetCore.Http.HttpResults; // To use Results.
using Microsoft.AspNetCore.Mvc; // To use [FromServices] and so on.
using Microsoft.EntityFrameworkCore;
using CybsClass.Cybersource.Models;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Json;
using CybsClass.Cybersource.Transactions;
using CybsClass.EntityModels; // To use B2cNorthwindContext, Product.
using CybsClass.WebApi.Service.Services.CcTransatcionProcessing;
using CybsClass.WebApi.Service.Services.DBOperations;
using CybsClass.WebApi.Service.Services.TokenProcessing;
using System.Security.Claims; // To use ClaimsPrincipal.
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CybsClass.WebApi.Service;

public static class WebApplicationExtensions
{
    public static void MapGets(this WebApplication app,
      int pageSize = 10)
    {
        app.MapGet("/", () => "Hello World!")
          .ExcludeFromDescription();

        app.MapGet("/api/images/{id:int}", async (int id, B2cNorthwindContext db) =>
        {
            var entity = await db.Categories.FindAsync(id);

            if (entity == null || entity.Picture == null)
            {
                return Results.NotFound();
            }

            return Results.File(entity.Picture, "image/jpeg"); // Adjust MIME Type as needed
        });


        app.MapGet("/secret", (ClaimsPrincipal user) =>
          string.Format("Welcome, {0}. The secret ingredient is love.",
            user.Identity?.Name ?? "secure user"))
          .RequireAuthorization();

        app.MapGet("api/b2ccustomers/{id:int}", (
        [FromServices] B2cNorthwindContext db,
        [FromRoute] int id) =>
        {
            B2cCustomer? b2Customer = db.B2cCustomers.Find(id);
            if (b2Customer == null)
            {
                return Results.NotFound();
            }
            else
            {
                return Results.Json(b2Customer);
            }
        })
        .WithName("GetCustomerById")
        .Produces<B2cCustomer>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        app.MapGet("api/randomcustomer/", (
            [FromServices] B2cNorthwindContext db) =>
            {
                Console.WriteLine("Calling Random Customer!!!!!!!!!");
                int count = db.SampleCustomerData.Count();
                Random r = new Random(count);

                SampleCustomerDatum? c = db.SampleCustomerData.FirstOrDefault(
                    c => c.SampleCustomerId == (int)(EF.Functions.Random() * count));
                if (c == null) return Results.NotFound();


                return Results.Json(c);
            })
            .WithName("GetRandomCustomers")
            .Produces<SampleCustomerDatum>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapGet("api/randomproducts/", (
            [FromServices] B2cNorthwindContext db) =>
                {
                    int count = db.Products.Count();

                    if (count <= 3) { count = count + 3; }

                    Random random = new Random();
                    int min = 1;
                    int max = count;
                    int randomNumber = random.Next(min, max + 1); // Generates a random integer from 1 to 10, inclusive

                    IQueryable<Product> products = db.Products.OrderBy(p => p.ProductId)
                        .Skip(randomNumber).Take(3);

                    return Results.Json(products);
            })
        .WithName("GetRandomProducts")  
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        app.MapGet("api/b2ccustomers", (
          [FromServices] B2cNorthwindContext db,
          [FromQuery] int? page) =>
          db.B2cCustomers
            .OrderBy(cust => cust.B2cCustomerId)
            .Skip(((page ?? 1) - 1) * pageSize)
            .Take(pageSize)
          )
          .WithName("GetB2cCustomers")
          .Produces<B2cCustomer[]>(StatusCodes.Status200OK);


        app.MapGet("api/products", (
          [FromServices] B2cNorthwindContext db,
          [FromQuery] int? page) =>
          db.Products
            .Where(p => (p.UnitsInStock > 0) && (!p.Discontinued))
            .OrderBy(product => product.ProductId)
            .Skip(((page ?? 1) - 1) * pageSize)
            .Take(pageSize)
          )
          .WithName("GetProducts")
          .Produces<Product[]>(StatusCodes.Status200OK);

        app.MapGet("api/products/outofstock",
          ([FromServices] B2cNorthwindContext db) => db.Products
            .Where(p => (p.UnitsInStock == 0) && (!p.Discontinued))
          )
          .WithName("GetProductsOutOfStock")
          .Produces<Product[]>(StatusCodes.Status200OK);

        app.MapGet("api/products/discontinued",
          ([FromServices] B2cNorthwindContext db) =>
            db.Products.Where(product => product.Discontinued)
          )
          .WithName("GetProductsDiscontinued")
          .Produces<Product[]>(StatusCodes.Status200OK);

        app.MapGet("api/products/{id:int}",
          async Task<Results<Ok<Product>, NotFound>> (
          [FromServices] B2cNorthwindContext db,
          [FromRoute] int id) =>
            await db.Products.FindAsync(id) is Product product ?
              TypedResults.Ok(product) : TypedResults.NotFound())
          .WithName("GetProductById")
          .Produces<Product>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status404NotFound);


        app.MapGet("api/products/{name}", (
          [FromServices] B2cNorthwindContext db,
          [FromRoute] string name) =>
            db.Products.Where(p => p.ProductName.Contains(name)))
          .WithName("GetProductsByName")
          .Produces<Product[]>(StatusCodes.Status200OK)
          .RequireCors(policyName: "CybsClass.Mvc.Policy");

        app.MapGet("/api/getorders/{id:int}", async ([FromRoute] int id, B2cNorthwindContext db) =>
        {
            IQueryable<Order>? orders = db.Orders?.Where(o => o.B2cCustomerId == id);

            return orders != null ? Results.Ok(await orders.ToListAsync()) : Results.NotFound();
        })
        .WithName("QueryingOrders");


        app.MapGet("api/samplecards", (
            [FromServices] B2cNorthwindContext db) =>
            Results.Json(db.PaymentCardSampleData))
        .WithName("GetSampleCards")
        .Produces<PaymentCardSampleDatum[]>(StatusCodes.Status200OK);


        app.MapGet("api/getcustomerjson", async (
            [FromServices] B2cNorthwindContext db) =>
            Results.Json(await db.B2cCustomers.ToListAsync()))
        .WithName("GetCustomerJson")
        .Produces<List<B2cCustomer>>(StatusCodes.Status200OK);

        app.MapGet("api/customercount", async () =>
        {
            return Results.Ok(await DBCustomerServices.GetCustomerCountAsync());
        });

        app.MapGet("api/getcustomers", async () =>
        {
            return Results.Ok(await DBCustomerServices.GetB2CCustomers());
        });

        app.MapGet("api/paymentcard/{id:int}", async Task<Results<Ok<PaymentCardInfo>, NotFound>> ([FromRoute] int id, B2cNorthwindContext db) =>
        {
            return await db.PaymentCardInfos.AsNoTracking()
                .FirstOrDefaultAsync(model => model.PaymentCardId == id)
                is PaymentCardInfo model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPaymentCardInfoById");


    }


    public static void MapPosts(this WebApplication app)
    {
        app.MapPost("api/ntdecrypt", async ([FromBody] NtDecodeInstDto ntDeCodeInst,
            [FromServices] CallNtDecrypt callNtDecrypt) =>
        {
            string? InstId = string.Empty;

            Dictionary<string, object> dbResults = new Dictionary<string, object>();

            InstId = ntDeCodeInst.InstrumentId;

            string? decryptedNt = await callNtDecrypt.CallForNtDecrypt(InstId!);

            JsonNode jsonNtNode = JsonNode.Parse(decryptedNt)!;

            // NEED TO ENHANCE ERROR HANDLING HERE

            if (decryptedNt is null)
            {
                Console.WriteLine("NO NETWORK TOKEN NUMBER FOUND");
                await Console.Out.WriteLineAsync("-------------- DB FUNCTIONS SKIPPED!");
            }
            else if ((decryptedNt is not null) && (decryptedNt.Contains("errors", StringComparison.OrdinalIgnoreCase)))
            {

                Console.WriteLine("NO NETWORK TOKEN NUMBER FOUND");
                await Console.Out.WriteLineAsync("-------------- DB FUNCTIONS SKIPPED!");

            }
            else
            {
                //await Console.Out.WriteLineAsync("--------------- Sending to DB functions ... ");

                await Console.Out.WriteLineAsync($"*************** IN NT DECRYPT MIN API: {jsonNtNode.ToString()}");

                jsonNtNode["PaymentCardId"] = ntDeCodeInst.PaymentCardId;
                dbResults = await PersistNtData.InsertNt(jsonNtNode);
                /*
                foreach (var result in dbResults)
                {
                    await Console.Out.WriteLineAsync($"DB Results Key: " + result.Key + " " + "DB Results Value: " + result.Value.ToString());
                }
                */

            }

            return Results.Json(jsonNtNode);

        })
          .Produces<JsonNode>(StatusCodes.Status201Created);

        app.MapPost("api/followontrans", async ([FromBody] FollowOnTransDto followOnTransDto) =>
        {
            string? originalTransId = string.Empty;

            int transActionType = (int)followOnTransDto.FollowOnTransaction.GetValueOrDefault();

            FollowOnTransactions folloOnTransValue = (FollowOnTransactions)transActionType; // Cast the int to the Transaction Type enum

            Dictionary<string, object> dbResults = new Dictionary<string, object>();

            originalTransId = followOnTransDto.OriginalTransactionId;

            if (originalTransId is null && followOnTransDto is not null
                && followOnTransDto.TransactionId is not null)
            {
                originalTransId = followOnTransDto.TransactionId;
            }

            string? amount = followOnTransDto!.TransactionAmount ?? "0";

            FollowOnTransJson? followOnTransJson = new FollowOnTransJson();

            JsonNode followOnTransJsonResponse = await CallForCybsFollowOn.RunAsyncFollowOnJsonObject(originalTransId!, amount!, folloOnTransValue.ToString());

            followOnTransJson = JsonSerializer.Deserialize<FollowOnTransJson>(followOnTransJsonResponse.ToString()!);
            string status = (string)followOnTransJsonResponse!["status"]!;

            if (followOnTransJson is not null)
            {
                dbResults = await PersistFollowOnTransaction.InsertFollowOnTransaction(followOnTransJsonResponse, followOnTransJson, followOnTransDto);

            }
            return Results.Json(followOnTransJsonResponse);

        })
        .Produces<JsonNode>(StatusCodes.Status201Created);

        app.MapPost("api/authtransaction", async ([FromBody] B2cCustomerDto b2cCustomerDto,
            [FromServices] CcAuthService ccAuthService) =>
        {
            JsonObject jsonObject = await ccAuthService.CallForAuth(b2cCustomerDto);

            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            var jsonString = JsonSerializer.Serialize(jsonObject, options);

            JsonNode jsonNode = jsonObject;

            Dictionary<string, object> dbResults = new Dictionary<string, object>();

            string statusNode = (string)jsonNode!["status"]!;

            if (statusNode is not null)
            {
                await Console.Out.WriteLineAsync($"**** STATUS NODE = {statusNode}");

                // NEED MORE ERROR HANDLING HERE FOR TIMEOUTS
                if (statusNode.Contains("INVALID", StringComparison.OrdinalIgnoreCase)
                    || statusNode.Contains("DECLINED", StringComparison.OrdinalIgnoreCase)
                    || statusNode.Contains("ERROR", StringComparison.OrdinalIgnoreCase))
                {

                    Console.WriteLine("INVALID_REQUEST");
                    await Console.Out.WriteLineAsync("-------------- DB FUNCTIONS SKIPPED!");
                }
                else
                {
                    await Console.Out.WriteLineAsync("--------------- Sending to DB functions ... ");
                    dbResults = await PersistCustomerData.InsertCustomers(b2cCustomerDto, jsonNode);
                    /*
                    foreach (var result in dbResults)
                    {
                        await Console.Out.WriteLineAsync($"DB Results Key: " + result.Key + " " + "DB Results Value: " + result.Value.ToString());
                    }
                    */
                    string payCardId = (string)dbResults["PaymentCardId"];
                    int orderId = (int)dbResults["OrderId"];
                    int b2cCustomerId = (int)dbResults["B2cCustomerId"];

                    jsonNode["B2cCustomerId"] = b2cCustomerId;
                    jsonNode["PaymentCardId"] = payCardId.ToString();
                    jsonNode["OrderId"] = orderId.ToString();

                }
            }
            else if (jsonString.Contains("errors", StringComparison.OrdinalIgnoreCase))
            {
                return Results.Json(jsonNode);
            }

            return Results.Json(jsonNode);

        })
        .Produces<JsonNode>(StatusCodes.Status201Created);


        app.MapPost("api/processtms", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            //************ CallCyberSource for Token Create

            JsonObject jsonObject = await CallForCybsAuthTokenCreate.RunAsyncJsonObject(b2cCustomerDto);

            //************ CallCybersource for Token Create

            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
            var jsonString = JsonSerializer.Serialize(jsonObject, options);

            Dictionary<string, string> dbResults = new Dictionary<string, string>();

            JsonNode jsonNode = jsonObject;

            if (jsonObject != null)
            {
                try
                {
                    int customerId = b2cCustomerDto.B2cCustomerId;
                    string statusNode = (string)jsonNode!["status"]!;
                    string id = (string)jsonNode!["id"]!;
                    await Console.Out.WriteLineAsync($"**** STATUS NODE = {id}");

                    await Console.Out.WriteLineAsync("--------------- Sending to DB functions ... ");
                    dbResults = await PersistCybsTokenData.TokenDBOps(customerId, jsonObject);
                    /*
                    foreach (var result in dbResults)
                    {
                        await Console.Out.WriteLineAsync($"DB Results Key: " + result.Key + " " + "DB Results Value: " + result.Value.ToString());
                    }
                    */

                    string payCardId = (string)dbResults["PaymentCardId"];

                    jsonNode["PaymentCardId"] = payCardId;
                    return Results.Json(jsonNode);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EXCEPTION: " + ex.Message);
                    jsonNode["Exception"] = ex.Message;
                    return Results.Json(jsonNode);
                }

            }
            else if (jsonString.Contains("errors", StringComparison.OrdinalIgnoreCase))
            {

                Console.WriteLine("INVALID_REQUEST");
                await Console.Out.WriteLineAsync("-------------- DB FUNCTIONS SKIPPED!");
                jsonNode["Exception"] = jsonString;
                return Results.Json(jsonNode);
            }
            else
            {
                Console.WriteLine("ERROR IN PROCESSING");
                await Console.Out.WriteLineAsync("-------------- DB FUNCTIONS SKIPPED!");
                jsonNode["Exception"] = "UNKNOWN ERROR IN PROCESSING";
                return Results.Json(jsonNode);
            }
        })
        .Produces<JsonNode>(StatusCodes.Status201Created);

        app.MapPost("api/standalonecredit", async ([FromBody] B2cCustomerDto b2cCustomerDto) =>
        {
            JsonObject jsonObject = await CallForCybsStandAloneCredit.RunAsyncJsonObject(b2cCustomerDto);
            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            var jsonString = JsonSerializer.Serialize(jsonObject, options);

            JsonNode jsonNode = jsonObject;

            Dictionary<string, object> dbResults = new Dictionary<string, object>();

            string statusNode = (string)jsonNode!["status"]!;

            if (statusNode is not null)
            {
                await Console.Out.WriteLineAsync($"**** STATUS NODE = {statusNode}");

                if (statusNode.Contains("INVALID") || statusNode.Contains("DECLINED"))
                {

                    Console.WriteLine("INVALID_REQUEST");
                    await Console.Out.WriteLineAsync("-------------- DB FUNCTIONS SKIPPED!");
                }
                else
                {
                    await Console.Out.WriteLineAsync("--------------- Sending to DB functions ... ");
                    dbResults = await PersistStandAloneCredit.InsertStandAloneCredit(b2cCustomerDto, jsonNode);
                    /*
                    foreach (var result in dbResults)
                    {
                        await Console.Out.WriteLineAsync($"DB Results Key: " + result.Key + " " + "DB Results Value: " + result.Value.ToString());
                    }
                    */
                    string? standAloneCreditID = Convert.ToString(dbResults["StandAloneCreditId"]);
                    //int orderId = (int)dbResults["OrderId"];


                    jsonNode["StandAloneCreditId"] = standAloneCreditID;

                }
            }

            return Results.Json(jsonNode);

        })
        .Produces<JsonNode>(StatusCodes.Status201Created);

        app.MapPost("api/products", async ([FromBody] Product product,
          [FromServices] B2cNorthwindContext db) =>
        {
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return Results.Created($"api/products/{product.ProductId}", product);
        })
          .Produces<Product>(StatusCodes.Status201Created);

    }

    public static void MapPuts(this WebApplication app)
    {
        app.MapPut("api/products/{id:int}", async (
          [FromRoute] int id,
          [FromBody] Product product,
          [FromServices] B2cNorthwindContext db) =>
        {
            Product? foundProduct = await db.Products.FindAsync(id);

            if (foundProduct is null) return Results.NotFound();

            foundProduct.ProductName = product.ProductName;
            foundProduct.CategoryId = product.CategoryId;
            foundProduct.SupplierId = product.SupplierId;
            foundProduct.QuantityPerUnit = product.QuantityPerUnit;
            foundProduct.UnitsInStock = product.UnitsInStock;
            foundProduct.UnitsOnOrder = product.UnitsOnOrder;
            foundProduct.ReorderLevel = product.ReorderLevel;
            foundProduct.UnitPrice = product.UnitPrice;
            foundProduct.Discontinued = product.Discontinued;

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
          .Produces(StatusCodes.Status404NotFound)
          .Produces(StatusCodes.Status204NoContent);
    }

    public static void MapDeletes(this WebApplication app)
    {
        app.MapDelete("api/products/{id:int}", async (
          [FromRoute] int id,
          [FromServices] B2cNorthwindContext db) =>
        {
            if (await db.Products.FindAsync(id) is Product product)
            {
                db.Products.Remove(product);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.NotFound();
        })
          .Produces(StatusCodes.Status404NotFound)
          .Produces(StatusCodes.Status204NoContent);
    }

}
