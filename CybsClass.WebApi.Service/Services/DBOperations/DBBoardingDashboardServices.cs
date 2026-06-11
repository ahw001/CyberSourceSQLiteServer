using Microsoft.EntityFrameworkCore;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.WebApi.Service.Services.DBOperations;

public static class DBBoardingDashboardServices
{
    public static async Task<BoardingDashboardDto?> GetDashboardAsync()
    {
        try
        {
            using var db = new B2cNorthwindContext();

            var orgs = await db.BoardingOrganizations.AsNoTracking()
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var merchants = await db.BoardingTransactingMerchants.AsNoTracking()
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            var cardJunctions = await db.BoardingTransactingMerchantSubscriptions.AsNoTracking()
                .Include(j => j.BoardingCardProductSubscription)
                    .ThenInclude(s => s.CardProcessingConfigs)
                        .ThenInclude(c => c.ProcessorConfigs)
                .ToListAsync();

            var productLinks = await db.BoardingTransactingMerchantProductSubscriptions.AsNoTracking()
                .OrderBy(j => j.BoardingTransactingMerchantId).ThenBy(j => j.ProductType)
                .ToListAsync();

            // Collect linked IDs per product type for targeted queries
            var linkedDigitalIds   = productLinks.Where(j => j.ProductType == BoardingProductTypes.DigitalPayments)   .Select(j => j.ProductSubscriptionId).ToHashSet();
            var linkedInvoicingIds = productLinks.Where(j => j.ProductType == BoardingProductTypes.CustomerInvoicing) .Select(j => j.ProductSubscriptionId).ToHashSet();
            var linkedPayByLinkIds = productLinks.Where(j => j.ProductType == BoardingProductTypes.PayByLink)         .Select(j => j.ProductSubscriptionId).ToHashSet();
            var linkedTokenIds     = productLinks.Where(j => j.ProductType == BoardingProductTypes.TokenManagement)   .Select(j => j.ProductSubscriptionId).ToHashSet();
            var linkedUcIds        = productLinks.Where(j => j.ProductType == BoardingProductTypes.UnifiedCheckout)   .Select(j => j.ProductSubscriptionId).ToHashSet();
            var linkedVasIds       = productLinks.Where(j => j.ProductType == BoardingProductTypes.ValueAddedServices).Select(j => j.ProductSubscriptionId).ToHashSet();
            var linkedVtIds        = productLinks.Where(j => j.ProductType == BoardingProductTypes.VirtualTerminal)   .Select(j => j.ProductSubscriptionId).ToHashSet();
            var linkedPaIds        = productLinks.Where(j => j.ProductType == BoardingProductTypes.PayerAuthentication).Select(j => j.ProductSubscriptionId).ToHashSet();

            Dictionary<int, (bool? Enabled, string? EnablementStatus)> digitalDict = linkedDigitalIds.Count > 0
                ? (await db.BoardingDigitalPaymentsSubscriptions.AsNoTracking()
                    .Where(x => linkedDigitalIds.Contains(x.BoardingDigitalPaymentsSubscriptionId)).ToListAsync())
                    .ToDictionary(x => x.BoardingDigitalPaymentsSubscriptionId,
                                  x => ((bool?)x.Enabled, (string?)x.EnablementStatus))
                : new();

            Dictionary<int, (bool? Enabled, string? EnablementStatus)> invoicingDict = linkedInvoicingIds.Count > 0
                ? (await db.BoardingInvoicingSubscriptions.AsNoTracking()
                    .Where(x => linkedInvoicingIds.Contains(x.BoardingInvoicingSubscriptionId)).ToListAsync())
                    .ToDictionary(x => x.BoardingInvoicingSubscriptionId,
                                  x => ((bool?)x.Enabled, (string?)x.EnablementStatus))
                : new();

            Dictionary<int, (bool? Enabled, string? EnablementStatus)> payByLinkDict = linkedPayByLinkIds.Count > 0
                ? (await db.BoardingPayByLinkSubscriptions.AsNoTracking()
                    .Where(x => linkedPayByLinkIds.Contains(x.BoardingPayByLinkSubscriptionId)).ToListAsync())
                    .ToDictionary(x => x.BoardingPayByLinkSubscriptionId,
                                  x => ((bool?)x.Enabled, (string?)x.EnablementStatus))
                : new();

            Dictionary<int, (bool? Enabled, string? EnablementStatus)> tokenDict = linkedTokenIds.Count > 0
                ? (await db.BoardingTokenManagementSubscriptions.AsNoTracking()
                    .Where(x => linkedTokenIds.Contains(x.BoardingTokenManagementSubscriptionId)).ToListAsync())
                    .ToDictionary(x => x.BoardingTokenManagementSubscriptionId,
                                  x => ((bool?)x.Enabled, (string?)x.EnablementStatus))
                : new();

            Dictionary<int, (bool? Enabled, string? EnablementStatus)> ucDict = linkedUcIds.Count > 0
                ? (await db.BoardingUnifiedCheckoutSubscriptions.AsNoTracking()
                    .Where(x => linkedUcIds.Contains(x.BoardingUnifiedCheckoutSubscriptionId)).ToListAsync())
                    .ToDictionary(x => x.BoardingUnifiedCheckoutSubscriptionId,
                                  x => ((bool?)x.Enabled, (string?)x.EnablementStatus))
                : new();

            Dictionary<int, (bool? Enabled, string? EnablementStatus)> vasDict = linkedVasIds.Count > 0
                ? (await db.BoardingValueAddedServicesSubscriptions.AsNoTracking()
                    .Where(x => linkedVasIds.Contains(x.BoardingValueAddedServicesSubscriptionId)).ToListAsync())
                    .ToDictionary(x => x.BoardingValueAddedServicesSubscriptionId,
                                  x => (x.TransactionSearchEnabled ?? x.ReportingEnabled,
                                        (string?)(x.TransactionSearchEnablementStatus ?? x.ReportingEnablementStatus)))
                : new();

            Dictionary<int, (bool? Enabled, string? EnablementStatus)> vtDict = linkedVtIds.Count > 0
                ? (await db.BoardingVirtualTerminalSubscriptions.AsNoTracking()
                    .Where(x => linkedVtIds.Contains(x.BoardingVirtualTerminalSubscriptionId)).ToListAsync())
                    .ToDictionary(x => x.BoardingVirtualTerminalSubscriptionId,
                                  x => ((bool?)x.Enabled, (string?)x.EnablementStatus))
                : new();

            Dictionary<int, (bool? Enabled, string? EnablementStatus)> paDict = linkedPaIds.Count > 0
                ? (await db.BoardingPayerAuthenticationSubscriptions.AsNoTracking()
                    .Where(x => linkedPaIds.Contains(x.BoardingPayerAuthenticationSubscriptionId)).ToListAsync())
                    .ToDictionary(x => x.BoardingPayerAuthenticationSubscriptionId,
                                  x => ((bool?)x.Enabled, (string?)x.EnablementStatus))
                : new();

            // Group for in-memory hierarchy assembly
            var cardByMerchant    = cardJunctions.GroupBy(j => j.BoardingTransactingMerchantId).ToDictionary(g => g.Key, g => g.ToList());
            var linksByMerchant   = productLinks.GroupBy(j => j.BoardingTransactingMerchantId).ToDictionary(g => g.Key, g => g.ToList());
            var merchantsByOrg    = merchants.GroupBy(m => m.BoardingOrganizationId).ToDictionary(g => g.Key, g => g.ToList());

            (bool? Enabled, string? EnablementStatus) ResolveSupplemental(string productType, int subscriptionId)
            {
                switch (productType)
                {
                    case BoardingProductTypes.DigitalPayments:
                        return digitalDict.TryGetValue(subscriptionId, out var d) ? d : (null, null);
                    case BoardingProductTypes.CustomerInvoicing:
                        return invoicingDict.TryGetValue(subscriptionId, out var inv) ? inv : (null, null);
                    case BoardingProductTypes.PayByLink:
                        return payByLinkDict.TryGetValue(subscriptionId, out var pbl) ? pbl : (null, null);
                    case BoardingProductTypes.TokenManagement:
                        return tokenDict.TryGetValue(subscriptionId, out var tm) ? tm : (null, null);
                    case BoardingProductTypes.UnifiedCheckout:
                        return ucDict.TryGetValue(subscriptionId, out var uc) ? uc : (null, null);
                    case BoardingProductTypes.ValueAddedServices:
                        return vasDict.TryGetValue(subscriptionId, out var vas) ? vas : (null, null);
                    case BoardingProductTypes.VirtualTerminal:
                        return vtDict.TryGetValue(subscriptionId, out var vt) ? vt : (null, null);
                    case BoardingProductTypes.PayerAuthentication:
                        return paDict.TryGetValue(subscriptionId, out var pa) ? pa : (null, null);
                    default:
                        return (null, null);
                }
            }

            var dashboard = new BoardingDashboardDto
            {
                Organizations = orgs.Select(org =>
                {
                    var orgMerchants = merchantsByOrg.TryGetValue(org.BoardingOrganizationId, out var ms) ? ms : new();
                    return new BoardingDashboardOrgItem
                    {
                        BoardingOrganizationId = org.BoardingOrganizationId,
                        OrganizationId         = org.OrganizationId,
                        BusinessName           = org.BusinessName,
                        Status                 = org.Status,
                        Type                   = org.Type,
                        BoardingFlow           = org.BoardingFlow,
                        TransactingMerchants   = orgMerchants.Select(m =>
                        {
                            var mid   = m.BoardingTransactingMerchantId;
                            var cards = cardByMerchant.TryGetValue(mid, out var cj) ? cj : new();
                            var links = linksByMerchant.TryGetValue(mid, out var lj) ? lj : new();

                            return new BoardingDashboardMerchantItem
                            {
                                BoardingTransactingMerchantId = mid,
                                TransactingOrganizationId     = m.TransactingOrganizationId,
                                BusinessName                  = m.BusinessName,
                                Status                        = m.Status,
                                BoardingProcessor             = m.BoardingProcessor,
                                CybersourceBoardingStatus     = m.CybersourceBoardingStatus,
                                CardProductSubscriptions = cards
                                    .Where(j => j.BoardingCardProductSubscription is not null)
                                    .Select(j =>
                                    {
                                        var sub         = j.BoardingCardProductSubscription!;
                                        var firstConfig = sub.CardProcessingConfigs.FirstOrDefault();
                                        return new BoardingDashboardCardProductItem
                                        {
                                            BoardingProductSubscriptionId             = sub.BoardingProductSubscriptionId,
                                            BoardingTransactingMerchantSubscriptionId = j.BoardingTransactingMerchantSubscriptionId,
                                            ProductName                               = sub.ProductName,
                                            EnablementStatus                          = sub.EnablementStatus,
                                            CybersourceBoardingStatus                 = j.CybersourceBoardingStatus,
                                            CardPresentEnabled                        = sub.CardPresentEnabled,
                                            CardNotPresentEnabled                     = sub.CardNotPresentEnabled,
                                            IncludeInBoarding                         = j.IncludeInBoarding,
                                            Processors = firstConfig is null
                                                ? new()
                                                : firstConfig.ProcessorConfigs.Select(p => new BoardingDashboardProcessorItem
                                                {
                                                    BoardingProcessorConfigId = p.BoardingProcessorConfigId,
                                                    ProcessorName             = p.ProcessorName
                                                }).ToList()
                                        };
                                    }).ToList(),
                                SupplementalProducts = links.Select(link =>
                                {
                                    var (enabled, status) = ResolveSupplemental(link.ProductType, link.ProductSubscriptionId);
                                    return new BoardingDashboardSupplementalProductItem
                                    {
                                        BoardingTransactingMerchantProductSubscriptionId = link.BoardingTransactingMerchantProductSubscriptionId,
                                        ProductType             = link.ProductType,
                                        ProductSubscriptionId   = link.ProductSubscriptionId,
                                        Enabled                 = enabled,
                                        EnablementStatus        = status,
                                        IncludeInBoarding       = link.IncludeInBoarding,
                                        CybersourceBoardingStatus = link.CybersourceBoardingStatus
                                    };
                                }).ToList()
                            };
                        }).ToList()
                    };
                }).ToList()
            };

            return dashboard;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DBBoardingDashboard] GetDashboard error: {ex.Message}{(ex.InnerException is null ? "" : " | inner: " + ex.InnerException.Message)}");
            return null;
        }
    }
}
