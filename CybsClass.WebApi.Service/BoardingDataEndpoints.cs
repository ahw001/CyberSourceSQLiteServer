using Microsoft.AspNetCore.Mvc;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.WebApi.Service.Services.DBOperations;
using System.Text.Json;

namespace CybsClass.WebApi.Service;

public static class BoardingDataEndpoints
{
    private static readonly JsonSerializerOptions _logOptions =
        new(JsonSerializerDefaults.Web) { WriteIndented = true };

    public static void MapBoardingDataEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/boardingdata").WithTags(nameof(BoardingDataEndpoints));

        // =====================================================================
        // ORGANIZATION
        // =====================================================================

        group.MapGet("/organizations", async () =>
        {
            Console.WriteLine("\n[BoardingData] GET /organizations");
            var orgs = await DBBoardingOrganizationServices.GetAllAsync();
            var dtos = orgs.Select(DBBoardingOrganizationServices.ToDto).ToList();
            Console.WriteLine($"\n[BoardingData] RESPONSE organizations count={dtos.Count}");
            return Results.Json(dtos);
        })
        .WithName("GetAllBoardingOrganizations");

        group.MapGet("/organization/{id:int}", async ([FromRoute] int id) =>
        {
            Console.WriteLine($"\n[BoardingData] GET /organization/{id}");
            var org = await DBBoardingOrganizationServices.GetByIdAsync(id);
            if (org is null)
            {
                var err = BuildError("Not Found", $"Organization {id} not found.", "Check the ID.");
                Console.WriteLine($"\n\n[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(err, _logOptions)}");
                return Results.Json(err);
            }
            var dto = DBBoardingOrganizationServices.ToDto(org);
            Console.WriteLine($"\n\n[BoardingData] RESPONSE: {JsonSerializer.Serialize(dto, _logOptions)}");
            return Results.Json(dto);
        })
        .WithName("GetBoardingOrganizationById");

        group.MapPost("/organization", async ([FromBody] BoardingOrganizationDto dto) =>
        {
            Console.WriteLine($"\n\n[BoardingData] POST /organization REQUEST: {JsonSerializer.Serialize(dto, _logOptions)}");
            var entity = await DBBoardingOrganizationServices.CreateAsync(dto);
            if (entity is null)
            {
                var err = BuildError("Database Error", "Failed to create organization.", "Check server logs.");
                Console.WriteLine($"\n\n[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(err, _logOptions)}");
                return Results.Json(err);
            }
            var response = DBBoardingOrganizationServices.ToDto(
                (await DBBoardingOrganizationServices.GetByIdAsync(entity.BoardingOrganizationId))!);
            Console.WriteLine($"\n\n[BoardingData] RESPONSE: {JsonSerializer.Serialize(response, _logOptions)}");
            return Results.Json(response);
        })
        .WithName("CreateBoardingOrganization");

        group.MapPut("/organization/{id:int}", async ([FromRoute] int id, [FromBody] BoardingOrganizationDto dto) =>
        {
            Console.WriteLine($"\n\n[BoardingData] PUT /organization/{id} REQUEST: {JsonSerializer.Serialize(dto, _logOptions)}");
            var entity = await DBBoardingOrganizationServices.UpdateAsync(id, dto);
            if (entity is null)
            {
                var err = BuildError("Not Found", $"Organization {id} not found.", "Check the ID.");
                Console.WriteLine($"\n\n[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(err, _logOptions)}");
                return Results.Json(err);
            }
            var response = DBBoardingOrganizationServices.ToDto(
                (await DBBoardingOrganizationServices.GetByIdAsync(entity.BoardingOrganizationId))!);
            Console.WriteLine($"\n\n[BoardingData] RESPONSE: {JsonSerializer.Serialize(response, _logOptions)}");
            return Results.Json(response);
        })
        .WithName("UpdateBoardingOrganization");

        group.MapDelete("/organization/{id:int}", async ([FromRoute] int id) =>
        {
            Console.WriteLine($"\n\n[BoardingData] DELETE /organization/{id}");
            bool deleted = await DBBoardingOrganizationServices.DeleteAsync(id);
            if (!deleted)
            {
                var err = BuildError("Not Found", $"Organization {id} not found or could not be deleted.", "Check the ID.");
                return Results.Json(err);
            }
            Console.WriteLine($"\n\n[BoardingData] DELETE /organization/{id} success");
            return Results.Json(new { deleted = true, boardingOrganizationId = id });
        })
        .WithName("DeleteBoardingOrganization");

        // =====================================================================
        // TRANSACTING MERCHANT
        // =====================================================================

        group.MapGet("/transactingmerchants", async () =>
        {
            Console.WriteLine("\n\n[BoardingData] GET /transactingmerchants");
            var all = await DBBoardingTransactingMerchantServices.GetAllAsync();
            var dtos = all.Select(DBBoardingTransactingMerchantServices.ToDto).ToList();
            Console.WriteLine($"[BoardingData] RESPONSE transacting count={dtos.Count}");
            return Results.Json(dtos);
        })
        .WithName("GetAllBoardingTransactingMerchants");

        group.MapGet("/transactingmerchants/byorg/{orgId:int}", async ([FromRoute] int orgId) =>
        {
            Console.WriteLine($"\n\n[BoardingData] GET /transactingmerchants/byorg/{orgId}");
            var all = await DBBoardingTransactingMerchantServices.GetByOrgIdAsync(orgId);
            var dtos = all.Select(DBBoardingTransactingMerchantServices.ToDto).ToList();
            Console.WriteLine($"[\n\nBoardingData] RESPONSE transacting by org count={dtos.Count}");
            return Results.Json(dtos);
        })
        .WithName("GetBoardingTransactingMerchantsByOrg");

        group.MapGet("/transactingmerchant/{id:int}", async ([FromRoute] int id) =>
        {
            Console.WriteLine($"\n\n[BoardingData] GET /transactingmerchant/{id}");
            var entity = await DBBoardingTransactingMerchantServices.GetByIdAsync(id);
            if (entity is null)
            {
                var err = BuildError("Not Found", $"Transacting merchant {id} not found.", "Check the ID.");
                return Results.Json(err);
            }
            var dto = DBBoardingTransactingMerchantServices.ToDto(entity);
            Console.WriteLine($"\n\n[BoardingData] RESPONSE: {JsonSerializer.Serialize(dto, _logOptions)}");
            return Results.Json(dto);
        })
        .WithName("GetBoardingTransactingMerchantById");

        group.MapPost("/transactingmerchant", async ([FromBody] BoardingTransactingMerchantDto dto) =>
        {
            Console.WriteLine($"\n\n[BoardingData] POST /transactingmerchant REQUEST: {JsonSerializer.Serialize(dto, _logOptions)}");
            var entity = await DBBoardingTransactingMerchantServices.CreateAsync(dto);
            if (entity is null)
            {
                var err = BuildError("Database Error", "Failed to create transacting merchant.", "Check server logs.");
                Console.WriteLine($"\n\n[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(err, _logOptions)}");
                return Results.Json(err);
            }
            var response = DBBoardingTransactingMerchantServices.ToDto(
                (await DBBoardingTransactingMerchantServices.GetByIdAsync(entity.BoardingTransactingMerchantId))!);
            Console.WriteLine($"[BoardingData] RESPONSE: {JsonSerializer.Serialize(response, _logOptions)}");
            return Results.Json(response);
        })
        .WithName("CreateBoardingTransactingMerchant");

        group.MapPut("/transactingmerchant/{id:int}", async ([FromRoute] int id, [FromBody] BoardingTransactingMerchantDto dto) =>
        {
            Console.WriteLine($"\n\n[BoardingData] PUT /transactingmerchant/{id} REQUEST: {JsonSerializer.Serialize(dto, _logOptions)}");
            var entity = await DBBoardingTransactingMerchantServices.UpdateAsync(id, dto);
            if (entity is null)
            {
                var err = BuildError("Not Found", $"Transacting merchant {id} not found.", "Check the ID.");
                return Results.Json(err);
            }
            var response = DBBoardingTransactingMerchantServices.ToDto(
                (await DBBoardingTransactingMerchantServices.GetByIdAsync(entity.BoardingTransactingMerchantId))!);
            Console.WriteLine($"\n\n[BoardingData] RESPONSE: {JsonSerializer.Serialize(response, _logOptions)}");
            return Results.Json(response);
        })
        .WithName("UpdateBoardingTransactingMerchant");

        group.MapDelete("/transactingmerchant/{id:int}", async ([FromRoute] int id) =>
        {
            Console.WriteLine($"\n\n[BoardingData] DELETE /transactingmerchant/{id}");
            bool deleted = await DBBoardingTransactingMerchantServices.DeleteAsync(id);
            if (!deleted)
            {
                var err = BuildError("Not Found", $"Transacting merchant {id} not found or could not be deleted.", "Check the ID.");
                return Results.Json(err);
            }
            Console.WriteLine($"\n\n[BoardingData] DELETE /transactingmerchant/{id} success");
            return Results.Json(new { deleted = true, boardingTransactingMerchantId = id });
        })
        .WithName("DeleteBoardingTransactingMerchant");

        // =====================================================================
        // CARD PRODUCT SUBSCRIPTIONS
        // =====================================================================

        // All subscriptions — used by CardProcessingConfigForm to populate dropdown
        group.MapGet("/cardproductsubscriptions", async () =>
        {
            Console.WriteLine("\n\n[BoardingData] GET /cardproductsubscriptions");
            var all = await DBBoardingCardProductSubscriptionServices.GetAllAsync();
            var dtos = all.Select(DBBoardingCardProductSubscriptionServices.ToDto).ToList();
            Console.WriteLine($"\n\n[BoardingData] RESPONSE cardproductsubscriptions count={dtos.Count}");
            return Results.Json(dtos);
        })
        .WithName("GetAllCardProductSubscriptions");

        // Subscriptions linked to a specific transacting merchant (via junction table)
        group.MapGet("/cardproductsubscriptions/bytransacting/{transId:int}", async ([FromRoute] int transId) =>
        {
            Console.WriteLine($"\n\n[BoardingData] GET /cardproductsubscriptions/bytransacting/{transId}");
            var subs = await DBBoardingCardProductSubscriptionServices.GetByTransactingIdAsync(transId);
            var dtos = subs.Select(DBBoardingCardProductSubscriptionServices.ToDto).ToList();
            Console.WriteLine($"\n\n[BoardingData] RESPONSE subscriptions count={dtos.Count}");
            return Results.Json(dtos);
        })
        .WithName("GetCardProductSubscriptionsByTransacting");

        group.MapGet("/cardproductsubscription/{id:int}", async ([FromRoute] int id) =>
        {
            Console.WriteLine($"\n\n[BoardingData] GET /cardproductsubscription/{id}");
            var sub = await DBBoardingCardProductSubscriptionServices.GetSubscriptionByIdAsync(id);
            if (sub is null)
            {
                var err = BuildError("Not Found", $"Subscription {id} not found.", "Check the ID.");
                return Results.Json(err);
            }
            var dto = DBBoardingCardProductSubscriptionServices.ToDto(sub);
            Console.WriteLine($"\n\n[BoardingData] RESPONSE: {JsonSerializer.Serialize(dto, _logOptions)}");
            return Results.Json(dto);
        })
        .WithName("GetCardProductSubscriptionById");

        group.MapPost("/cardproductsubscription", async ([FromBody] BoardingCardProductSubscriptionDto dto) =>
        {
            Console.WriteLine($"\n\n[BoardingData] POST /cardproductsubscription REQUEST: {JsonSerializer.Serialize(dto, _logOptions)}");
            var entity = await DBBoardingCardProductSubscriptionServices.CreateSubscriptionAsync(dto);
            if (entity is null)
            {
                var err = BuildError("Database Error", "Failed to create card product subscription.", "Check server logs.");
                Console.WriteLine($"\n\n[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(err, _logOptions)}");
                return Results.Json(err);
            }
            var full = await DBBoardingCardProductSubscriptionServices.GetSubscriptionByIdAsync(entity.BoardingProductSubscriptionId);
            var response = DBBoardingCardProductSubscriptionServices.ToDto(full!);
            Console.WriteLine($"\n\n[BoardingData] RESPONSE: {JsonSerializer.Serialize(response, _logOptions)}");
            return Results.Json(response);
        })
        .WithName("CreateCardProductSubscription");

        group.MapPut("/cardproductsubscription/{id:int}", async ([FromRoute] int id, [FromBody] BoardingCardProductSubscriptionDto dto) =>
        {
            Console.WriteLine($"\n\n[BoardingData] PUT /cardproductsubscription/{id} REQUEST: {JsonSerializer.Serialize(dto, _logOptions)}");
            var entity = await DBBoardingCardProductSubscriptionServices.UpdateSubscriptionAsync(id, dto);
            if (entity is null)
            {
                var err = BuildError("Not Found", $"Subscription {id} not found or could not be updated.", "Check the ID and server logs.");
                Console.WriteLine($"\n\n[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(err, _logOptions)}");
                return Results.Json(err);
            }
            var full = await DBBoardingCardProductSubscriptionServices.GetSubscriptionByIdAsync(entity.BoardingProductSubscriptionId);
            var response = DBBoardingCardProductSubscriptionServices.ToDto(full!);
            Console.WriteLine($"\n\n[BoardingData] RESPONSE: {JsonSerializer.Serialize(response, _logOptions)}");
            return Results.Json(response);
        })
        .WithName("UpdateCardProductSubscription");

        group.MapDelete("/cardproductsubscription/{id:int}", async ([FromRoute] int id) =>
        {
            Console.WriteLine($"\n\n[BoardingData] DELETE /cardproductsubscription/{id}");
            bool deleted = await DBBoardingCardProductSubscriptionServices.DeleteSubscriptionAsync(id);
            if (!deleted)
            {
                var err = BuildError("Not Found", $"Subscription {id} not found or could not be deleted.", "Check the ID.");
                return Results.Json(err);
            }
            return Results.Json(new { deleted = true, boardingProductSubscriptionId = id });
        })
        .WithName("DeleteCardProductSubscription");

        group.MapPost("/clonesubscription/{id:int}", async ([FromRoute] int id) =>
        {
            Console.WriteLine($"\n\n[BoardingData] POST /clonesubscription/{id}");
            var cloned = await DBBoardingCardProductSubscriptionServices.CloneSubscriptionAsync(id);
            if (cloned is null)
            {
                var err = BuildError("Database Error", $"Failed to clone subscription {id}.", "Check server logs.");
                Console.WriteLine($"\n\n[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(err, _logOptions)}");
                return Results.Json(err);
            }
            var full = await DBBoardingCardProductSubscriptionServices.GetSubscriptionByIdAsync(cloned.BoardingProductSubscriptionId);
            var response = DBBoardingCardProductSubscriptionServices.ToDto(full!);
            Console.WriteLine($"\n\n[BoardingData] RESPONSE: {JsonSerializer.Serialize(response, _logOptions)}");
            return Results.Json(response);
        })
        .WithName("CloneCardProductSubscription");

        // =====================================================================
        // TRANSACTING MERCHANT ↔ SUBSCRIPTION JUNCTION
        // =====================================================================

        group.MapPost("/transactingmerchantsubscription", async ([FromBody] BoardingTransactingMerchantSubscriptionDto dto) =>
        {
            Console.WriteLine($"\n\n[BoardingData] POST /transactingmerchantsubscription REQUEST: {JsonSerializer.Serialize(dto, _logOptions)}");
            var entity = await DBBoardingCardProductSubscriptionServices.LinkSubscriptionToMerchantAsync(dto);
            if (entity is null)
            {
                var err = BuildError("Database Error", "Failed to link subscription to transacting merchant.", "Check server logs.");
                Console.WriteLine($"\n\n[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(err, _logOptions)}");
                return Results.Json(err);
            }
            var response = new BoardingTransactingMerchantSubscriptionDto
            {
                BoardingTransactingMerchantSubscriptionId = entity.BoardingTransactingMerchantSubscriptionId,
                BoardingTransactingMerchantId             = entity.BoardingTransactingMerchantId,
                BoardingProductSubscriptionId             = entity.BoardingProductSubscriptionId
            };
            Console.WriteLine($"\n\n[BoardingData] RESPONSE: {JsonSerializer.Serialize(response, _logOptions)}");
            return Results.Json(response);
        })
        .WithName("LinkSubscriptionToTransactingMerchant");

        group.MapDelete("/transactingmerchantsubscription/{id:int}", async ([FromRoute] int id) =>
        {
            Console.WriteLine($"\n\n[BoardingData] DELETE /transactingmerchantsubscription/{id}");
            bool deleted = await DBBoardingCardProductSubscriptionServices.UnlinkSubscriptionFromMerchantAsync(id);
            if (!deleted)
            {
                var err = BuildError("Not Found", $"Merchant-subscription link {id} not found or could not be deleted.", "Check the ID.");
                return Results.Json(err);
            }
            return Results.Json(new { deleted = true, boardingTransactingMerchantSubscriptionId = id });
        })
        .WithName("UnlinkSubscriptionFromTransactingMerchant");

        group.MapPatch("/transactingmerchantsubscription/{id:int}/includeinboarding", async ([FromRoute] int id, [FromBody] IncludeInBoardingUpdateDto dto) =>
        {
            Console.WriteLine($"\n\n[BoardingData] PATCH /transactingmerchantsubscription/{id}/includeinboarding REQUEST: {JsonSerializer.Serialize(dto, _logOptions)}");
            bool ok = await DBBoardingCardProductSubscriptionServices.UpdateCardJunctionIncludeInBoardingAsync(id, dto.Include);
            if (!ok)
            {
                var err = BuildError("Not Found", $"Card config junction {id} not found or could not be updated.", "Check the ID.");
                Console.WriteLine($"\n\n[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(err, _logOptions)}");
                return Results.Json(err);
            }
            var response = new IncludeInBoardingUpdateDto { Include = dto.Include };
            Console.WriteLine($"\n\n[BoardingData] RESPONSE: {JsonSerializer.Serialize(response, _logOptions)}");
            return Results.Json(response);
        })
        .WithName("UpdateCardJunctionIncludeInBoarding");

        // =====================================================================
        // PROCESSOR CONFIG (standalone upsert used from CardProcessingConfigForm)
        // =====================================================================

        group.MapPost("/processorconfig", async ([FromBody] BoardingProcessorConfigDto dto) =>
        {
            Console.WriteLine($"\n\n[BoardingData] POST /processorconfig REQUEST: {JsonSerializer.Serialize(dto, _logOptions)}");
            var entity = await DBBoardingCardProductSubscriptionServices.UpsertProcessorConfigAsync(dto);
            if (entity is null)
            {
                var err = BuildError("Database Error", "Failed to save processor config.", "Check server logs.");
                Console.WriteLine($"\n\n[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(err, _logOptions)}");
                return Results.Json(err);
            }
            var response = new BoardingProcessorConfigDto { BoardingProcessorConfigId = entity.BoardingProcessorConfigId, ProcessorName = entity.ProcessorName };
            Console.WriteLine($"[BoardingData] RESPONSE: {JsonSerializer.Serialize(response, _logOptions)}");
            return Results.Json(response);
        })
        .WithName("UpsertBoardingProcessorConfig");

        group.MapDelete("/processorconfig/{id:int}", async ([FromRoute] int id) =>
        {
            Console.WriteLine($"\n\n[BoardingData] DELETE /processorconfig/{id}");
            bool deleted = await DBBoardingCardProductSubscriptionServices.DeleteProcessorConfigAsync(id);
            if (!deleted)
            {
                var err = BuildError("Not Found", $"Processor config {id} not found or could not be deleted.", "Check the ID.");
                return Results.Json(err);
            }
            return Results.Json(new { deleted = true, boardingProcessorConfigId = id });
        })
        .WithName("DeleteBoardingProcessorConfig");

        // =====================================================================
        // SUPPLEMENTAL PRODUCT SUBSCRIPTIONS
        // (digitalPayments / customerInvoicing / payByLink / tokenManagement /
        //  unifiedCheckout / valueAddedServices / virtualTerminal)
        // =====================================================================

        MapSimpleCrud(group, "digitalpayments",
            () => DBBoardingDigitalPaymentsServices.GetAllAsync().ContinueWith(t => t.Result.Select(DBBoardingDigitalPaymentsServices.ToDto).ToList()),
            id => DBBoardingDigitalPaymentsServices.GetByIdAsync(id).ContinueWith(t => t.Result is null ? null : DBBoardingDigitalPaymentsServices.ToDto(t.Result)),
            (BoardingDigitalPaymentsSubscriptionDto dto) => DBBoardingDigitalPaymentsServices.CreateAsync(dto).ContinueWith(t => t.Result is null ? null : DBBoardingDigitalPaymentsServices.ToDto(t.Result)),
            (int id, BoardingDigitalPaymentsSubscriptionDto dto) => DBBoardingDigitalPaymentsServices.UpdateAsync(id, dto).ContinueWith(t => t.Result is null ? null : DBBoardingDigitalPaymentsServices.ToDto(t.Result)),
            id => DBBoardingDigitalPaymentsServices.DeleteAsync(id));

        MapSimpleCrud(group, "invoicing",
            () => DBBoardingInvoicingServices.GetAllAsync().ContinueWith(t => t.Result.Select(DBBoardingInvoicingServices.ToDto).ToList()),
            id => DBBoardingInvoicingServices.GetByIdAsync(id).ContinueWith(t => t.Result is null ? null : DBBoardingInvoicingServices.ToDto(t.Result)),
            (BoardingInvoicingSubscriptionDto dto) => DBBoardingInvoicingServices.CreateAsync(dto).ContinueWith(t => t.Result is null ? null : DBBoardingInvoicingServices.ToDto(t.Result)),
            (int id, BoardingInvoicingSubscriptionDto dto) => DBBoardingInvoicingServices.UpdateAsync(id, dto).ContinueWith(t => t.Result is null ? null : DBBoardingInvoicingServices.ToDto(t.Result)),
            id => DBBoardingInvoicingServices.DeleteAsync(id));

        MapSimpleCrud(group, "paybylink",
            () => DBBoardingPayByLinkServices.GetAllAsync().ContinueWith(t => t.Result.Select(DBBoardingPayByLinkServices.ToDto).ToList()),
            id => DBBoardingPayByLinkServices.GetByIdAsync(id).ContinueWith(t => t.Result is null ? null : DBBoardingPayByLinkServices.ToDto(t.Result)),
            (BoardingPayByLinkSubscriptionDto dto) => DBBoardingPayByLinkServices.CreateAsync(dto).ContinueWith(t => t.Result is null ? null : DBBoardingPayByLinkServices.ToDto(t.Result)),
            (int id, BoardingPayByLinkSubscriptionDto dto) => DBBoardingPayByLinkServices.UpdateAsync(id, dto).ContinueWith(t => t.Result is null ? null : DBBoardingPayByLinkServices.ToDto(t.Result)),
            id => DBBoardingPayByLinkServices.DeleteAsync(id));

        MapSimpleCrud(group, "tokenmanagement",
            () => DBBoardingTokenManagementServices.GetAllAsync().ContinueWith(t => t.Result.Select(DBBoardingTokenManagementServices.ToDto).ToList()),
            id => DBBoardingTokenManagementServices.GetByIdAsync(id).ContinueWith(t => t.Result is null ? null : DBBoardingTokenManagementServices.ToDto(t.Result)),
            (BoardingTokenManagementSubscriptionDto dto) => DBBoardingTokenManagementServices.CreateAsync(dto).ContinueWith(t => t.Result is null ? null : DBBoardingTokenManagementServices.ToDto(t.Result)),
            (int id, BoardingTokenManagementSubscriptionDto dto) => DBBoardingTokenManagementServices.UpdateAsync(id, dto).ContinueWith(t => t.Result is null ? null : DBBoardingTokenManagementServices.ToDto(t.Result)),
            id => DBBoardingTokenManagementServices.DeleteAsync(id));

        MapSimpleCrud(group, "unifiedcheckout",
            () => DBBoardingUnifiedCheckoutServices.GetAllAsync().ContinueWith(t => t.Result.Select(DBBoardingUnifiedCheckoutServices.ToDto).ToList()),
            id => DBBoardingUnifiedCheckoutServices.GetByIdAsync(id).ContinueWith(t => t.Result is null ? null : DBBoardingUnifiedCheckoutServices.ToDto(t.Result)),
            (BoardingUnifiedCheckoutSubscriptionDto dto) => DBBoardingUnifiedCheckoutServices.CreateAsync(dto).ContinueWith(t => t.Result is null ? null : DBBoardingUnifiedCheckoutServices.ToDto(t.Result)),
            (int id, BoardingUnifiedCheckoutSubscriptionDto dto) => DBBoardingUnifiedCheckoutServices.UpdateAsync(id, dto).ContinueWith(t => t.Result is null ? null : DBBoardingUnifiedCheckoutServices.ToDto(t.Result)),
            id => DBBoardingUnifiedCheckoutServices.DeleteAsync(id));

        MapSimpleCrud(group, "valueaddedservices",
            () => DBBoardingValueAddedServicesServices.GetAllAsync().ContinueWith(t => t.Result.Select(DBBoardingValueAddedServicesServices.ToDto).ToList()),
            id => DBBoardingValueAddedServicesServices.GetByIdAsync(id).ContinueWith(t => t.Result is null ? null : DBBoardingValueAddedServicesServices.ToDto(t.Result)),
            (BoardingValueAddedServicesSubscriptionDto dto) => DBBoardingValueAddedServicesServices.CreateAsync(dto).ContinueWith(t => t.Result is null ? null : DBBoardingValueAddedServicesServices.ToDto(t.Result)),
            (int id, BoardingValueAddedServicesSubscriptionDto dto) => DBBoardingValueAddedServicesServices.UpdateAsync(id, dto).ContinueWith(t => t.Result is null ? null : DBBoardingValueAddedServicesServices.ToDto(t.Result)),
            id => DBBoardingValueAddedServicesServices.DeleteAsync(id));

        MapSimpleCrud(group, "virtualterminal",
            () => DBBoardingVirtualTerminalServices.GetAllAsync().ContinueWith(t => t.Result.Select(DBBoardingVirtualTerminalServices.ToDto).ToList()),
            id => DBBoardingVirtualTerminalServices.GetByIdAsync(id).ContinueWith(t => t.Result is null ? null : DBBoardingVirtualTerminalServices.ToDto(t.Result)),
            (BoardingVirtualTerminalSubscriptionDto dto) => DBBoardingVirtualTerminalServices.CreateAsync(dto).ContinueWith(t => t.Result is null ? null : DBBoardingVirtualTerminalServices.ToDto(t.Result)),
            (int id, BoardingVirtualTerminalSubscriptionDto dto) => DBBoardingVirtualTerminalServices.UpdateAsync(id, dto).ContinueWith(t => t.Result is null ? null : DBBoardingVirtualTerminalServices.ToDto(t.Result)),
            id => DBBoardingVirtualTerminalServices.DeleteAsync(id));

        MapSimpleCrud(group, "payerauthentication",
            () => DBBoardingPayerAuthenticationServices.GetAllAsync().ContinueWith(t => t.Result.Select(DBBoardingPayerAuthenticationServices.ToDto).ToList()),
            id => DBBoardingPayerAuthenticationServices.GetByIdAsync(id).ContinueWith(t => t.Result is null ? null : DBBoardingPayerAuthenticationServices.ToDto(t.Result)),
            (BoardingPayerAuthenticationSubscriptionDto dto) => DBBoardingPayerAuthenticationServices.CreateAsync(dto).ContinueWith(t => t.Result is null ? null : DBBoardingPayerAuthenticationServices.ToDto(t.Result)),
            (int id, BoardingPayerAuthenticationSubscriptionDto dto) => DBBoardingPayerAuthenticationServices.UpdateAsync(id, dto).ContinueWith(t => t.Result is null ? null : DBBoardingPayerAuthenticationServices.ToDto(t.Result)),
            id => DBBoardingPayerAuthenticationServices.DeleteAsync(id));

        // =====================================================================
        // DASHBOARD
        // =====================================================================

        group.MapGet("/dashboard", async () =>
        {
            Console.WriteLine("\n\n[BoardingData] GET /dashboard");
            var result = await DBBoardingDashboardServices.GetDashboardAsync();
            if (result is null)
            {
                var err = BuildError("Database Error", "Failed to load boarding dashboard.", "Check server logs.");
                Console.WriteLine($"\n\n[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(err, _logOptions)}");
                return Results.Json(err);
            }
            Console.WriteLine($"\n\n[BoardingData] RESPONSE dashboard orgs={result.Organizations.Count}");
            return Results.Json(result);
        })
        .WithName("GetBoardingDashboard");

        // ── Polymorphic link endpoints ───────────────────────────────────────
        group.MapGet("/productlinks/bymerchant/{merchantId:int}", async ([FromRoute] int merchantId) =>
        {
            Console.WriteLine($"\n\n[BoardingData] GET /productlinks/bymerchant/{merchantId}");
            var rows = await DBBoardingMerchantProductSubscriptionServices.GetByMerchantAsync(merchantId);
            var dtos = rows.Select(DBBoardingMerchantProductSubscriptionServices.ToDto).ToList();
            Console.WriteLine($"[BoardingData] RESPONSE productlinks count={dtos.Count}");
            return Results.Json(dtos);
        })
        .WithName("GetBoardingProductLinksByMerchant");

        group.MapPost("/productlink", async ([FromBody] BoardingTransactingMerchantProductSubscriptionDto dto) =>
        {
            Console.WriteLine($"\n\n[BoardingData] POST /productlink REQUEST: {JsonSerializer.Serialize(dto, _logOptions)}");
            if (!BoardingProductTypes.IsValid(dto.ProductType))
            {
                var bad = BuildError("Validation Error", $"Unknown productType '{dto.ProductType}'.", "Use one of the supported product keys.");
                Console.WriteLine($"[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(bad, _logOptions)}");
                return Results.Json(bad);
            }
            var entity = await DBBoardingMerchantProductSubscriptionServices.LinkAsync(dto);
            if (entity is null)
            {
                var err = BuildError("Database Error", "Failed to link product subscription to transacting merchant.", "Check server logs.");
                return Results.Json(err);
            }
            var response = DBBoardingMerchantProductSubscriptionServices.ToDto(entity);
            Console.WriteLine($"[BoardingData] RESPONSE: {JsonSerializer.Serialize(response, _logOptions)}");
            return Results.Json(response);
        })
        .WithName("LinkProductSubscriptionToTransactingMerchant");

        group.MapDelete("/productlink/{id:int}", async ([FromRoute] int id) =>
        {
            Console.WriteLine($"\n\n[BoardingData] DELETE /productlink/{id}");
            bool deleted = await DBBoardingMerchantProductSubscriptionServices.UnlinkAsync(id);
            if (!deleted)
            {
                var err = BuildError("Not Found", $"Product link {id} not found or could not be deleted.", "Check the ID.");
                return Results.Json(err);
            }
            return Results.Json(new { deleted = true, boardingTransactingMerchantProductSubscriptionId = id });
        })
        .WithName("UnlinkProductSubscriptionFromTransactingMerchant");

        group.MapPatch("/productlink/{id:int}/includeinboarding", async ([FromRoute] int id, [FromBody] IncludeInBoardingUpdateDto dto) =>
        {
            Console.WriteLine($"\n\n[BoardingData] PATCH /productlink/{id}/includeinboarding REQUEST: {JsonSerializer.Serialize(dto, _logOptions)}");
            bool ok = await DBBoardingMerchantProductSubscriptionServices.UpdateProductLinkIncludeInBoardingAsync(id, dto.Include);
            if (!ok)
            {
                var err = BuildError("Not Found", $"Product link {id} not found or could not be updated.", "Check the ID.");
                Console.WriteLine($"\n\n[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(err, _logOptions)}");
                return Results.Json(err);
            }
            var response = new IncludeInBoardingUpdateDto { Include = dto.Include };
            Console.WriteLine($"\n\n[BoardingData] RESPONSE: {JsonSerializer.Serialize(response, _logOptions)}");
            return Results.Json(response);
        })
        .WithName("UpdateProductLinkIncludeInBoarding");
    }

    private static ErrorObject BuildError(string error, string message, string action) =>
        new ErrorObject { Error = error, Message = message, Action = action, Reason = "Boarding data operation failed." };

    // Generic wiring for a /{route} CRUD group backed by a product-specific DB service.
    // Each product's endpoints log inbound/outbound JSON and return 2xx with an
    // ErrorObject on failure, per the project's error-handling rules.
    private static void MapSimpleCrud<TDto>(
        RouteGroupBuilder group,
        string route,
        Func<Task<List<TDto>>> getAll,
        Func<int, Task<TDto?>> getById,
        Func<TDto, Task<TDto?>> create,
        Func<int, TDto, Task<TDto?>> update,
        Func<int, Task<bool>> delete)
        where TDto : class
    {
        var baseName = char.ToUpperInvariant(route[0]) + route.Substring(1);

        group.MapGet($"/{route}/all", async () =>
        {
            Console.WriteLine($"\n\n[BoardingData] GET /{route}/all");
            var list = await getAll();
            Console.WriteLine($"[BoardingData] RESPONSE {route} count={list.Count}");
            return Results.Json(list);
        }).WithName($"GetAll{baseName}");

        group.MapGet($"/{route}/{{id:int}}", async ([FromRoute] int id) =>
        {
            Console.WriteLine($"\n\n[BoardingData] GET /{route}/{id}");
            var dto = await getById(id);
            if (dto is null)
            {
                var err = BuildError("Not Found", $"{route} {id} not found.", "Check the ID.");
                return Results.Json(err);
            }
            Console.WriteLine($"[BoardingData] RESPONSE: {JsonSerializer.Serialize(dto, _logOptions)}");
            return Results.Json(dto);
        }).WithName($"Get{baseName}ById");

        group.MapPost($"/{route}", async ([FromBody] TDto dto) =>
        {
            Console.WriteLine($"\n\n[BoardingData] POST /{route} REQUEST: {JsonSerializer.Serialize(dto, _logOptions)}");
            var saved = await create(dto);
            if (saved is null)
            {
                var err = BuildError("Database Error", $"Failed to create {route}.", "Check server logs.");
                Console.WriteLine($"[BoardingData] RESPONSE ERROR: {JsonSerializer.Serialize(err, _logOptions)}");
                return Results.Json(err);
            }
            Console.WriteLine($"[BoardingData] RESPONSE: {JsonSerializer.Serialize(saved, _logOptions)}");
            return Results.Json(saved);
        }).WithName($"Create{baseName}");

        group.MapPut($"/{route}/{{id:int}}", async ([FromRoute] int id, [FromBody] TDto dto) =>
        {
            Console.WriteLine($"\n\n[BoardingData] PUT /{route}/{id} REQUEST: {JsonSerializer.Serialize(dto, _logOptions)}");
            var saved = await update(id, dto);
            if (saved is null)
            {
                var err = BuildError("Not Found", $"{route} {id} not found.", "Check the ID.");
                return Results.Json(err);
            }
            Console.WriteLine($"[BoardingData] RESPONSE: {JsonSerializer.Serialize(saved, _logOptions)}");
            return Results.Json(saved);
        }).WithName($"Update{baseName}");

        group.MapDelete($"/{route}/{{id:int}}", async ([FromRoute] int id) =>
        {
            Console.WriteLine($"\n\n[BoardingData] DELETE /{route}/{id}");
            var ok = await delete(id);
            if (!ok)
            {
                var err = BuildError("Not Found", $"{route} {id} not found or could not be deleted.", "Check the ID.");
                return Results.Json(err);
            }
            return Results.Json(new { deleted = true, id });
        }).WithName($"Delete{baseName}");
    }
}
