using Microsoft.EntityFrameworkCore;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.WebApi.Service.Services.DBOperations;

public static class DBBoardingTransactingMerchantServices
{
    // ── Create ───────────────────────────────────────────────────────────────
    public static async Task<BoardingTransactingMerchant?> CreateAsync(BoardingTransactingMerchantDto dto)
    {
        try
        {
            var entity = MapFromDto(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            using B2cNorthwindContext db = new();
            db.BoardingTransactingMerchants.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingTrans] Create error: {ex.Message}");
            return null;
        }
    }

    // ── Read All ──────────────────────────────────────────────────────────────
    public static async Task<List<BoardingTransactingMerchant>> GetAllAsync()
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingTransactingMerchants
                .AsNoTracking()
                .Include(t => t.BoardingOrganization)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingTrans] GetAll error: {ex.Message}");
            return new List<BoardingTransactingMerchant>();
        }
    }

    // ── Read by TransactingOrganizationId string ──────────────────────────────
    public static async Task<BoardingTransactingMerchant?> GetByTransactingOrgIdAsync(string transactingOrganizationId)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingTransactingMerchants
                .AsNoTracking()
                .Include(t => t.BoardingOrganization)
                .FirstOrDefaultAsync(t => t.TransactingOrganizationId == transactingOrganizationId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingTrans] GetByTransactingOrgId error: {ex.Message}");
            return null;
        }
    }

    // ── Read by Org ───────────────────────────────────────────────────────────
    public static async Task<List<BoardingTransactingMerchant>> GetByOrgIdAsync(int boardingOrganizationId)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingTransactingMerchants
                .AsNoTracking()
                .Where(t => t.BoardingOrganizationId == boardingOrganizationId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingTrans] GetByOrg error: {ex.Message}");
            return new List<BoardingTransactingMerchant>();
        }
    }

    // ── Read by ID ────────────────────────────────────────────────────────────
    public static async Task<BoardingTransactingMerchant?> GetByIdAsync(int id)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingTransactingMerchants
                .AsNoTracking()
                .Include(t => t.BoardingOrganization)
                .FirstOrDefaultAsync(t => t.BoardingTransactingMerchantId == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingTrans] GetById error: {ex.Message}");
            return null;
        }
    }

    // ── Update ────────────────────────────────────────────────────────────────
    public static async Task<BoardingTransactingMerchant?> UpdateAsync(int id, BoardingTransactingMerchantDto dto)
    {
        try
        {
            using B2cNorthwindContext db = new();
            var entity = await db.BoardingTransactingMerchants
                .FirstOrDefaultAsync(t => t.BoardingTransactingMerchantId == id);

            if (entity is null) return null;

            ApplyDtoToEntity(dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingTrans] Update error: {ex.Message}");
            return null;
        }
    }

    // ── Delete ────────────────────────────────────────────────────────────────
    public static async Task<bool> DeleteAsync(int id)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingTransactingMerchants
                .Where(t => t.BoardingTransactingMerchantId == id)
                .ExecuteDeleteAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingTrans] Delete error: {ex.Message}");
            return false;
        }
    }

    // ── Map helpers ───────────────────────────────────────────────────────────
    private static BoardingTransactingMerchant MapFromDto(BoardingTransactingMerchantDto dto)
    {
        var entity = new BoardingTransactingMerchant();
        ApplyDtoToEntity(dto, entity);
        return entity;
    }

    private static void ApplyDtoToEntity(BoardingTransactingMerchantDto dto, BoardingTransactingMerchant entity)
    {
        entity.BoardingOrganizationId      = dto.BoardingOrganizationId;
        entity.TransactingOrganizationId   = dto.TransactingOrganizationId ?? string.Empty;
        entity.Status                      = dto.Status;
        entity.Type                        = dto.Type;
        entity.Configurable                = dto.Configurable;
        entity.BusinessName                = dto.BusinessName;
        entity.WebsiteUrl                  = dto.WebsiteUrl;
        entity.BusinessPhoneNumber         = dto.BusinessPhoneNumber;
        entity.TimeZone                    = dto.TimeZone;
        entity.MerchantCategoryCode        = dto.MerchantCategoryCode;
        entity.AddressCountry              = dto.AddressCountry;
        entity.Address1                    = dto.Address1;
        entity.PostalCode                  = dto.PostalCode;
        entity.AdministrativeArea          = dto.AdministrativeArea;
        entity.Locality                    = dto.Locality;
        entity.BoardingProcessor           = dto.BoardingProcessor;
        entity.BoardingPackageId           = dto.BoardingPackageId;
    }

    // ── DTO projection ────────────────────────────────────────────────────────
    public static BoardingTransactingMerchantDto ToDto(BoardingTransactingMerchant entity)
    {
        return new BoardingTransactingMerchantDto
        {
            BoardingTransactingMerchantId = entity.BoardingTransactingMerchantId,
            BoardingOrganizationId        = entity.BoardingOrganizationId,
            TransactingOrganizationId     = entity.TransactingOrganizationId,
            Status                        = entity.Status,
            Type                          = entity.Type,
            Configurable                  = entity.Configurable,
            BusinessName                  = entity.BusinessName,
            WebsiteUrl                    = entity.WebsiteUrl,
            BusinessPhoneNumber           = entity.BusinessPhoneNumber,
            TimeZone                      = entity.TimeZone,
            MerchantCategoryCode          = entity.MerchantCategoryCode,
            AddressCountry                = entity.AddressCountry,
            Address1                      = entity.Address1,
            PostalCode                    = entity.PostalCode,
            AdministrativeArea            = entity.AdministrativeArea,
            Locality                      = entity.Locality,
            BoardingProcessor             = entity.BoardingProcessor,
            BoardingPackageId             = entity.BoardingPackageId,
            ParentOrganizationId          = entity.BoardingOrganization?.OrganizationId
        };
    }
}
