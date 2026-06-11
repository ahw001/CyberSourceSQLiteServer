using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.WebApi.Service.Services.DBOperations;

// =========================================================================
// One static class per supplemental product + one junction service. Each
// follows the existing DBBoarding* pattern: Create / Update / GetAll /
// GetById / Delete + ToDto projection. The direct BoardingTransactingMerchantId
// column on each product table is retained but optional — the polymorphic
// junction drives many-to-many linking.
// =========================================================================

// ── Digital Payments ─────────────────────────────────────────────────────
public static class DBBoardingDigitalPaymentsServices
{
    public static async Task<BoardingDigitalPaymentsSubscription?> CreateAsync(BoardingDigitalPaymentsSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = new BoardingDigitalPaymentsSubscription
            {
                BoardingTransactingMerchantId = null,
                Enabled            = dto.Enabled,
                EnablementStatus   = dto.EnablementStatus,
                SelfServiceability = dto.SelfServiceability,
                Distributability   = dto.Distributability,
                SamsungPayEnabled  = dto.SamsungPayEnabled,
                ApplePayEnabled    = dto.ApplePayEnabled,
                CreatedAt          = DateTime.UtcNow,
                UpdatedAt          = DateTime.UtcNow
            };
            db.BoardingDigitalPaymentsSubscriptions.Add(e);
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(CreateAsync), ex); return null; }
    }

    public static async Task<BoardingDigitalPaymentsSubscription?> UpdateAsync(int id, BoardingDigitalPaymentsSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = await db.BoardingDigitalPaymentsSubscriptions.FirstOrDefaultAsync(x => x.BoardingDigitalPaymentsSubscriptionId == id);
            if (e is null) return null;
            e.Enabled            = dto.Enabled;
            e.EnablementStatus   = dto.EnablementStatus;
            e.SelfServiceability = dto.SelfServiceability;
            e.Distributability   = dto.Distributability;
            e.SamsungPayEnabled  = dto.SamsungPayEnabled;
            e.ApplePayEnabled    = dto.ApplePayEnabled;
            e.UpdatedAt          = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(UpdateAsync), ex); return null; }
    }

    public static async Task<List<BoardingDigitalPaymentsSubscription>> GetAllAsync()
    {
        try { using var db = new B2cNorthwindContext();
              return await db.BoardingDigitalPaymentsSubscriptions.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync(); }
        catch (Exception ex) { LogErr(nameof(GetAllAsync), ex); return new(); }
    }

    public static async Task<BoardingDigitalPaymentsSubscription?> GetByIdAsync(int id)
    {
        try { using var db = new B2cNorthwindContext();
              return await db.BoardingDigitalPaymentsSubscriptions.AsNoTracking().FirstOrDefaultAsync(x => x.BoardingDigitalPaymentsSubscriptionId == id); }
        catch (Exception ex) { LogErr(nameof(GetByIdAsync), ex); return null; }
    }

    public static async Task<bool> DeleteAsync(int id)
    {
        try { using var db = new B2cNorthwindContext();
              await db.BoardingTransactingMerchantProductSubscriptions.Where(j => j.ProductType == BoardingProductTypes.DigitalPayments && j.ProductSubscriptionId == id).ExecuteDeleteAsync();
              return await db.BoardingDigitalPaymentsSubscriptions.Where(x => x.BoardingDigitalPaymentsSubscriptionId == id).ExecuteDeleteAsync() > 0; }
        catch (Exception ex) { LogErr(nameof(DeleteAsync), ex); return false; }
    }

    public static BoardingDigitalPaymentsSubscriptionDto ToDto(BoardingDigitalPaymentsSubscription e) => new()
    {
        BoardingDigitalPaymentsSubscriptionId = e.BoardingDigitalPaymentsSubscriptionId,
        Enabled            = e.Enabled,
        EnablementStatus   = e.EnablementStatus,
        SelfServiceability = e.SelfServiceability,
        Distributability   = e.Distributability,
        SamsungPayEnabled  = e.SamsungPayEnabled,
        ApplePayEnabled    = e.ApplePayEnabled
    };

    private static void LogErr(string method, Exception ex) =>
        Console.WriteLine($"[DBBoardingDigitalPayments] {method} error: {ex.Message}{(ex.InnerException is null ? "" : " | inner: " + ex.InnerException.Message)}");
}

// ── Invoicing ────────────────────────────────────────────────────────────
public static class DBBoardingInvoicingServices
{
    public static async Task<BoardingInvoicingSubscription?> CreateAsync(BoardingInvoicingSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = new BoardingInvoicingSubscription
            {
                BoardingTransactingMerchantId = null,
                Enabled            = dto.Enabled,
                EnablementStatus   = dto.EnablementStatus,
                SelfServiceability = dto.SelfServiceability,
                Distributability   = dto.Distributability,
                CreatedAt          = DateTime.UtcNow,
                UpdatedAt          = DateTime.UtcNow
            };
            db.BoardingInvoicingSubscriptions.Add(e);
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(CreateAsync), ex); return null; }
    }

    public static async Task<BoardingInvoicingSubscription?> UpdateAsync(int id, BoardingInvoicingSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = await db.BoardingInvoicingSubscriptions.FirstOrDefaultAsync(x => x.BoardingInvoicingSubscriptionId == id);
            if (e is null) return null;
            e.Enabled            = dto.Enabled;
            e.EnablementStatus   = dto.EnablementStatus;
            e.SelfServiceability = dto.SelfServiceability;
            e.Distributability   = dto.Distributability;
            e.UpdatedAt          = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(UpdateAsync), ex); return null; }
    }

    public static async Task<List<BoardingInvoicingSubscription>> GetAllAsync()
    {
        try { using var db = new B2cNorthwindContext();
              return await db.BoardingInvoicingSubscriptions.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync(); }
        catch (Exception ex) { LogErr(nameof(GetAllAsync), ex); return new(); }
    }

    public static async Task<BoardingInvoicingSubscription?> GetByIdAsync(int id)
    {
        try { using var db = new B2cNorthwindContext();
              return await db.BoardingInvoicingSubscriptions.AsNoTracking().FirstOrDefaultAsync(x => x.BoardingInvoicingSubscriptionId == id); }
        catch (Exception ex) { LogErr(nameof(GetByIdAsync), ex); return null; }
    }

    public static async Task<bool> DeleteAsync(int id)
    {
        try { using var db = new B2cNorthwindContext();
              await db.BoardingTransactingMerchantProductSubscriptions.Where(j => j.ProductType == BoardingProductTypes.CustomerInvoicing && j.ProductSubscriptionId == id).ExecuteDeleteAsync();
              return await db.BoardingInvoicingSubscriptions.Where(x => x.BoardingInvoicingSubscriptionId == id).ExecuteDeleteAsync() > 0; }
        catch (Exception ex) { LogErr(nameof(DeleteAsync), ex); return false; }
    }

    public static BoardingInvoicingSubscriptionDto ToDto(BoardingInvoicingSubscription e) => new()
    {
        BoardingInvoicingSubscriptionId = e.BoardingInvoicingSubscriptionId,
        Enabled            = e.Enabled,
        EnablementStatus   = e.EnablementStatus,
        SelfServiceability = e.SelfServiceability,
        Distributability   = e.Distributability
    };

    private static void LogErr(string method, Exception ex) =>
        Console.WriteLine($"[DBBoardingInvoicing] {method} error: {ex.Message}{(ex.InnerException is null ? "" : " | inner: " + ex.InnerException.Message)}");
}

// ── Pay By Link ──────────────────────────────────────────────────────────
public static class DBBoardingPayByLinkServices
{
    public static async Task<BoardingPayByLinkSubscription?> CreateAsync(BoardingPayByLinkSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = new BoardingPayByLinkSubscription
            {
                BoardingTransactingMerchantId = null,
                Enabled            = dto.Enabled,
                EnablementStatus   = dto.EnablementStatus,
                SelfServiceability = dto.SelfServiceability,
                Distributability   = dto.Distributability,
                CreatedAt          = DateTime.UtcNow,
                UpdatedAt          = DateTime.UtcNow
            };
            db.BoardingPayByLinkSubscriptions.Add(e);
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(CreateAsync), ex); return null; }
    }

    public static async Task<BoardingPayByLinkSubscription?> UpdateAsync(int id, BoardingPayByLinkSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = await db.BoardingPayByLinkSubscriptions.FirstOrDefaultAsync(x => x.BoardingPayByLinkSubscriptionId == id);
            if (e is null) return null;
            e.Enabled            = dto.Enabled;
            e.EnablementStatus   = dto.EnablementStatus;
            e.SelfServiceability = dto.SelfServiceability;
            e.Distributability   = dto.Distributability;
            e.UpdatedAt          = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(UpdateAsync), ex); return null; }
    }

    public static async Task<List<BoardingPayByLinkSubscription>> GetAllAsync()
    {
        try { using var db = new B2cNorthwindContext();
              return await db.BoardingPayByLinkSubscriptions.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync(); }
        catch (Exception ex) { LogErr(nameof(GetAllAsync), ex); return new(); }
    }

    public static async Task<BoardingPayByLinkSubscription?> GetByIdAsync(int id)
    {
        try { using var db = new B2cNorthwindContext();
              return await db.BoardingPayByLinkSubscriptions.AsNoTracking().FirstOrDefaultAsync(x => x.BoardingPayByLinkSubscriptionId == id); }
        catch (Exception ex) { LogErr(nameof(GetByIdAsync), ex); return null; }
    }

    public static async Task<bool> DeleteAsync(int id)
    {
        try { using var db = new B2cNorthwindContext();
              await db.BoardingTransactingMerchantProductSubscriptions.Where(j => j.ProductType == BoardingProductTypes.PayByLink && j.ProductSubscriptionId == id).ExecuteDeleteAsync();
              return await db.BoardingPayByLinkSubscriptions.Where(x => x.BoardingPayByLinkSubscriptionId == id).ExecuteDeleteAsync() > 0; }
        catch (Exception ex) { LogErr(nameof(DeleteAsync), ex); return false; }
    }

    public static BoardingPayByLinkSubscriptionDto ToDto(BoardingPayByLinkSubscription e) => new()
    {
        BoardingPayByLinkSubscriptionId = e.BoardingPayByLinkSubscriptionId,
        Enabled            = e.Enabled,
        EnablementStatus   = e.EnablementStatus,
        SelfServiceability = e.SelfServiceability,
        Distributability   = e.Distributability
    };

    private static void LogErr(string method, Exception ex) =>
        Console.WriteLine($"[DBBoardingPayByLink] {method} error: {ex.Message}{(ex.InnerException is null ? "" : " | inner: " + ex.InnerException.Message)}");
}

// ── Token Management ─────────────────────────────────────────────────────
public static class DBBoardingTokenManagementServices
{
    public static async Task<BoardingTokenManagementSubscription?> CreateAsync(BoardingTokenManagementSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = new BoardingTokenManagementSubscription();
            Apply(dto, e);
            e.CreatedAt = DateTime.UtcNow;
            e.UpdatedAt = DateTime.UtcNow;
            db.BoardingTokenManagementSubscriptions.Add(e);
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(CreateAsync), ex); return null; }
    }

    public static async Task<BoardingTokenManagementSubscription?> UpdateAsync(int id, BoardingTokenManagementSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = await db.BoardingTokenManagementSubscriptions.FirstOrDefaultAsync(x => x.BoardingTokenManagementSubscriptionId == id);
            if (e is null) return null;
            Apply(dto, e);
            e.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(UpdateAsync), ex); return null; }
    }

    private static void Apply(BoardingTokenManagementSubscriptionDto d, BoardingTokenManagementSubscription e)
    {
        e.Enabled                             = d.Enabled;
        e.EnablementStatus                    = d.EnablementStatus;
        e.SelfServiceability                  = d.SelfServiceability;
        e.Distributability                    = d.Distributability;
        e.CustomerTokenFormat                 = d.CustomerTokenFormat;
        e.InstrumentIdentifierCardTokenFormat = d.InstrumentIdentifierCardTokenFormat;
        e.PaymentInstrumentTokenFormat        = d.PaymentInstrumentTokenFormat;
        e.CardNumberMaskingFormat             = d.CardNumberMaskingFormat;
        e.VisaTokenServiceEnabled             = d.VisaTokenServiceEnabled;
        e.VisaTokenTransactionEnabled         = d.VisaTokenTransactionEnabled;
        e.MastercardTokenServiceEnabled       = d.MastercardTokenServiceEnabled;
        e.MastercardTokenTransactionEnabled   = d.MastercardTokenTransactionEnabled;
        e.CreateInstrumentIdentifierOnSuccess = d.CreateInstrumentIdentifierOnSuccess;
        e.CreateInstrumentIdentifierOnFailure = d.CreateInstrumentIdentifierOnFailure;
        e.ConfigurationStatus                 = d.ConfigurationStatus;
        e.ConfigurationMessage                = d.ConfigurationMessage;
    }

    public static async Task<List<BoardingTokenManagementSubscription>> GetAllAsync()
    {
        try { using var db = new B2cNorthwindContext();
              return await db.BoardingTokenManagementSubscriptions.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync(); }
        catch (Exception ex) { LogErr(nameof(GetAllAsync), ex); return new(); }
    }

    public static async Task<BoardingTokenManagementSubscription?> GetByIdAsync(int id)
    {
        try { using var db = new B2cNorthwindContext();
              return await db.BoardingTokenManagementSubscriptions.AsNoTracking().FirstOrDefaultAsync(x => x.BoardingTokenManagementSubscriptionId == id); }
        catch (Exception ex) { LogErr(nameof(GetByIdAsync), ex); return null; }
    }

    public static async Task<bool> DeleteAsync(int id)
    {
        try { using var db = new B2cNorthwindContext();
              await db.BoardingTransactingMerchantProductSubscriptions.Where(j => j.ProductType == BoardingProductTypes.TokenManagement && j.ProductSubscriptionId == id).ExecuteDeleteAsync();
              return await db.BoardingTokenManagementSubscriptions.Where(x => x.BoardingTokenManagementSubscriptionId == id).ExecuteDeleteAsync() > 0; }
        catch (Exception ex) { LogErr(nameof(DeleteAsync), ex); return false; }
    }

    public static BoardingTokenManagementSubscriptionDto ToDto(BoardingTokenManagementSubscription e) => new()
    {
        BoardingTokenManagementSubscriptionId = e.BoardingTokenManagementSubscriptionId,
        Enabled                             = e.Enabled,
        EnablementStatus                    = e.EnablementStatus,
        SelfServiceability                  = e.SelfServiceability,
        Distributability                    = e.Distributability,
        CustomerTokenFormat                 = e.CustomerTokenFormat,
        InstrumentIdentifierCardTokenFormat = e.InstrumentIdentifierCardTokenFormat,
        PaymentInstrumentTokenFormat        = e.PaymentInstrumentTokenFormat,
        CardNumberMaskingFormat             = e.CardNumberMaskingFormat,
        VisaTokenServiceEnabled             = e.VisaTokenServiceEnabled,
        VisaTokenTransactionEnabled         = e.VisaTokenTransactionEnabled,
        MastercardTokenServiceEnabled       = e.MastercardTokenServiceEnabled,
        MastercardTokenTransactionEnabled   = e.MastercardTokenTransactionEnabled,
        CreateInstrumentIdentifierOnSuccess = e.CreateInstrumentIdentifierOnSuccess,
        CreateInstrumentIdentifierOnFailure = e.CreateInstrumentIdentifierOnFailure,
        ConfigurationStatus                 = e.ConfigurationStatus,
        ConfigurationMessage                = e.ConfigurationMessage
    };

    private static void LogErr(string method, Exception ex) =>
        Console.WriteLine($"[DBBoardingTokenManagement] {method} error: {ex.Message}{(ex.InnerException is null ? "" : " | inner: " + ex.InnerException.Message)}");
}

// ── Unified Checkout ─────────────────────────────────────────────────────
public static class DBBoardingUnifiedCheckoutServices
{
    public static async Task<BoardingUnifiedCheckoutSubscription?> CreateAsync(BoardingUnifiedCheckoutSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = new BoardingUnifiedCheckoutSubscription
            {
                BoardingTransactingMerchantId = null,
                Enabled                  = dto.Enabled,
                EnablementStatus         = dto.EnablementStatus,
                SelfServiceability       = dto.SelfServiceability,
                Distributability         = dto.Distributability,
                ConfigurationStatus      = dto.ConfigurationStatus,
                ConfigurationMessage     = dto.ConfigurationMessage,
                ApplePayEnabled          = dto.ApplePayEnabled,
                ClickToPayEnabled        = dto.ClickToPayEnabled,
                ECheckEnabled            = dto.ECheckEnabled,
                GooglePayEnabled         = dto.GooglePayEnabled,
                DecisionManagerEnabled   = dto.DecisionManagerEnabled,
                PayerAuthenticationEnabled = dto.PayerAuthenticationEnabled,
                TokenManagementEnabled   = dto.TokenManagementEnabled,
                CreatedAt                = DateTime.UtcNow,
                UpdatedAt                = DateTime.UtcNow
            };
            db.BoardingUnifiedCheckoutSubscriptions.Add(e);
            await db.SaveChangesAsync();
            foreach (var card in dto.AllowedCardNetworks.Where(c => !string.IsNullOrWhiteSpace(c)))
            {
                db.BoardingUnifiedCheckoutAllowedCardNetworks.Add(new BoardingUnifiedCheckoutAllowedCardNetwork
                {
                    BoardingUnifiedCheckoutSubscriptionId = e.BoardingUnifiedCheckoutSubscriptionId,
                    CardNetwork                           = card
                });
            }
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(CreateAsync), ex); return null; }
    }

    public static async Task<BoardingUnifiedCheckoutSubscription?> UpdateAsync(int id, BoardingUnifiedCheckoutSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = await db.BoardingUnifiedCheckoutSubscriptions.FirstOrDefaultAsync(x => x.BoardingUnifiedCheckoutSubscriptionId == id);
            if (e is null) return null;
            e.Enabled                  = dto.Enabled;
            e.EnablementStatus         = dto.EnablementStatus;
            e.SelfServiceability       = dto.SelfServiceability;
            e.Distributability         = dto.Distributability;
            e.ConfigurationStatus      = dto.ConfigurationStatus;
            e.ConfigurationMessage     = dto.ConfigurationMessage;
            e.ApplePayEnabled          = dto.ApplePayEnabled;
            e.ClickToPayEnabled        = dto.ClickToPayEnabled;
            e.ECheckEnabled            = dto.ECheckEnabled;
            e.GooglePayEnabled         = dto.GooglePayEnabled;
            e.DecisionManagerEnabled   = dto.DecisionManagerEnabled;
            e.PayerAuthenticationEnabled = dto.PayerAuthenticationEnabled;
            e.TokenManagementEnabled   = dto.TokenManagementEnabled;
            e.UpdatedAt                = DateTime.UtcNow;

            await db.BoardingUnifiedCheckoutAllowedCardNetworks
                .Where(c => c.BoardingUnifiedCheckoutSubscriptionId == id)
                .ExecuteDeleteAsync();
            foreach (var card in dto.AllowedCardNetworks.Where(c => !string.IsNullOrWhiteSpace(c)))
            {
                db.BoardingUnifiedCheckoutAllowedCardNetworks.Add(new BoardingUnifiedCheckoutAllowedCardNetwork
                {
                    BoardingUnifiedCheckoutSubscriptionId = id,
                    CardNetwork                           = card
                });
            }
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(UpdateAsync), ex); return null; }
    }

    public static async Task<List<BoardingUnifiedCheckoutSubscription>> GetAllAsync()
    {
        try { using var db = new B2cNorthwindContext();
              return await db.BoardingUnifiedCheckoutSubscriptions.AsNoTracking().Include(x => x.AllowedCardNetworks).OrderByDescending(x => x.CreatedAt).ToListAsync(); }
        catch (Exception ex) { LogErr(nameof(GetAllAsync), ex); return new(); }
    }

    public static async Task<BoardingUnifiedCheckoutSubscription?> GetByIdAsync(int id)
    {
        try { using var db = new B2cNorthwindContext();
              return await db.BoardingUnifiedCheckoutSubscriptions.AsNoTracking().Include(x => x.AllowedCardNetworks).FirstOrDefaultAsync(x => x.BoardingUnifiedCheckoutSubscriptionId == id); }
        catch (Exception ex) { LogErr(nameof(GetByIdAsync), ex); return null; }
    }

    public static async Task<bool> DeleteAsync(int id)
    {
        try { using var db = new B2cNorthwindContext();
              await db.BoardingTransactingMerchantProductSubscriptions.Where(j => j.ProductType == BoardingProductTypes.UnifiedCheckout && j.ProductSubscriptionId == id).ExecuteDeleteAsync();
              await db.BoardingUnifiedCheckoutAllowedCardNetworks.Where(c => c.BoardingUnifiedCheckoutSubscriptionId == id).ExecuteDeleteAsync();
              return await db.BoardingUnifiedCheckoutSubscriptions.Where(x => x.BoardingUnifiedCheckoutSubscriptionId == id).ExecuteDeleteAsync() > 0; }
        catch (Exception ex) { LogErr(nameof(DeleteAsync), ex); return false; }
    }

    public static BoardingUnifiedCheckoutSubscriptionDto ToDto(BoardingUnifiedCheckoutSubscription e) => new()
    {
        BoardingUnifiedCheckoutSubscriptionId = e.BoardingUnifiedCheckoutSubscriptionId,
        Enabled                  = e.Enabled,
        EnablementStatus         = e.EnablementStatus,
        SelfServiceability       = e.SelfServiceability,
        Distributability         = e.Distributability,
        ConfigurationStatus      = e.ConfigurationStatus,
        ConfigurationMessage     = e.ConfigurationMessage,
        ApplePayEnabled          = e.ApplePayEnabled,
        ClickToPayEnabled        = e.ClickToPayEnabled,
        ECheckEnabled            = e.ECheckEnabled,
        GooglePayEnabled         = e.GooglePayEnabled,
        DecisionManagerEnabled   = e.DecisionManagerEnabled,
        PayerAuthenticationEnabled = e.PayerAuthenticationEnabled,
        TokenManagementEnabled   = e.TokenManagementEnabled,
        AllowedCardNetworks      = e.AllowedCardNetworks.Select(c => c.CardNetwork).ToList()
    };

    private static void LogErr(string method, Exception ex) =>
        Console.WriteLine($"[DBBoardingUnifiedCheckout] {method} error: {ex.Message}{(ex.InnerException is null ? "" : " | inner: " + ex.InnerException.Message)}");
}

// ── Value Added Services ─────────────────────────────────────────────────
public static class DBBoardingValueAddedServicesServices
{
    public static async Task<BoardingValueAddedServicesSubscription?> CreateAsync(BoardingValueAddedServicesSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = new BoardingValueAddedServicesSubscription();
            Apply(dto, e);
            e.CreatedAt = DateTime.UtcNow;
            e.UpdatedAt = DateTime.UtcNow;
            db.BoardingValueAddedServicesSubscriptions.Add(e);
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(CreateAsync), ex); return null; }
    }

    public static async Task<BoardingValueAddedServicesSubscription?> UpdateAsync(int id, BoardingValueAddedServicesSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = await db.BoardingValueAddedServicesSubscriptions.FirstOrDefaultAsync(x => x.BoardingValueAddedServicesSubscriptionId == id);
            if (e is null) return null;
            Apply(dto, e);
            e.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(UpdateAsync), ex); return null; }
    }

    private static void Apply(BoardingValueAddedServicesSubscriptionDto d, BoardingValueAddedServicesSubscription e)
    {
        e.TransactionSearchEnabled            = d.TransactionSearchEnabled;
        e.TransactionSearchEnablementStatus   = d.TransactionSearchEnablementStatus;
        e.TransactionSearchSelfServiceability = d.TransactionSearchSelfServiceability;
        e.TransactionSearchDistributability   = d.TransactionSearchDistributability;
        e.ReportingEnabled                    = d.ReportingEnabled;
        e.ReportingEnablementStatus           = d.ReportingEnablementStatus;
        e.ReportingSelfServiceability         = d.ReportingSelfServiceability;
        e.ReportingDistributability           = d.ReportingDistributability;
        e.DisputeManagementEnabled            = d.DisputeManagementEnabled;
    }

    public static async Task<List<BoardingValueAddedServicesSubscription>> GetAllAsync()
    {
        try { using var db = new B2cNorthwindContext();
              return await db.BoardingValueAddedServicesSubscriptions.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync(); }
        catch (Exception ex) { LogErr(nameof(GetAllAsync), ex); return new(); }
    }

    public static async Task<BoardingValueAddedServicesSubscription?> GetByIdAsync(int id)
    {
        try { using var db = new B2cNorthwindContext();
              return await db.BoardingValueAddedServicesSubscriptions.AsNoTracking().FirstOrDefaultAsync(x => x.BoardingValueAddedServicesSubscriptionId == id); }
        catch (Exception ex) { LogErr(nameof(GetByIdAsync), ex); return null; }
    }

    public static async Task<bool> DeleteAsync(int id)
    {
        try { using var db = new B2cNorthwindContext();
              await db.BoardingTransactingMerchantProductSubscriptions.Where(j => j.ProductType == BoardingProductTypes.ValueAddedServices && j.ProductSubscriptionId == id).ExecuteDeleteAsync();
              return await db.BoardingValueAddedServicesSubscriptions.Where(x => x.BoardingValueAddedServicesSubscriptionId == id).ExecuteDeleteAsync() > 0; }
        catch (Exception ex) { LogErr(nameof(DeleteAsync), ex); return false; }
    }

    public static BoardingValueAddedServicesSubscriptionDto ToDto(BoardingValueAddedServicesSubscription e) => new()
    {
        BoardingValueAddedServicesSubscriptionId = e.BoardingValueAddedServicesSubscriptionId,
        TransactionSearchEnabled            = e.TransactionSearchEnabled,
        TransactionSearchEnablementStatus   = e.TransactionSearchEnablementStatus,
        TransactionSearchSelfServiceability = e.TransactionSearchSelfServiceability,
        TransactionSearchDistributability   = e.TransactionSearchDistributability,
        ReportingEnabled                    = e.ReportingEnabled,
        ReportingEnablementStatus           = e.ReportingEnablementStatus,
        ReportingSelfServiceability         = e.ReportingSelfServiceability,
        ReportingDistributability           = e.ReportingDistributability,
        DisputeManagementEnabled            = e.DisputeManagementEnabled
    };

    private static void LogErr(string method, Exception ex) =>
        Console.WriteLine($"[DBBoardingVas] {method} error: {ex.Message}{(ex.InnerException is null ? "" : " | inner: " + ex.InnerException.Message)}");
}

// ── Virtual Terminal ─────────────────────────────────────────────────────
public static class DBBoardingVirtualTerminalServices
{
    private static readonly JsonSerializerOptions _jsonOpts = new(JsonSerializerDefaults.Web);

    public static async Task<BoardingVirtualTerminalSubscription?> CreateAsync(BoardingVirtualTerminalSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = new BoardingVirtualTerminalSubscription();
            ApplyParent(dto, e);
            e.CreatedAt = DateTime.UtcNow;
            e.UpdatedAt = DateTime.UtcNow;
            db.BoardingVirtualTerminalSubscriptions.Add(e);
            await db.SaveChangesAsync();

            ApplyChildren(db, e.BoardingVirtualTerminalSubscriptionId, dto);
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(CreateAsync), ex); return null; }
    }

    public static async Task<BoardingVirtualTerminalSubscription?> UpdateAsync(int id, BoardingVirtualTerminalSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = await db.BoardingVirtualTerminalSubscriptions.FirstOrDefaultAsync(x => x.BoardingVirtualTerminalSubscriptionId == id);
            if (e is null) return null;
            ApplyParent(dto, e);
            e.UpdatedAt = DateTime.UtcNow;

            await db.BoardingVirtualTerminalGlobalPaymentInfos.Where(x => x.BoardingVirtualTerminalSubscriptionId == id).ExecuteDeleteAsync();
            await db.BoardingVirtualTerminalReceiptInfos.Where(x => x.BoardingVirtualTerminalSubscriptionId == id).ExecuteDeleteAsync();
            await db.BoardingVirtualTerminalReaderInfos.Where(x => x.BoardingVirtualTerminalSubscriptionId == id).ExecuteDeleteAsync();
            await db.BoardingVirtualTerminalAcceptedCardTypes.Where(x => x.BoardingVirtualTerminalSubscriptionId == id).ExecuteDeleteAsync();
            await db.BoardingVirtualTerminalMerchantDefinedFields.Where(x => x.BoardingVirtualTerminalSubscriptionId == id).ExecuteDeleteAsync();

            ApplyChildren(db, id, dto);
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(UpdateAsync), ex); return null; }
    }

    private static void ApplyParent(BoardingVirtualTerminalSubscriptionDto d, BoardingVirtualTerminalSubscription e)
    {
        e.Enabled                   = d.Enabled;
        e.EnablementStatus          = d.EnablementStatus;
        e.SelfServiceability        = d.SelfServiceability;
        e.Distributability          = d.Distributability;
        e.ConfigurationStatus       = d.ConfigurationStatus;
        e.AllowECheckFields         = d.AllowECheckFields;
        e.AllowLevel3Fields         = d.AllowLevel3Fields;
        e.AllowServiceFeeFields     = d.AllowServiceFeeFields;
        e.ProductProfileEnabled     = d.ProductProfileEnabled;
        e.MerchantCountry           = d.MerchantCountry;
        e.AccountLevelEnabled       = d.AccountLevelEnabled;
        e.TokenProvider             = d.TokenProvider;
        e.SecureStorageEnabled      = d.SecureStorageEnabled;
        e.OtsTokenClass             = d.OtsTokenClass;
        e.OtsProfileId              = d.OtsProfileId;
        e.CardProcessingType        = d.CardProcessingType;
        e.DefaultTransactionMethod  = d.DefaultTransactionMethod;
    }

    private static void ApplyChildren(B2cNorthwindContext db, int subscriptionId, BoardingVirtualTerminalSubscriptionDto dto)
    {
        var gpi = DeserializeOrNull<BoardingVirtualTerminalGlobalPaymentInfo>(dto.GlobalPaymentInfoJson);
        if (gpi is not null) { gpi.BoardingVirtualTerminalSubscriptionId = subscriptionId; gpi.BoardingVirtualTerminalGlobalPaymentInfoId = 0; db.BoardingVirtualTerminalGlobalPaymentInfos.Add(gpi); }

        var ri = DeserializeOrNull<BoardingVirtualTerminalReceiptInfo>(dto.ReceiptInfoJson);
        if (ri is not null) { ri.BoardingVirtualTerminalSubscriptionId = subscriptionId; ri.BoardingVirtualTerminalReceiptInfoId = 0; db.BoardingVirtualTerminalReceiptInfos.Add(ri); }

        var rdr = DeserializeOrNull<BoardingVirtualTerminalReaderInfo>(dto.ReaderInfoJson);
        if (rdr is not null) { rdr.BoardingVirtualTerminalSubscriptionId = subscriptionId; rdr.BoardingVirtualTerminalReaderInfoId = 0; db.BoardingVirtualTerminalReaderInfos.Add(rdr); }

        foreach (var ct in dto.AcceptedCardTypes.Where(c => !string.IsNullOrWhiteSpace(c.ListType) && !string.IsNullOrWhiteSpace(c.CardType)))
        {
            db.BoardingVirtualTerminalAcceptedCardTypes.Add(new BoardingVirtualTerminalAcceptedCardType
            {
                BoardingVirtualTerminalSubscriptionId = subscriptionId,
                ListType = ct.ListType!,
                CardType = ct.CardType!
            });
        }

        foreach (var mdf in dto.MerchantDefinedFields.Where(f => f.FieldIndex is >= 1 and <= 20))
        {
            db.BoardingVirtualTerminalMerchantDefinedFields.Add(new BoardingVirtualTerminalMerchantDefinedField
            {
                BoardingVirtualTerminalSubscriptionId = subscriptionId,
                FieldIndex            = mdf.FieldIndex,
                DisplayField          = mdf.DisplayField,
                RequiredField         = mdf.RequiredField,
                ShowReceipt           = mdf.ShowReceipt,
                ReceiptDisplayEnabled = mdf.ReceiptDisplayEnabled
            });
        }
    }

    private static T? DeserializeOrNull<T>(string? json) where T : class
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        try { return JsonSerializer.Deserialize<T>(json, _jsonOpts); }
        catch (JsonException ex) { Console.WriteLine($"[DBBoardingVirtualTerminal] JSON parse failed for {typeof(T).Name}: {ex.Message}"); return null; }
    }

    public static async Task<List<BoardingVirtualTerminalSubscription>> GetAllAsync()
    {
        try { using var db = new B2cNorthwindContext();
              return await db.BoardingVirtualTerminalSubscriptions.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync(); }
        catch (Exception ex) { LogErr(nameof(GetAllAsync), ex); return new(); }
    }

    public static async Task<BoardingVirtualTerminalSubscription?> GetByIdAsync(int id)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            return await db.BoardingVirtualTerminalSubscriptions.AsNoTracking()
                .Include(x => x.GlobalPaymentInfo)
                .Include(x => x.ReceiptInfo)
                .Include(x => x.ReaderInfo)
                .Include(x => x.AcceptedCardTypes)
                .Include(x => x.MerchantDefinedFields)
                .FirstOrDefaultAsync(x => x.BoardingVirtualTerminalSubscriptionId == id);
        }
        catch (Exception ex) { LogErr(nameof(GetByIdAsync), ex); return null; }
    }

    public static async Task<bool> DeleteAsync(int id)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            await db.BoardingTransactingMerchantProductSubscriptions.Where(j => j.ProductType == BoardingProductTypes.VirtualTerminal && j.ProductSubscriptionId == id).ExecuteDeleteAsync();
            await db.BoardingVirtualTerminalGlobalPaymentInfos.Where(x => x.BoardingVirtualTerminalSubscriptionId == id).ExecuteDeleteAsync();
            await db.BoardingVirtualTerminalReceiptInfos.Where(x => x.BoardingVirtualTerminalSubscriptionId == id).ExecuteDeleteAsync();
            await db.BoardingVirtualTerminalReaderInfos.Where(x => x.BoardingVirtualTerminalSubscriptionId == id).ExecuteDeleteAsync();
            await db.BoardingVirtualTerminalAcceptedCardTypes.Where(x => x.BoardingVirtualTerminalSubscriptionId == id).ExecuteDeleteAsync();
            await db.BoardingVirtualTerminalMerchantDefinedFields.Where(x => x.BoardingVirtualTerminalSubscriptionId == id).ExecuteDeleteAsync();
            return await db.BoardingVirtualTerminalSubscriptions.Where(x => x.BoardingVirtualTerminalSubscriptionId == id).ExecuteDeleteAsync() > 0;
        }
        catch (Exception ex) { LogErr(nameof(DeleteAsync), ex); return false; }
    }

    public static BoardingVirtualTerminalSubscriptionDto ToDto(BoardingVirtualTerminalSubscription e)
    {
        var dto = new BoardingVirtualTerminalSubscriptionDto
        {
            BoardingVirtualTerminalSubscriptionId = e.BoardingVirtualTerminalSubscriptionId,
            Enabled                   = e.Enabled,
            EnablementStatus          = e.EnablementStatus,
            SelfServiceability        = e.SelfServiceability,
            Distributability          = e.Distributability,
            ConfigurationStatus       = e.ConfigurationStatus,
            AllowECheckFields         = e.AllowECheckFields,
            AllowLevel3Fields         = e.AllowLevel3Fields,
            AllowServiceFeeFields     = e.AllowServiceFeeFields,
            ProductProfileEnabled     = e.ProductProfileEnabled,
            MerchantCountry           = e.MerchantCountry,
            AccountLevelEnabled       = e.AccountLevelEnabled,
            TokenProvider             = e.TokenProvider,
            SecureStorageEnabled      = e.SecureStorageEnabled,
            OtsTokenClass             = e.OtsTokenClass,
            OtsProfileId              = e.OtsProfileId,
            CardProcessingType        = e.CardProcessingType,
            DefaultTransactionMethod  = e.DefaultTransactionMethod
        };
        if (e.GlobalPaymentInfo is not null) dto.GlobalPaymentInfoJson = JsonSerializer.Serialize(e.GlobalPaymentInfo, _jsonOpts);
        if (e.ReceiptInfo       is not null) dto.ReceiptInfoJson       = JsonSerializer.Serialize(e.ReceiptInfo, _jsonOpts);
        if (e.ReaderInfo        is not null) dto.ReaderInfoJson        = JsonSerializer.Serialize(e.ReaderInfo, _jsonOpts);
        dto.AcceptedCardTypes = e.AcceptedCardTypes.Select(c => new BoardingVirtualTerminalAcceptedCardTypeDto
        {
            BoardingVirtualTerminalAcceptedCardTypeId = c.BoardingVirtualTerminalAcceptedCardTypeId,
            ListType = c.ListType,
            CardType = c.CardType
        }).ToList();
        dto.MerchantDefinedFields = e.MerchantDefinedFields.Select(f => new BoardingVirtualTerminalMerchantDefinedFieldDto
        {
            BoardingVirtualTerminalMerchantDefinedFieldId = f.BoardingVirtualTerminalMerchantDefinedFieldId,
            FieldIndex            = f.FieldIndex,
            DisplayField          = f.DisplayField,
            RequiredField         = f.RequiredField,
            ShowReceipt           = f.ShowReceipt,
            ReceiptDisplayEnabled = f.ReceiptDisplayEnabled
        }).ToList();
        return dto;
    }

    private static void LogErr(string method, Exception ex) =>
        Console.WriteLine($"[DBBoardingVirtualTerminal] {method} error: {ex.Message}{(ex.InnerException is null ? "" : " | inner: " + ex.InnerException.Message)}");
}

// ── Payer Authentication ─────────────────────────────────────────────────
public static class DBBoardingPayerAuthenticationServices
{
    public static async Task<BoardingPayerAuthenticationSubscription?> CreateAsync(BoardingPayerAuthenticationSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = new BoardingPayerAuthenticationSubscription
            {
                BoardingTransactingMerchantId = null,
                Enabled            = dto.Enabled,
                EnablementStatus   = dto.EnablementStatus,
                SelfServiceability = dto.SelfServiceability,
                Distributability   = dto.Distributability,
                TemplateId         = dto.TemplateId,
                CreatedAt          = DateTime.UtcNow,
                UpdatedAt          = DateTime.UtcNow,
                CardTypeConfigs    = dto.CardTypeConfigs
                    .Where(ct => !string.IsNullOrWhiteSpace(ct.CardTypeName))
                    .Select(ct => new BoardingPayerAuthenticationCardTypeConfig
                    {
                        CardTypeName = ct.CardTypeName!,
                        Currencies   = ct.Currencies.Select(cur => new BoardingPayerAuthenticationCurrency
                        {
                            CurrencyCodes       = cur.CurrencyCodes,
                            AcquirerId          = cur.AcquirerId,
                            ProcessorMerchantId = cur.ProcessorMerchantId
                        }).ToList()
                    }).ToList()
            };
            db.BoardingPayerAuthenticationSubscriptions.Add(e);
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(CreateAsync), ex); return null; }
    }

    public static async Task<BoardingPayerAuthenticationSubscription?> UpdateAsync(int id, BoardingPayerAuthenticationSubscriptionDto dto)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            var e = await db.BoardingPayerAuthenticationSubscriptions.FirstOrDefaultAsync(x => x.BoardingPayerAuthenticationSubscriptionId == id);
            if (e is null) return null;
            e.Enabled            = dto.Enabled;
            e.EnablementStatus   = dto.EnablementStatus;
            e.SelfServiceability = dto.SelfServiceability;
            e.Distributability   = dto.Distributability;
            e.TemplateId         = dto.TemplateId;
            e.UpdatedAt          = DateTime.UtcNow;

            var configIds = await db.BoardingPayerAuthenticationCardTypeConfigs
                .Where(c => c.BoardingPayerAuthenticationSubscriptionId == id)
                .Select(c => c.BoardingPayerAuthenticationCardTypeConfigId)
                .ToListAsync();
            if (configIds.Count > 0)
            {
                await db.BoardingPayerAuthenticationCurrencies
                    .Where(cur => configIds.Contains(cur.BoardingPayerAuthenticationCardTypeConfigId))
                    .ExecuteDeleteAsync();
                await db.BoardingPayerAuthenticationCardTypeConfigs
                    .Where(c => c.BoardingPayerAuthenticationSubscriptionId == id)
                    .ExecuteDeleteAsync();
            }

            foreach (var ct in dto.CardTypeConfigs.Where(c => !string.IsNullOrWhiteSpace(c.CardTypeName)))
            {
                var ctEntity = new BoardingPayerAuthenticationCardTypeConfig
                {
                    BoardingPayerAuthenticationSubscriptionId = id,
                    CardTypeName = ct.CardTypeName!,
                    Currencies   = ct.Currencies.Select(cur => new BoardingPayerAuthenticationCurrency
                    {
                        CurrencyCodes       = cur.CurrencyCodes,
                        AcquirerId          = cur.AcquirerId,
                        ProcessorMerchantId = cur.ProcessorMerchantId
                    }).ToList()
                };
                db.BoardingPayerAuthenticationCardTypeConfigs.Add(ctEntity);
            }
            await db.SaveChangesAsync();
            return e;
        }
        catch (Exception ex) { LogErr(nameof(UpdateAsync), ex); return null; }
    }

    public static async Task<List<BoardingPayerAuthenticationSubscription>> GetAllAsync()
    {
        try
        {
            using var db = new B2cNorthwindContext();
            return await db.BoardingPayerAuthenticationSubscriptions.AsNoTracking()
                .Include(x => x.CardTypeConfigs).ThenInclude(ct => ct.Currencies)
                .OrderByDescending(x => x.CreatedAt).ToListAsync();
        }
        catch (Exception ex) { LogErr(nameof(GetAllAsync), ex); return new(); }
    }

    public static async Task<BoardingPayerAuthenticationSubscription?> GetByIdAsync(int id)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            return await db.BoardingPayerAuthenticationSubscriptions.AsNoTracking()
                .Include(x => x.CardTypeConfigs).ThenInclude(ct => ct.Currencies)
                .FirstOrDefaultAsync(x => x.BoardingPayerAuthenticationSubscriptionId == id);
        }
        catch (Exception ex) { LogErr(nameof(GetByIdAsync), ex); return null; }
    }

    public static async Task<bool> DeleteAsync(int id)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            await db.BoardingTransactingMerchantProductSubscriptions
                .Where(j => j.ProductType == BoardingProductTypes.PayerAuthentication && j.ProductSubscriptionId == id)
                .ExecuteDeleteAsync();
            var configIds = await db.BoardingPayerAuthenticationCardTypeConfigs
                .Where(c => c.BoardingPayerAuthenticationSubscriptionId == id)
                .Select(c => c.BoardingPayerAuthenticationCardTypeConfigId)
                .ToListAsync();
            if (configIds.Count > 0)
            {
                await db.BoardingPayerAuthenticationCurrencies
                    .Where(cur => configIds.Contains(cur.BoardingPayerAuthenticationCardTypeConfigId))
                    .ExecuteDeleteAsync();
                await db.BoardingPayerAuthenticationCardTypeConfigs
                    .Where(c => c.BoardingPayerAuthenticationSubscriptionId == id)
                    .ExecuteDeleteAsync();
            }
            return await db.BoardingPayerAuthenticationSubscriptions
                .Where(x => x.BoardingPayerAuthenticationSubscriptionId == id)
                .ExecuteDeleteAsync() > 0;
        }
        catch (Exception ex) { LogErr(nameof(DeleteAsync), ex); return false; }
    }

    public static BoardingPayerAuthenticationSubscriptionDto ToDto(BoardingPayerAuthenticationSubscription e) => new()
    {
        BoardingPayerAuthenticationSubscriptionId = e.BoardingPayerAuthenticationSubscriptionId,
        Enabled            = e.Enabled,
        EnablementStatus   = e.EnablementStatus,
        SelfServiceability = e.SelfServiceability,
        Distributability   = e.Distributability,
        TemplateId         = e.TemplateId,
        CardTypeConfigs    = e.CardTypeConfigs.Select(ct => new BoardingPayerAuthenticationCardTypeDto
        {
            BoardingPayerAuthenticationCardTypeConfigId = ct.BoardingPayerAuthenticationCardTypeConfigId,
            CardTypeName = ct.CardTypeName,
            Currencies   = ct.Currencies.Select(cur => new BoardingPayerAuthenticationCurrencyDto
            {
                BoardingPayerAuthenticationCurrencyId = cur.BoardingPayerAuthenticationCurrencyId,
                CurrencyCodes       = cur.CurrencyCodes,
                AcquirerId          = cur.AcquirerId,
                ProcessorMerchantId = cur.ProcessorMerchantId
            }).ToList()
        }).ToList()
    };

    private static void LogErr(string method, Exception ex) =>
        Console.WriteLine($"[DBBoardingPayerAuth] {method} error: {ex.Message}{(ex.InnerException is null ? "" : " | inner: " + ex.InnerException.Message)}");
}

// ── Junction (polymorphic) ───────────────────────────────────────────────
public static class DBBoardingMerchantProductSubscriptionServices
{
    public static async Task<BoardingTransactingMerchantProductSubscription?> LinkAsync(BoardingTransactingMerchantProductSubscriptionDto dto)
    {
        if (!BoardingProductTypes.IsValid(dto.ProductType)) return null;
        try
        {
            using var db = new B2cNorthwindContext();

            var existing = await db.BoardingTransactingMerchantProductSubscriptions.FirstOrDefaultAsync(j =>
                j.BoardingTransactingMerchantId == dto.BoardingTransactingMerchantId &&
                j.ProductType                   == dto.ProductType &&
                j.ProductSubscriptionId         == dto.ProductSubscriptionId);
            if (existing is not null) return existing;

            var row = new BoardingTransactingMerchantProductSubscription
            {
                BoardingTransactingMerchantId = dto.BoardingTransactingMerchantId,
                ProductType                   = dto.ProductType!,
                ProductSubscriptionId         = dto.ProductSubscriptionId,
                AssignedAt                    = DateTime.UtcNow
            };
            db.BoardingTransactingMerchantProductSubscriptions.Add(row);
            await db.SaveChangesAsync();
            return row;
        }
        catch (Exception ex) { LogErr(nameof(LinkAsync), ex); return null; }
    }

    public static async Task<bool> UpdateProductLinkIncludeInBoardingAsync(int junctionId, bool include)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            return await db.BoardingTransactingMerchantProductSubscriptions
                .Where(j => j.BoardingTransactingMerchantProductSubscriptionId == junctionId)
                .ExecuteUpdateAsync(s => s.SetProperty(j => j.IncludeInBoarding, include)) > 0;
        }
        catch (Exception ex) { LogErr(nameof(UpdateProductLinkIncludeInBoardingAsync), ex); return false; }
    }

    public static async Task<bool> UnlinkAsync(int junctionId)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            return await db.BoardingTransactingMerchantProductSubscriptions
                .Where(j => j.BoardingTransactingMerchantProductSubscriptionId == junctionId)
                .ExecuteDeleteAsync() > 0;
        }
        catch (Exception ex) { LogErr(nameof(UnlinkAsync), ex); return false; }
    }

    public static async Task<List<BoardingTransactingMerchantProductSubscription>> GetByMerchantAsync(int merchantId)
    {
        try
        {
            using var db = new B2cNorthwindContext();
            return await db.BoardingTransactingMerchantProductSubscriptions.AsNoTracking()
                .Where(j => j.BoardingTransactingMerchantId == merchantId)
                .OrderBy(j => j.ProductType).ThenBy(j => j.ProductSubscriptionId)
                .ToListAsync();
        }
        catch (Exception ex) { LogErr(nameof(GetByMerchantAsync), ex); return new(); }
    }

    private static void LogErr(string method, Exception ex) =>
        Console.WriteLine($"[DBBoardingMps] {method} error: {ex.Message}{(ex.InnerException is null ? "" : " | inner: " + ex.InnerException.Message)}");

    public static BoardingTransactingMerchantProductSubscriptionDto ToDto(BoardingTransactingMerchantProductSubscription e) => new()
    {
        BoardingTransactingMerchantProductSubscriptionId = e.BoardingTransactingMerchantProductSubscriptionId,
        BoardingTransactingMerchantId                    = e.BoardingTransactingMerchantId,
        ProductType                                      = e.ProductType,
        ProductSubscriptionId                            = e.ProductSubscriptionId,
        IncludeInBoarding                                = e.IncludeInBoarding
    };
}
