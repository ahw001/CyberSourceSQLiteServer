using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Transactions;
using CybsClass.EntityModels;
using CybsClass.WebApi.Service.Services.DBOperations;
using CybsClass.WebApi.Service.Services.MerchantBoarding;
using System.Text.Json;
namespace CybsClass.WebApi.Service;

public static class MerchantBoardingEndpoints
{
    public static void MapMerchantBoardingEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/merchantboarding").WithTags(nameof(MapMerchantBoardingEndpoints));

        group.MapPost("/getmerchantdetails", async () =>
        {
            string mid = "ahwvdcadmax1144";
            return await CallGetMerchantBoardingInfo.RunAsyncGetMerchantInfo(mid, true);
        })
        .WithName("GetMerchantBoardingDetails");



        group.MapPost("/createmerchant", async ([FromBody] MerchantModelDto merchantModelDto) =>
        {
            return await CallCybsMerchantBoardingCreate.RunAsyncJsonObject(merchantModelDto);
        })
        .WithName("CreateCybsMerchantOrgBoarding");

        group.MapPost("/createtransmerchant", async ([FromBody] MerchantModelDto merchantModelDto) =>
        {
            Console.WriteLine($"\n[MerchantBoarding] POST /createtransmerchant for '{merchantModelDto.TransactingOrganizationId}'");
            Console.WriteLine($"\n[MerchantBoarding] INPUT: {JsonSerializer.Serialize(merchantModelDto)}");

            // When address fields are absent, backfill from DB — try the saved transacting merchant
            // record first, then fall back to the parent org record
            if (string.IsNullOrEmpty(merchantModelDto.Address1))
            {
                if (!string.IsNullOrEmpty(merchantModelDto.TransactingOrganizationId))
                {
                    var trans = await DBBoardingTransactingMerchantServices.GetByTransactingOrgIdAsync(merchantModelDto.TransactingOrganizationId);
                    if (trans is not null)
                    {
                        merchantModelDto.Country            ??= trans.AddressCountry;
                        merchantModelDto.Address1           ??= trans.Address1;
                        merchantModelDto.PostalCode         ??= trans.PostalCode;
                        merchantModelDto.AdministrativeArea ??= trans.AdministrativeArea;
                        merchantModelDto.Locality           ??= trans.Locality;
                        merchantModelDto.Name               ??= trans.BusinessName;
                        merchantModelDto.WebsiteUrl         ??= trans.WebsiteUrl;
                    }
                }

                if (string.IsNullOrEmpty(merchantModelDto.Address1) && !string.IsNullOrEmpty(merchantModelDto.ParentOrganizationId))
                {
                    var parentOrg = await DBBoardingOrganizationServices.GetByOrganizationIdAsync(merchantModelDto.ParentOrganizationId);
                    if (parentOrg is not null)
                    {
                        merchantModelDto.Country            ??= parentOrg.AddressCountry;
                        merchantModelDto.Address1           ??= parentOrg.Address1;
                        merchantModelDto.PostalCode         ??= parentOrg.PostalCode;
                        merchantModelDto.AdministrativeArea ??= parentOrg.AdministrativeArea;
                        merchantModelDto.Locality           ??= parentOrg.Locality;
                        merchantModelDto.Name               ??= parentOrg.BusinessName;
                    }
                }

                Console.WriteLine($"\n[MerchantBoarding] Address after DB backfill — Address1: '{merchantModelDto.Address1}', Locality: '{merchantModelDto.Locality}', Area: '{merchantModelDto.AdministrativeArea}', Postal: '{merchantModelDto.PostalCode}'");
            }

            if (!string.IsNullOrEmpty(merchantModelDto.BoardingProcessor) &&
                merchantModelDto.BoardingProcessor.Contains("fdiglobal", StringComparison.OrdinalIgnoreCase))
            {
                return await CallCybsMerchantTransactingCreateFdiGlobal.RunAsyncJsonObject(merchantModelDto);
            }

            return await CallCybsMerchantTransactingCreate.RunAsyncJsonObject(merchantModelDto);
        })
        .WithName("CreateCybsTransactingBoarding");

        group.MapPost("/submittransactingfromsql/{boardingTransactingMerchantId:int}", async ([FromRoute] int boardingTransactingMerchantId) =>
        {
            Console.WriteLine($"\n[MerchantBoarding] POST /submittransactingfromsql/{boardingTransactingMerchantId}");

            var merchant = await DBSubmitTransactingFromSqlServices.GetTransactingMerchantWithDetailsAsync(boardingTransactingMerchantId);
            if (merchant is null)
            {
                var err = BuildError("Not Found",
                    $"Transacting merchant {boardingTransactingMerchantId} not found in the database.",
                    "Verify the BoardingTransactingMerchantId and try again.");
                Console.WriteLine($"\n[MerchantBoarding] RESPONSE ERROR: {JsonSerializer.Serialize(err)}");
                return Results.Json(err);
            }

            Console.WriteLine($"\n[MerchantBoarding] INPUT transactingOrganizationId: {merchant.TransactingOrganizationId}, processor: {merchant.BoardingProcessor}, parentOrg: {merchant.BoardingOrganization?.OrganizationId}");

            Console.WriteLine($"\n[MerchantBoarding] Checking Cybersource for transacting merchant '{merchant.TransactingOrganizationId}'");
            var checkResult = await CallGetMerchantBoardingInfo.RunAsyncGetMerchantInfo(merchant.TransactingOrganizationId, true);
            Console.WriteLine($"\n[MerchantBoarding] Cybersource check result: {checkResult}");

            if (checkResult.ContainsKey("organizationInformation"))
            {
                Console.WriteLine($"\n[MerchantBoarding] Merchant '{merchant.TransactingOrganizationId}' already exists in Cybersource — boarding included products/card configs only.");

                var includedCardSub      = await DBSubmitTransactingFromSqlServices.GetIncludedCardSubscriptionForMerchantAsync(boardingTransactingMerchantId);
                var includedProducts     = await DBSubmitTransactingFromSqlServices.GetIncludedSupplementalProductTypesAsync(boardingTransactingMerchantId);
                var includedVas          = await DBSubmitTransactingFromSqlServices.GetIncludedVasSubscriptionForMerchantAsync(boardingTransactingMerchantId);

                Console.WriteLine($"\n[MerchantBoarding] Included card sub: {includedCardSub?.ProductName ?? "none"}, included supplemental products: {JsonSerializer.Serialize(includedProducts)}");

                var patchResult = await CallCybsTransactingFromSql.RunAsyncProductsOnlyJsonObject(merchant, includedCardSub, includedProducts, includedVas);
                Console.WriteLine($"\n[MerchantBoarding] RESPONSE products-only patch: {patchResult}");

                if (patchResult.ContainsKey("organizationInformation") && !patchResult.ContainsKey("error"))
                {
                    await DBSubmitTransactingFromSqlServices.UpdateCybersourceBoardingStatusAsync(boardingTransactingMerchantId, "BOARDED");
                    if (includedCardSub is not null)
                    {
                        var cardCfgStatus = patchResult["productInformationSetups"]?[0]?["setups"]?["payments"]?["cardProcessing"]?["configurationStatus"]?["status"]?.GetValue<string>();
                        var cardStatus = string.Equals(cardCfgStatus, "FAILURE", StringComparison.OrdinalIgnoreCase) ? "FAILED" : "BOARDED";
                        await DBSubmitTransactingFromSqlServices.UpdateCardSubscriptionBoardingStatusAsync(boardingTransactingMerchantId, includedCardSub.BoardingProductSubscriptionId, cardStatus);
                    }
                    if (includedProducts.Count > 0)
                        await DBSubmitTransactingFromSqlServices.UpdateProductJunctionsBoardingStatusAsync(boardingTransactingMerchantId, includedProducts, "BOARDED");
                }

                return Results.Json(patchResult);
            }

            var cardSubscription      = await DBSubmitTransactingFromSqlServices.GetCardSubscriptionForMerchantAsync(boardingTransactingMerchantId);
            var supplementalProducts  = await DBSubmitTransactingFromSqlServices.GetSupplementalProductTypesAsync(boardingTransactingMerchantId);
            var vasSubscription       = await DBSubmitTransactingFromSqlServices.GetVasSubscriptionForMerchantAsync(boardingTransactingMerchantId);

            Console.WriteLine($"\n[MerchantBoarding] Card subscription productName: {cardSubscription?.ProductName}, processor: {cardSubscription?.CardProcessingConfigs.FirstOrDefault()?.ProcessorConfigs.FirstOrDefault()?.ProcessorName}");
            Console.WriteLine($"\n[MerchantBoarding] Supplemental products: {JsonSerializer.Serialize(supplementalProducts)}");

            var result = await CallCybsTransactingFromSql.RunAsyncJsonObject(merchant, cardSubscription, supplementalProducts, vasSubscription);
            Console.WriteLine($"\n[MerchantBoarding] RESPONSE submittransactingfromsql: {result}");

            if (result.ContainsKey("organizationInformation") && !result.ContainsKey("error"))
            {
                await DBSubmitTransactingFromSqlServices.UpdateCybersourceBoardingStatusAsync(boardingTransactingMerchantId, "BOARDED");
                if (cardSubscription is not null)
                {
                    var cardCfgStatus = result["productInformationSetups"]?[0]?["setups"]?["payments"]?["cardProcessing"]?["configurationStatus"]?["status"]?.GetValue<string>();
                    var cardStatus = string.Equals(cardCfgStatus, "FAILURE", StringComparison.OrdinalIgnoreCase) ? "FAILED" : "BOARDED";
                    await DBSubmitTransactingFromSqlServices.UpdateCardSubscriptionBoardingStatusAsync(boardingTransactingMerchantId, cardSubscription.BoardingProductSubscriptionId, cardStatus);
                }
                if (supplementalProducts.Count > 0)
                    await DBSubmitTransactingFromSqlServices.UpdateProductJunctionsBoardingStatusAsync(boardingTransactingMerchantId, supplementalProducts, "BOARDED");
            }

            return Results.Json(result);
        })
        .WithName("SubmitTransactingMerchantToCybersource");

        group.MapPost("/submitorgfromsql/{boardingOrganizationId:int}", async ([FromRoute] int boardingOrganizationId) =>
        {
            Console.WriteLine($"\n[MerchantBoarding] POST /submitorgfromsql/{boardingOrganizationId}");

            var org = await DBBoardingOrganizationServices.GetByIdAsync(boardingOrganizationId);
            if (org is null)
            {
                var err = BuildError("Not Found",
                    $"Organization {boardingOrganizationId} not found in the database.",
                    "Verify the BoardingOrganizationId and try again.");
                Console.WriteLine($"\n[MerchantBoarding] RESPONSE ERROR: {JsonSerializer.Serialize(err)}");
                return Results.Json(err);
            }

            Console.WriteLine($"\n[MerchantBoarding] Checking Cybersource for org '{org.OrganizationId}'");
            var checkResult = await CallGetMerchantBoardingInfo.RunAsyncGetMerchantInfo(org.OrganizationId ?? "", true);
            Console.WriteLine($"\n[MerchantBoarding] Cybersource check result: {checkResult}");

            if (checkResult.ContainsKey("organizationInformation"))
            {
                var err = BuildError("Already Exists",
                    $"Merchant '{org.OrganizationId}' already exists in Cybersource. No action taken.",
                    "Use the Cybersource portal to modify the existing merchant.");
                Console.WriteLine($"\n[MerchantBoarding] RESPONSE ERROR (already exists): {JsonSerializer.Serialize(err)}");
                return Results.Json(err);
            }

            var contacts = org.Contacts?.ToList() ?? new List<BoardingContact>();
            var biz  = contacts.FirstOrDefault(c => c.ContactType == "Business");
            var tech = contacts.FirstOrDefault(c => c.ContactType == "Technical");
            var emrg = contacts.FirstOrDefault(c => c.ContactType == "Emergency");

            var merchantDto = new MerchantModelDto
            {
                OrganizationId              = org.OrganizationId,
                ParentOrganizationId        = org.ParentOrganizationId,
                Status                      = org.Status,
                Type                        = org.Type ?? "MERCHANT",
                Configurable                = org.Configurable == true ? "TRUE" : "FALSE",
                Country                     = org.AddressCountry,
                Address1                    = org.Address1,
                PostalCode                  = org.PostalCode,
                AdministrativeArea          = org.AdministrativeArea,
                Locality                    = org.Locality,
                Name                        = org.BusinessName,
                WebsiteUrl                  = org.WebsiteUrl,
                BusinessInformationPhoneNumber = org.BusinessPhoneNumber,
                BusinessInformationTimeZone = org.TimeZone,
                MerchantCategoryCode        = int.TryParse(org.MerchantCategoryCode, out var mcc) ? mcc : 5999,
                BoardingPackageId           = org.BoardingPackageId,
                OrganizationMerchant        = true,
                TransactingMerchant         = false,
                BusinessContactFirstName    = biz?.FirstName,
                BusinessContactLastName     = biz?.LastName,
                BusinessContactPhoneNumber  = biz?.PhoneNumber,
                BusinessContactEmail        = biz?.Email,
                TechnicalContactFirstName   = tech?.FirstName,
                TechnicalContactLastName    = tech?.LastName,
                TechnicalContactPhoneNumber = tech?.PhoneNumber,
                TechnicalContactEmail       = tech?.Email,
                EmergencyContactFirstName   = emrg?.FirstName,
                EmergencyContactLastName    = emrg?.LastName,
                EmergencyContactPhoneNumber = emrg?.PhoneNumber,
                EmergencyContactEmail       = emrg?.Email,
            };

            Console.WriteLine($"\n[MerchantBoarding] Submitting org '{org.OrganizationId}' to Cybersource");
            var result = await CallCybsMerchantBoardingCreate.RunAsyncJsonObject(merchantDto);
            Console.WriteLine($"\n[MerchantBoarding] RESPONSE submitorgfromsql: {result}");
            return Results.Json(result);
        })
        .WithName("SubmitOrgToCybersource");

    }

    private static CybsClass.Cybersource.Models.BaseData.ErrorObject BuildError(string error, string message, string action) =>
        new CybsClass.Cybersource.Models.BaseData.ErrorObject { Error = error, Message = message, Action = action, Reason = "Merchant boarding operation failed." };
}

