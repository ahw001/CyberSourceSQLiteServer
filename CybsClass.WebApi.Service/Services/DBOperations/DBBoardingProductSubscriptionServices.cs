using Microsoft.EntityFrameworkCore;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.WebApi.Service.Services.DBOperations;

public static class DBBoardingCardProductSubscriptionServices
{
    // ── Create Subscription ──────────────────────────────────────────────────
    public static async Task<BoardingCardProductSubscription?> CreateSubscriptionAsync(BoardingCardProductSubscriptionDto dto)
    {
        try
        {
            using B2cNorthwindContext db = new();

            var sub = new BoardingCardProductSubscription
            {
                BoardingTransactingMerchantId = dto.BoardingTransactingMerchantId,
                ProductName                   = dto.ProductName ?? string.Empty,
                ProductCategory               = dto.ProductCategory ?? string.Empty,
                SubscriptionEnabled           = dto.SubscriptionEnabled,
                EnablementStatus              = dto.EnablementStatus,
                SelfServiceability            = dto.SelfServiceability,
                Distributability              = dto.Distributability,
                CardPresentEnabled            = dto.CardPresentEnabled,
                CardNotPresentEnabled         = dto.CardNotPresentEnabled,
                TemplateId                    = dto.TemplateId,
                CreatedAt                     = DateTime.UtcNow
            };
            db.BoardingCardProductSubscriptions.Add(sub);
            await db.SaveChangesAsync();

            if (dto.CardProcessingConfig is not null)
            {
                await CreateCardProcessingConfigAsync(db, sub.BoardingProductSubscriptionId, dto.CardProcessingConfig);
            }

            return sub;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingCardProd] CreateSubscription error: {ex.Message}");
            if (ex.InnerException is not null)
                Console.WriteLine($"[DBBoardingCardProd] CreateSubscription inner: {ex.InnerException.Message}");
            return null;
        }
    }

    // ── Update Subscription ──────────────────────────────────────────────────
    public static async Task<BoardingCardProductSubscription?> UpdateSubscriptionAsync(int id, BoardingCardProductSubscriptionDto dto)
    {
        try
        {
            using B2cNorthwindContext db = new();

            var sub = await db.BoardingCardProductSubscriptions
                .FirstOrDefaultAsync(s => s.BoardingProductSubscriptionId == id);
            if (sub is null) return null;

            sub.ProductName           = dto.ProductName ?? string.Empty;
            sub.ProductCategory       = dto.ProductCategory ?? string.Empty;
            sub.SubscriptionEnabled   = dto.SubscriptionEnabled;
            sub.EnablementStatus      = dto.EnablementStatus;
            sub.SelfServiceability    = dto.SelfServiceability;
            sub.Distributability      = dto.Distributability;
            sub.CardPresentEnabled    = dto.CardPresentEnabled;
            sub.CardNotPresentEnabled = dto.CardNotPresentEnabled;
            sub.TemplateId            = dto.TemplateId;
            await db.SaveChangesAsync();

            // Replace card processing config: delete existing, then recreate from DTO
            var configIds = await db.BoardingCardProcessingConfigs
                .Where(c => c.BoardingProductSubscriptionId == id)
                .Select(c => c.BoardingCardProcessingConfigId)
                .ToListAsync();

            var processorIds = await db.BoardingProcessorConfigs
                .Where(p => configIds.Contains(p.BoardingCardProcessingConfigId))
                .Select(p => p.BoardingProcessorConfigId)
                .ToListAsync();

            await db.BoardingProcessorPaymentTypes
                .Where(pt => processorIds.Contains(pt.BoardingProcessorConfigId))
                .ExecuteDeleteAsync();

            await db.BoardingProcessorConfigs
                .Where(p => processorIds.Contains(p.BoardingProcessorConfigId))
                .ExecuteDeleteAsync();

            await db.BoardingCardProcessingConfigs
                .Where(c => configIds.Contains(c.BoardingCardProcessingConfigId))
                .ExecuteDeleteAsync();

            if (dto.CardProcessingConfig is not null)
                await CreateCardProcessingConfigAsync(db, id, dto.CardProcessingConfig);

            return sub;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingCardProd] UpdateSubscription error: {ex.Message}");
            if (ex.InnerException is not null)
                Console.WriteLine($"[DBBoardingCardProd] UpdateSubscription inner: {ex.InnerException.Message}");
            return null;
        }
    }

    // ── Read All Subscriptions ───────────────────────────────────────────────
    public static async Task<List<BoardingCardProductSubscription>> GetAllAsync()
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingCardProductSubscriptions
                .AsNoTracking()
                .Include(s => s.CardProcessingConfigs)
                    .ThenInclude(c => c.ProcessorConfigs)
                        .ThenInclude(p => p.PaymentTypes)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingCardProd] GetAll error: {ex.Message}");
            return new List<BoardingCardProductSubscription>();
        }
    }

    // ── Read Subscriptions linked to a Transacting Merchant (via junction) ───
    public static async Task<List<BoardingCardProductSubscription>> GetByTransactingIdAsync(int boardingTransactingMerchantId)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingTransactingMerchantSubscriptions
                .AsNoTracking()
                .Where(j => j.BoardingTransactingMerchantId == boardingTransactingMerchantId)
                .Include(j => j.BoardingCardProductSubscription)
                    .ThenInclude(s => s.CardProcessingConfigs)
                        .ThenInclude(c => c.ProcessorConfigs)
                            .ThenInclude(p => p.PaymentTypes)
                .Select(j => j.BoardingCardProductSubscription)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingCardProd] GetByTransacting error: {ex.Message}");
            return new List<BoardingCardProductSubscription>();
        }
    }

    // ── Read Subscription by ID ───────────────────────────────────────────────
    public static async Task<BoardingCardProductSubscription?> GetSubscriptionByIdAsync(int id)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingCardProductSubscriptions
                .AsNoTracking()
                .Include(s => s.CardProcessingConfigs)
                    .ThenInclude(c => c.ProcessorConfigs)
                        .ThenInclude(p => p.PaymentTypes)
                .FirstOrDefaultAsync(s => s.BoardingProductSubscriptionId == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingCardProd] GetById error: {ex.Message}");
            return null;
        }
    }

    // ── Delete Subscription ───────────────────────────────────────────────────
    public static async Task<bool> DeleteSubscriptionAsync(int id)
    {
        try
        {
            using B2cNorthwindContext db = new();

            // Collect IDs top-down so we can delete bottom-up without cascade.
            var configIds = await db.BoardingCardProcessingConfigs
                .Where(c => c.BoardingProductSubscriptionId == id)
                .Select(c => c.BoardingCardProcessingConfigId)
                .ToListAsync();

            var processorIds = await db.BoardingProcessorConfigs
                .Where(p => configIds.Contains(p.BoardingCardProcessingConfigId))
                .Select(p => p.BoardingProcessorConfigId)
                .ToListAsync();

            // Delete leaf → parent
            await db.BoardingProcessorPaymentTypes
                .Where(pt => processorIds.Contains(pt.BoardingProcessorConfigId))
                .ExecuteDeleteAsync();

            await db.BoardingProcessorConfigs
                .Where(p => processorIds.Contains(p.BoardingProcessorConfigId))
                .ExecuteDeleteAsync();

            await db.BoardingCardProcessingConfigs
                .Where(c => configIds.Contains(c.BoardingCardProcessingConfigId))
                .ExecuteDeleteAsync();

            await db.BoardingTransactingMerchantSubscriptions
                .Where(j => j.BoardingProductSubscriptionId == id)
                .ExecuteDeleteAsync();

            return await db.BoardingCardProductSubscriptions
                .Where(s => s.BoardingProductSubscriptionId == id)
                .ExecuteDeleteAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingCardProd] DeleteSubscription error: {ex.Message}");
            if (ex.InnerException is not null)
                Console.WriteLine($"[DBBoardingCardProd] DeleteSubscription inner: {ex.InnerException.Message}");
            return false;
        }
    }

    // ── Junction Table: Link Subscription to Transacting Merchant ────────────
    public static async Task<BoardingTransactingMerchantSubscription?> LinkSubscriptionToMerchantAsync(BoardingTransactingMerchantSubscriptionDto dto)
    {
        try
        {
            using B2cNorthwindContext db = new();

            bool alreadyLinked = await db.BoardingTransactingMerchantSubscriptions
                .AnyAsync(j => j.BoardingTransactingMerchantId == dto.BoardingTransactingMerchantId
                            && j.BoardingProductSubscriptionId  == dto.BoardingProductSubscriptionId);

            if (alreadyLinked) return await db.BoardingTransactingMerchantSubscriptions
                .FirstAsync(j => j.BoardingTransactingMerchantId == dto.BoardingTransactingMerchantId
                              && j.BoardingProductSubscriptionId  == dto.BoardingProductSubscriptionId);

            var junction = new BoardingTransactingMerchantSubscription
            {
                BoardingTransactingMerchantId = dto.BoardingTransactingMerchantId,
                BoardingProductSubscriptionId = dto.BoardingProductSubscriptionId,
                AssignedAt                    = DateTime.UtcNow
            };
            db.BoardingTransactingMerchantSubscriptions.Add(junction);
            await db.SaveChangesAsync();
            return junction;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingCardProd] LinkSubscription error: {ex.Message}");
            return null;
        }
    }

    // ── Junction Table: Update IncludeInBoarding flag ─────────────────────────
    public static async Task<bool> UpdateCardJunctionIncludeInBoardingAsync(int junctionId, bool include)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingTransactingMerchantSubscriptions
                .Where(j => j.BoardingTransactingMerchantSubscriptionId == junctionId)
                .ExecuteUpdateAsync(s => s.SetProperty(j => j.IncludeInBoarding, include)) > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingCardProd] UpdateCardJunctionIncludeInBoarding error: {ex.Message}");
            return false;
        }
    }

    // ── Junction Table: Unlink Subscription from Transacting Merchant ─────────
    public static async Task<bool> UnlinkSubscriptionFromMerchantAsync(int boardingTransactingMerchantSubscriptionId)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingTransactingMerchantSubscriptions
                .Where(j => j.BoardingTransactingMerchantSubscriptionId == boardingTransactingMerchantSubscriptionId)
                .ExecuteDeleteAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingCardProd] UnlinkSubscription error: {ex.Message}");
            return false;
        }
    }

    // ── Clone Subscription (deep copy; CurrencyUsdTerminalIds cleared for uniqueness) ──
    public static async Task<BoardingCardProductSubscription?> CloneSubscriptionAsync(int sourceSubscriptionId)
    {
        try
        {
            using B2cNorthwindContext db = new();

            var source = await db.BoardingCardProductSubscriptions
                .AsNoTracking()
                .Include(s => s.CardProcessingConfigs)
                    .ThenInclude(c => c.ProcessorConfigs)
                        .ThenInclude(p => p.PaymentTypes)
                .FirstOrDefaultAsync(s => s.BoardingProductSubscriptionId == sourceSubscriptionId);

            if (source is null) return null;

            var newSub = new BoardingCardProductSubscription
            {
                BoardingTransactingMerchantId = null,
                ProductName                   = source.ProductName,
                ProductCategory               = source.ProductCategory,
                SubscriptionEnabled           = source.SubscriptionEnabled,
                EnablementStatus              = source.EnablementStatus,
                SelfServiceability            = source.SelfServiceability,
                Distributability              = source.Distributability,
                CardPresentEnabled            = source.CardPresentEnabled,
                CardNotPresentEnabled         = source.CardNotPresentEnabled,
                TemplateId                    = source.TemplateId,
                CreatedAt                     = DateTime.UtcNow
            };
            db.BoardingCardProductSubscriptions.Add(newSub);
            await db.SaveChangesAsync();

            foreach (var srcConfig in source.CardProcessingConfigs)
            {
                var newConfig = new BoardingCardProcessingConfig
                {
                    BoardingProductSubscriptionId     = newSub.BoardingProductSubscriptionId,
                    DefaultAuthTypeCode               = srcConfig.DefaultAuthTypeCode,
                    EnablePartialAuth                 = srcConfig.EnablePartialAuth,
                    MerchantCategoryCode              = srcConfig.MerchantCategoryCode,
                    EnableDuplicateRefNumBlocking     = srcConfig.EnableDuplicateRefNumBlocking,
                    AuthMerchantRetryDisabled         = srcConfig.AuthMerchantRetryDisabled,
                    IgnoreAddressVerificationSystem   = srcConfig.IgnoreAddressVerificationSystem,
                    VisaStraightThroughProcessingOnly = srcConfig.VisaStraightThroughProcessingOnly,
                    CardPresentSolutionType           = srcConfig.CardPresentSolutionType,
                    CardPresentProductSelected        = srcConfig.CardPresentProductSelected,
                    CpRelaxAddressVerificationSystem  = srcConfig.CpRelaxAddressVerificationSystem,
                    CpRelaxAvsAllowZipWithoutCountry  = srcConfig.CpRelaxAvsAllowZipWithoutCountry,
                    CpRelaxAvsAllowExpiredCard        = srcConfig.CpRelaxAvsAllowExpiredCard
                };
                db.BoardingCardProcessingConfigs.Add(newConfig);
                await db.SaveChangesAsync();

                foreach (var srcProc in srcConfig.ProcessorConfigs)
                {
                    var newProc = new BoardingProcessorConfig
                    {
                        BoardingCardProcessingConfigId         = newConfig.BoardingCardProcessingConfigId,
                        ProcessorName                          = srcProc.ProcessorName,
                        BatchGroup                             = srcProc.BatchGroup,
                        AcquirerCountryCode                    = srcProc.AcquirerCountryCode,
                        AcquirerFileDestinationBin             = srcProc.AcquirerFileDestinationBin,
                        AcquirerInterbankCardAssociationId     = srcProc.AcquirerInterbankCardAssociationId,
                        AcquirerInstitutionId                  = srcProc.AcquirerInstitutionId,
                        AcquirerDiscoverInstitutionId          = srcProc.AcquirerDiscoverInstitutionId,
                        AcquirerMerchantId                     = srcProc.AcquirerMerchantId,
                        EnableTransactionReferenceNumber       = srcProc.EnableTransactionReferenceNumber,
                        EnablePosNetworkSwitching              = srcProc.EnablePosNetworkSwitching,
                        MerchantTaxId                          = srcProc.MerchantTaxId,
                        AllowMultipleBills                     = srcProc.AllowMultipleBills,
                        BusinessApplicationId                  = srcProc.BusinessApplicationId,
                        EnableAutoAuthReversalAfterVoid        = srcProc.EnableAutoAuthReversalAfterVoid,
                        EnableExpresspayPanTranslation         = srcProc.EnableExpresspayPanTranslation,
                        QuasiCash                              = srcProc.QuasiCash,
                        DisablePointOfSaleTerminalIdValidation = srcProc.DisablePointOfSaleTerminalIdValidation,
                        EnablePinTranslation                   = srcProc.EnablePinTranslation,
                        DefaultPointOfSaleTerminalId           = srcProc.DefaultPointOfSaleTerminalId,
                        PointOfSaleTerminalIds                 = srcProc.PointOfSaleTerminalIds,
                        EnableMultipleTerminalIDs              = srcProc.EnableMultipleTerminalIDs,
                        PinDebitEnablePartialAuth              = srcProc.PinDebitEnablePartialAuth,
                        RelaxAddressVerificationSystem         = srcProc.RelaxAddressVerificationSystem,
                        RelaxAvsAllowZipWithoutCountry         = srcProc.RelaxAvsAllowZipWithoutCountry,
                        RelaxAvsAllowExpiredCard               = srcProc.RelaxAvsAllowExpiredCard,
                        EnableEmsTransactionRiskScore          = srcProc.EnableEmsTransactionRiskScore,
                        CurrencyUsdEnabled                     = srcProc.CurrencyUsdEnabled,
                        CurrencyUsdEnabledCardPresent          = srcProc.CurrencyUsdEnabledCardPresent,
                        CurrencyUsdEnabledCardNotPresent       = srcProc.CurrencyUsdEnabledCardNotPresent,
                        CurrencyUsdMerchantId                  = srcProc.CurrencyUsdMerchantId,
                        CurrencyUsdTerminalId                  = srcProc.CurrencyUsdTerminalId,
                        CurrencyUsdTerminalIds                 = null  // must be set uniquely per merchant before boarding
                    };
                    db.BoardingProcessorConfigs.Add(newProc);
                    await db.SaveChangesAsync();

                    foreach (var pt in srcProc.PaymentTypes)
                    {
                        db.BoardingProcessorPaymentTypes.Add(new BoardingProcessorPaymentType
                        {
                            BoardingProcessorConfigId = newProc.BoardingProcessorConfigId,
                            PaymentType               = pt.PaymentType,
                            Enabled                   = pt.Enabled,
                            MerchantId                = pt.MerchantId,
                            TerminalId                = pt.TerminalId,
                            EnabledCardPresent        = pt.EnabledCardPresent,
                            EnabledCardNotPresent     = pt.EnabledCardNotPresent
                        });
                    }
                    await db.SaveChangesAsync();
                }
            }

            return newSub;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingCardProd] CloneSubscription error: {ex.Message}");
            if (ex.InnerException is not null)
                Console.WriteLine($"[DBBoardingCardProd] CloneSubscription inner: {ex.InnerException.Message}");
            return null;
        }
    }

    // ── Create/Update Processor Config ────────────────────────────────────────
    public static async Task<BoardingProcessorConfig?> UpsertProcessorConfigAsync(BoardingProcessorConfigDto dto)
    {
        try
        {
            using B2cNorthwindContext db = new();

            BoardingProcessorConfig? entity;

            if (dto.BoardingProcessorConfigId > 0)
            {
                entity = await db.BoardingProcessorConfigs
                    .Include(p => p.PaymentTypes)
                    .FirstOrDefaultAsync(p => p.BoardingProcessorConfigId == dto.BoardingProcessorConfigId);
                if (entity is null) return null;
                db.BoardingProcessorPaymentTypes.RemoveRange(entity.PaymentTypes);
            }
            else
            {
                entity = new BoardingProcessorConfig();
                db.BoardingProcessorConfigs.Add(entity);
            }

            ApplyProcessorDtoToEntity(dto, entity);
            await db.SaveChangesAsync();

            foreach (var pt in dto.PaymentTypes)
            {
                db.BoardingProcessorPaymentTypes.Add(new BoardingProcessorPaymentType
                {
                    BoardingProcessorConfigId = entity.BoardingProcessorConfigId,
                    PaymentType               = pt.PaymentType ?? string.Empty,
                    Enabled                   = pt.Enabled,
                    MerchantId                = pt.MerchantId,
                    TerminalId                = pt.TerminalId,
                    EnabledCardPresent        = pt.EnabledCardPresent,
                    EnabledCardNotPresent     = pt.EnabledCardNotPresent
                });
            }
            await db.SaveChangesAsync();

            return entity;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingCardProd] UpsertProcessorConfig error: {ex.Message}");
            return null;
        }
    }

    // ── Delete Processor Config ────────────────────────────────────────────────
    public static async Task<bool> DeleteProcessorConfigAsync(int id)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingProcessorConfigs
                .Where(p => p.BoardingProcessorConfigId == id)
                .ExecuteDeleteAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingCardProd] DeleteProcessorConfig error: {ex.Message}");
            return false;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────
    private static async Task CreateCardProcessingConfigAsync(
        B2cNorthwindContext db,
        int subscriptionId,
        BoardingCardProcessingConfigDto configDto)
    {
        var config = new BoardingCardProcessingConfig
        {
            BoardingProductSubscriptionId      = subscriptionId,
            DefaultAuthTypeCode                = configDto.DefaultAuthTypeCode,
            EnablePartialAuth                  = configDto.EnablePartialAuth,
            MerchantCategoryCode               = configDto.MerchantCategoryCode,
            EnableDuplicateRefNumBlocking      = configDto.EnableDuplicateRefNumBlocking,
            AuthMerchantRetryDisabled          = configDto.AuthMerchantRetryDisabled,
            IgnoreAddressVerificationSystem    = configDto.IgnoreAddressVerificationSystem,
            VisaStraightThroughProcessingOnly  = configDto.VisaStraightThroughProcessingOnly,
            CardPresentSolutionType            = configDto.CardPresentSolutionType,
            CardPresentProductSelected         = configDto.CardPresentProductSelected,
            CpRelaxAddressVerificationSystem   = configDto.CpRelaxAddressVerificationSystem,
            CpRelaxAvsAllowZipWithoutCountry   = configDto.CpRelaxAvsAllowZipWithoutCountry,
            CpRelaxAvsAllowExpiredCard         = configDto.CpRelaxAvsAllowExpiredCard
        };
        db.BoardingCardProcessingConfigs.Add(config);
        await db.SaveChangesAsync();

        foreach (var procDto in configDto.ProcessorConfigs)
        {
            var proc = new BoardingProcessorConfig();
            ApplyProcessorDtoToEntity(procDto, proc);
            proc.BoardingCardProcessingConfigId = config.BoardingCardProcessingConfigId;
            db.BoardingProcessorConfigs.Add(proc);
            await db.SaveChangesAsync();

            foreach (var pt in procDto.PaymentTypes)
            {
                db.BoardingProcessorPaymentTypes.Add(new BoardingProcessorPaymentType
                {
                    BoardingProcessorConfigId = proc.BoardingProcessorConfigId,
                    PaymentType               = pt.PaymentType ?? string.Empty,
                    Enabled                   = pt.Enabled,
                    MerchantId                = pt.MerchantId,
                    TerminalId                = pt.TerminalId,
                    EnabledCardPresent        = pt.EnabledCardPresent,
                    EnabledCardNotPresent     = pt.EnabledCardNotPresent
                });
            }
            await db.SaveChangesAsync();
        }
    }

    private static void ApplyProcessorDtoToEntity(BoardingProcessorConfigDto dto, BoardingProcessorConfig entity)
    {
        entity.BoardingCardProcessingConfigId         = dto.BoardingCardProcessingConfigId;
        entity.ProcessorName                          = dto.ProcessorName ?? string.Empty;
        entity.BatchGroup                             = dto.BatchGroup;
        entity.AcquirerCountryCode                    = dto.AcquirerCountryCode;
        entity.AcquirerFileDestinationBin             = dto.AcquirerFileDestinationBin;
        entity.AcquirerInterbankCardAssociationId     = dto.AcquirerInterbankCardAssociationId;
        entity.AcquirerInstitutionId                  = dto.AcquirerInstitutionId;
        entity.AcquirerDiscoverInstitutionId          = dto.AcquirerDiscoverInstitutionId;
        entity.AcquirerMerchantId                     = dto.AcquirerMerchantId;
        entity.EnableTransactionReferenceNumber       = dto.EnableTransactionReferenceNumber;
        entity.EnablePosNetworkSwitching              = dto.EnablePosNetworkSwitching;
        entity.MerchantTaxId                          = dto.MerchantTaxId;
        entity.AllowMultipleBills                     = dto.AllowMultipleBills;
        entity.BusinessApplicationId                  = dto.BusinessApplicationId;
        entity.EnableAutoAuthReversalAfterVoid        = dto.EnableAutoAuthReversalAfterVoid;
        entity.EnableExpresspayPanTranslation         = dto.EnableExpresspayPanTranslation;
        entity.QuasiCash                              = dto.QuasiCash;
        entity.DisablePointOfSaleTerminalIdValidation = dto.DisablePointOfSaleTerminalIdValidation;
        entity.EnablePinTranslation                   = dto.EnablePinTranslation;
        entity.DefaultPointOfSaleTerminalId           = dto.DefaultPointOfSaleTerminalId;
        entity.PointOfSaleTerminalIds                 = dto.PointOfSaleTerminalIds;
        entity.EnableMultipleTerminalIDs              = dto.EnableMultipleTerminalIDs;
        entity.PinDebitEnablePartialAuth              = dto.PinDebitEnablePartialAuth;
        entity.RelaxAddressVerificationSystem         = dto.RelaxAddressVerificationSystem;
        entity.RelaxAvsAllowZipWithoutCountry         = dto.RelaxAvsAllowZipWithoutCountry;
        entity.RelaxAvsAllowExpiredCard               = dto.RelaxAvsAllowExpiredCard;
        entity.EnableEmsTransactionRiskScore          = dto.EnableEmsTransactionRiskScore;
        entity.CurrencyUsdEnabled                     = dto.CurrencyUsdEnabled;
        entity.CurrencyUsdEnabledCardPresent          = dto.CurrencyUsdEnabledCardPresent;
        entity.CurrencyUsdEnabledCardNotPresent       = dto.CurrencyUsdEnabledCardNotPresent;
        entity.CurrencyUsdMerchantId                  = dto.CurrencyUsdMerchantId;
        entity.CurrencyUsdTerminalId                  = dto.CurrencyUsdTerminalId;
        entity.CurrencyUsdTerminalIds                 = dto.CurrencyUsdTerminalIds;
    }

    // ── DTO projections ───────────────────────────────────────────────────────
    public static BoardingCardProductSubscriptionDto ToDto(BoardingCardProductSubscription entity)
    {
        var dto = new BoardingCardProductSubscriptionDto
        {
            BoardingProductSubscriptionId = entity.BoardingProductSubscriptionId,
            BoardingTransactingMerchantId = entity.BoardingTransactingMerchantId,
            ProductName                   = entity.ProductName,
            ProductCategory               = entity.ProductCategory,
            SubscriptionEnabled           = entity.SubscriptionEnabled,
            EnablementStatus              = entity.EnablementStatus,
            SelfServiceability            = entity.SelfServiceability,
            Distributability              = entity.Distributability,
            CardPresentEnabled            = entity.CardPresentEnabled,
            CardNotPresentEnabled         = entity.CardNotPresentEnabled,
            TemplateId                    = entity.TemplateId
        };

        var firstConfig = entity.CardProcessingConfigs.FirstOrDefault();
        if (firstConfig is not null)
        {
            dto.CardProcessingConfig = new BoardingCardProcessingConfigDto
            {
                BoardingCardProcessingConfigId   = firstConfig.BoardingCardProcessingConfigId,
                DefaultAuthTypeCode              = firstConfig.DefaultAuthTypeCode,
                EnablePartialAuth                = firstConfig.EnablePartialAuth,
                MerchantCategoryCode             = firstConfig.MerchantCategoryCode,
                EnableDuplicateRefNumBlocking    = firstConfig.EnableDuplicateRefNumBlocking,
                AuthMerchantRetryDisabled        = firstConfig.AuthMerchantRetryDisabled,
                IgnoreAddressVerificationSystem  = firstConfig.IgnoreAddressVerificationSystem,
                VisaStraightThroughProcessingOnly = firstConfig.VisaStraightThroughProcessingOnly,
                CardPresentSolutionType          = firstConfig.CardPresentSolutionType,
                CardPresentProductSelected       = firstConfig.CardPresentProductSelected,
                CpRelaxAddressVerificationSystem = firstConfig.CpRelaxAddressVerificationSystem,
                CpRelaxAvsAllowZipWithoutCountry = firstConfig.CpRelaxAvsAllowZipWithoutCountry,
                CpRelaxAvsAllowExpiredCard       = firstConfig.CpRelaxAvsAllowExpiredCard,
                ProcessorConfigs = firstConfig.ProcessorConfigs.Select(p => new BoardingProcessorConfigDto
                {
                    BoardingProcessorConfigId        = p.BoardingProcessorConfigId,
                    BoardingCardProcessingConfigId   = p.BoardingCardProcessingConfigId,
                    ProcessorName                    = p.ProcessorName,
                    BatchGroup                       = p.BatchGroup,
                    AcquirerCountryCode              = p.AcquirerCountryCode,
                    AcquirerFileDestinationBin       = p.AcquirerFileDestinationBin,
                    AcquirerInterbankCardAssociationId = p.AcquirerInterbankCardAssociationId,
                    AcquirerInstitutionId            = p.AcquirerInstitutionId,
                    AcquirerDiscoverInstitutionId    = p.AcquirerDiscoverInstitutionId,
                    AcquirerMerchantId               = p.AcquirerMerchantId,
                    EnableTransactionReferenceNumber = p.EnableTransactionReferenceNumber,
                    EnablePosNetworkSwitching        = p.EnablePosNetworkSwitching,
                    MerchantTaxId                    = p.MerchantTaxId,
                    AllowMultipleBills               = p.AllowMultipleBills,
                    BusinessApplicationId            = p.BusinessApplicationId,
                    EnableAutoAuthReversalAfterVoid  = p.EnableAutoAuthReversalAfterVoid,
                    EnableExpresspayPanTranslation   = p.EnableExpresspayPanTranslation,
                    QuasiCash                        = p.QuasiCash,
                    DisablePointOfSaleTerminalIdValidation = p.DisablePointOfSaleTerminalIdValidation,
                    EnablePinTranslation             = p.EnablePinTranslation,
                    DefaultPointOfSaleTerminalId     = p.DefaultPointOfSaleTerminalId,
                    PointOfSaleTerminalIds           = p.PointOfSaleTerminalIds,
                    EnableMultipleTerminalIDs        = p.EnableMultipleTerminalIDs,
                    PinDebitEnablePartialAuth        = p.PinDebitEnablePartialAuth,
                    RelaxAddressVerificationSystem   = p.RelaxAddressVerificationSystem,
                    RelaxAvsAllowZipWithoutCountry   = p.RelaxAvsAllowZipWithoutCountry,
                    RelaxAvsAllowExpiredCard         = p.RelaxAvsAllowExpiredCard,
                    EnableEmsTransactionRiskScore    = p.EnableEmsTransactionRiskScore,
                    CurrencyUsdEnabled               = p.CurrencyUsdEnabled,
                    CurrencyUsdEnabledCardPresent    = p.CurrencyUsdEnabledCardPresent,
                    CurrencyUsdEnabledCardNotPresent = p.CurrencyUsdEnabledCardNotPresent,
                    CurrencyUsdMerchantId            = p.CurrencyUsdMerchantId,
                    CurrencyUsdTerminalId            = p.CurrencyUsdTerminalId,
                    CurrencyUsdTerminalIds           = p.CurrencyUsdTerminalIds,
                    PaymentTypes = p.PaymentTypes.Select(t => new BoardingPaymentTypeDto
                    {
                        BoardingProcessorPaymentTypeId = t.BoardingProcessorPaymentTypeId,
                        PaymentType                    = t.PaymentType,
                        Enabled                        = t.Enabled,
                        MerchantId                     = t.MerchantId,
                        TerminalId                     = t.TerminalId,
                        EnabledCardPresent             = t.EnabledCardPresent,
                        EnabledCardNotPresent          = t.EnabledCardNotPresent
                    }).ToList()
                }).ToList()
            };
        }

        return dto;
    }
}
