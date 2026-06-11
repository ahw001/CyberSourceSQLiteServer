using Microsoft.EntityFrameworkCore;
using CybsClass.EntityModels;
using CybsClass.Cybersource.Models.DTOs;

namespace CybsClass.WebApi.Service.Services.DBOperations;

public static class DBSubmitTransactingFromSqlServices
{
    public static async Task<BoardingTransactingMerchant?> GetTransactingMerchantWithDetailsAsync(int boardingTransactingMerchantId)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            return await db.BoardingTransactingMerchants
                .AsNoTracking()
                .Include(m => m.BoardingOrganization)
                    .ThenInclude(o => o.Contacts)
                .FirstOrDefaultAsync(m => m.BoardingTransactingMerchantId == boardingTransactingMerchantId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBSubmitTransactingFromSql] GetDetails error: {ex.Message}");
            return null;
        }
    }

    public static async Task<BoardingCardProductSubscription?> GetCardSubscriptionForMerchantAsync(int boardingTransactingMerchantId)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            return await db.BoardingTransactingMerchantSubscriptions
                .AsNoTracking()
                .Where(j => j.BoardingTransactingMerchantId == boardingTransactingMerchantId)
                .Include(j => j.BoardingCardProductSubscription)
                    .ThenInclude(s => s.CardProcessingConfigs)
                        .ThenInclude(c => c.ProcessorConfigs)
                            .ThenInclude(p => p.PaymentTypes)
                .Select(j => j.BoardingCardProductSubscription)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBSubmitTransactingFromSql] GetCardSub error: {ex.Message}");
            return null;
        }
    }

    public static async Task<BoardingCardProductSubscription?> GetIncludedCardSubscriptionForMerchantAsync(int boardingTransactingMerchantId)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            return await db.BoardingTransactingMerchantSubscriptions
                .AsNoTracking()
                .Where(j => j.BoardingTransactingMerchantId == boardingTransactingMerchantId
                         && j.IncludeInBoarding
                         && (j.CybersourceBoardingStatus == null
                             || j.CybersourceBoardingStatus != "BOARDED"))
                .Include(j => j.BoardingCardProductSubscription)
                    .ThenInclude(s => s.CardProcessingConfigs)
                        .ThenInclude(c => c.ProcessorConfigs)
                            .ThenInclude(p => p.PaymentTypes)
                .Select(j => j.BoardingCardProductSubscription)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBSubmitTransactingFromSql] GetIncludedCardSub error: {ex.Message}");
            return null;
        }
    }

    public static async Task<List<string>> GetIncludedSupplementalProductTypesAsync(int boardingTransactingMerchantId)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            return await db.BoardingTransactingMerchantProductSubscriptions
                .AsNoTracking()
                .Where(j => j.BoardingTransactingMerchantId == boardingTransactingMerchantId
                         && j.IncludeInBoarding
                         && (j.CybersourceBoardingStatus == null || j.CybersourceBoardingStatus != "BOARDED"))
                .Select(j => j.ProductType)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBSubmitTransactingFromSql] GetIncludedSupplementalTypes error: {ex.Message}");
            return new List<string>();
        }
    }

    public static async Task<bool> UpdateCardSubscriptionBoardingStatusAsync(int boardingTransactingMerchantId, int boardingProductSubscriptionId, string status)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            return await db.BoardingTransactingMerchantSubscriptions
                .Where(j => j.BoardingTransactingMerchantId == boardingTransactingMerchantId
                         && j.BoardingProductSubscriptionId == boardingProductSubscriptionId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.CybersourceBoardingStatus, status)) > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBSubmitTransactingFromSql] UpdateCardSubStatus error: {ex.Message}");
            return false;
        }
    }

    public static async Task<bool> UpdateProductJunctionsBoardingStatusAsync(int boardingTransactingMerchantId, List<string> productTypes, string status)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            return await db.BoardingTransactingMerchantProductSubscriptions
                .Where(j => j.BoardingTransactingMerchantId == boardingTransactingMerchantId
                         && productTypes.Contains(j.ProductType))
                .ExecuteUpdateAsync(s => s.SetProperty(j => j.CybersourceBoardingStatus, status)) > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBSubmitTransactingFromSql] UpdateProductJunctions error: {ex.Message}");
            return false;
        }
    }

    public static async Task<List<string>> GetSupplementalProductTypesAsync(int boardingTransactingMerchantId)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            return await db.BoardingTransactingMerchantProductSubscriptions
                .AsNoTracking()
                .Where(j => j.BoardingTransactingMerchantId == boardingTransactingMerchantId)
                .Select(j => j.ProductType)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBSubmitTransactingFromSql] GetSupplementalTypes error: {ex.Message}");
            return new List<string>();
        }
    }

    public static async Task<BoardingValueAddedServicesSubscription?> GetVasSubscriptionForMerchantAsync(int boardingTransactingMerchantId)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var junction = await db.BoardingTransactingMerchantProductSubscriptions
                .AsNoTracking()
                .Where(j => j.BoardingTransactingMerchantId == boardingTransactingMerchantId
                         && j.ProductType == BoardingProductTypes.ValueAddedServices)
                .FirstOrDefaultAsync();
            if (junction is null) return null;
            return await db.BoardingValueAddedServicesSubscriptions
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.BoardingValueAddedServicesSubscriptionId == junction.ProductSubscriptionId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBSubmitTransactingFromSql] GetVasSub error: {ex.Message}");
            return null;
        }
    }

    public static async Task<BoardingValueAddedServicesSubscription?> GetIncludedVasSubscriptionForMerchantAsync(int boardingTransactingMerchantId)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var junction = await db.BoardingTransactingMerchantProductSubscriptions
                .AsNoTracking()
                .Where(j => j.BoardingTransactingMerchantId == boardingTransactingMerchantId
                         && j.ProductType == BoardingProductTypes.ValueAddedServices
                         && j.IncludeInBoarding
                         && (j.CybersourceBoardingStatus == null || j.CybersourceBoardingStatus != "BOARDED"))
                .FirstOrDefaultAsync();
            if (junction is null) return null;
            return await db.BoardingValueAddedServicesSubscriptions
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.BoardingValueAddedServicesSubscriptionId == junction.ProductSubscriptionId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBSubmitTransactingFromSql] GetIncludedVasSub error: {ex.Message}");
            return null;
        }
    }

    public static async Task<bool> UpdateCybersourceBoardingStatusAsync(int boardingTransactingMerchantId, string status)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var entity = await db.BoardingTransactingMerchants
                .FirstOrDefaultAsync(m => m.BoardingTransactingMerchantId == boardingTransactingMerchantId);
            if (entity is null) return false;
            entity.CybersourceBoardingStatus = status;
            entity.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBSubmitTransactingFromSql] UpdateStatus error: {ex.Message}");
            return false;
        }
    }
}
