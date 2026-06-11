using Microsoft.EntityFrameworkCore;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.WebApi.Service.Services.DBOperations;

public static class DBBoardingOrganizationServices
{
    // ── Create ──────────────────────────────────────────────────────────────
    public static async Task<BoardingOrganization?> CreateAsync(BoardingOrganizationDto dto)
    {
        try
        {
            var entity = MapFromDto(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            using B2cNorthwindContext db = new();
            db.BoardingOrganizations.Add(entity);
            await db.SaveChangesAsync();

            await UpsertContactsAsync(db, entity.BoardingOrganizationId, dto);
            return entity;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n\n[DBBoardingOrg] Create error: {ex}");
            return null;
        }
    }

    // ── Read All ─────────────────────────────────────────────────────────────
    public static async Task<List<BoardingOrganization>> GetAllAsync()
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingOrganizations
                .AsNoTracking()
                .Include(o => o.Contacts)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n\n[DBBoardingOrg] GetAll error: {ex}");
            return new List<BoardingOrganization>();
        }
    }

    // ── Read by ID ────────────────────────────────────────────────────────────
    public static async Task<BoardingOrganization?> GetByIdAsync(int id)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingOrganizations
                .AsNoTracking()
                .Include(o => o.Contacts)
                .FirstOrDefaultAsync(o => o.BoardingOrganizationId == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n\n[DBBoardingOrg] GetById error: {ex}");
            return null;
        }
    }

    // ── Read by OrganizationId string ─────────────────────────────────────────
    public static async Task<BoardingOrganization?> GetByOrganizationIdAsync(string organizationId)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingOrganizations
                .AsNoTracking()
                .Include(o => o.Contacts)
                .FirstOrDefaultAsync(o => o.OrganizationId == organizationId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n\n[DBBoardingOrg] GetByOrganizationId error: {ex}");
            return null;
        }
    }

    // ── Update ────────────────────────────────────────────────────────────────
    public static async Task<BoardingOrganization?> UpdateAsync(int id, BoardingOrganizationDto dto)
    {
        try
        {
            using B2cNorthwindContext db = new();
            var entity = await db.BoardingOrganizations
                .Include(o => o.Contacts)
                .FirstOrDefaultAsync(o => o.BoardingOrganizationId == id);

            if (entity is null) return null;

            ApplyDtoToEntity(dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            db.BoardingContacts.RemoveRange(entity.Contacts);
            await db.SaveChangesAsync();
            await UpsertContactsAsync(db, id, dto);

            return entity;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n\n[DBBoardingOrg] Update error: {ex}");
            return null;
        }
    }

    // ── Delete ────────────────────────────────────────────────────────────────
    public static async Task<bool> DeleteAsync(int id)
    {
        try
        {
            using B2cNorthwindContext db = new();
            return await db.BoardingOrganizations
                .Where(o => o.BoardingOrganizationId == id)
                .ExecuteDeleteAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n\n[DBBoardingOrg] Delete error: {ex}");
            return false;
        }
    }

    // ── Map helpers ───────────────────────────────────────────────────────────
    private static BoardingOrganization MapFromDto(BoardingOrganizationDto dto)
    {
        var entity = new BoardingOrganization();
        ApplyDtoToEntity(dto, entity);
        return entity;
    }

    private static void ApplyDtoToEntity(BoardingOrganizationDto dto, BoardingOrganization entity)
    {
        entity.OrganizationId         = dto.OrganizationId ?? string.Empty;
        entity.ParentOrganizationId   = dto.ParentOrganizationId;
        entity.Status                 = dto.Status;
        entity.Type                   = dto.Type;
        entity.Configurable           = dto.Configurable;
        entity.BoardingFlow           = dto.BoardingFlow;
        entity.Mode                   = dto.Mode;
        entity.BoardingPackageId      = dto.BoardingPackageId;
        entity.BusinessName           = dto.BusinessName;
        entity.WebsiteUrl             = dto.WebsiteUrl;
        entity.BusinessPhoneNumber    = dto.BusinessPhoneNumber;
        entity.TimeZone               = dto.TimeZone;
        entity.MerchantCategoryCode   = dto.MerchantCategoryCode;
        entity.AddressCountry         = dto.AddressCountry;
        entity.Address1               = dto.Address1;
        entity.PostalCode             = dto.PostalCode;
        entity.AdministrativeArea     = dto.AdministrativeArea;
        entity.Locality               = dto.Locality;
    }

    private static async Task UpsertContactsAsync(B2cNorthwindContext db, int orgId, BoardingOrganizationDto dto)
    {
        var contacts = new List<BoardingContact>();

        if (!string.IsNullOrWhiteSpace(dto.BusinessContactFirstName) || !string.IsNullOrWhiteSpace(dto.BusinessContactEmail))
            contacts.Add(new BoardingContact { BoardingOrganizationId = orgId, ContactType = "Business", FirstName = dto.BusinessContactFirstName, LastName = dto.BusinessContactLastName, PhoneNumber = dto.BusinessContactPhone, Email = dto.BusinessContactEmail });

        if (!string.IsNullOrWhiteSpace(dto.TechnicalContactFirstName) || !string.IsNullOrWhiteSpace(dto.TechnicalContactEmail))
            contacts.Add(new BoardingContact { BoardingOrganizationId = orgId, ContactType = "Technical", FirstName = dto.TechnicalContactFirstName, LastName = dto.TechnicalContactLastName, PhoneNumber = dto.TechnicalContactPhone, Email = dto.TechnicalContactEmail });

        if (!string.IsNullOrWhiteSpace(dto.EmergencyContactFirstName) || !string.IsNullOrWhiteSpace(dto.EmergencyContactEmail))
            contacts.Add(new BoardingContact { BoardingOrganizationId = orgId, ContactType = "Emergency", FirstName = dto.EmergencyContactFirstName, LastName = dto.EmergencyContactLastName, PhoneNumber = dto.EmergencyContactPhone, Email = dto.EmergencyContactEmail });

        if (contacts.Count > 0)
        {
            db.BoardingContacts.AddRange(contacts);
            await db.SaveChangesAsync();
        }
    }

    // ── DTO projection ────────────────────────────────────────────────────────
    public static BoardingOrganizationDto ToDto(BoardingOrganization entity)
    {
        var business  = entity.Contacts.FirstOrDefault(c => c.ContactType == "Business");
        var technical = entity.Contacts.FirstOrDefault(c => c.ContactType == "Technical");
        var emergency = entity.Contacts.FirstOrDefault(c => c.ContactType == "Emergency");

        return new BoardingOrganizationDto
        {
            BoardingOrganizationId       = entity.BoardingOrganizationId,
            OrganizationId               = entity.OrganizationId,
            ParentOrganizationId         = entity.ParentOrganizationId,
            Status                       = entity.Status,
            Type                         = entity.Type,
            Configurable                 = entity.Configurable,
            BoardingFlow                 = entity.BoardingFlow,
            Mode                         = entity.Mode,
            BoardingPackageId            = entity.BoardingPackageId,
            BusinessName                 = entity.BusinessName,
            WebsiteUrl                   = entity.WebsiteUrl,
            BusinessPhoneNumber          = entity.BusinessPhoneNumber,
            TimeZone                     = entity.TimeZone,
            MerchantCategoryCode         = entity.MerchantCategoryCode,
            AddressCountry               = entity.AddressCountry,
            Address1                     = entity.Address1,
            PostalCode                   = entity.PostalCode,
            AdministrativeArea           = entity.AdministrativeArea,
            Locality                     = entity.Locality,
            BusinessContactFirstName     = business?.FirstName,
            BusinessContactLastName      = business?.LastName,
            BusinessContactPhone         = business?.PhoneNumber,
            BusinessContactEmail         = business?.Email,
            TechnicalContactFirstName    = technical?.FirstName,
            TechnicalContactLastName     = technical?.LastName,
            TechnicalContactPhone        = technical?.PhoneNumber,
            TechnicalContactEmail        = technical?.Email,
            EmergencyContactFirstName    = emergency?.FirstName,
            EmergencyContactLastName     = emergency?.LastName,
            EmergencyContactPhone        = emergency?.PhoneNumber,
            EmergencyContactEmail        = emergency?.Email
        };
    }
}
