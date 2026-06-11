using Microsoft.EntityFrameworkCore;
using CybsClass.EntityModels;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public class DBSampleInvoiceDetailServices
    {
        public static async Task<List<SampleInvoiceDetail>> GetAllSampleInvoiceDetails()
        {
            try
            {
                Console.WriteLine("[DBSampleInvoiceDetailServices] Fetching all sample invoice details.");
                using B2cNorthwindContext db = new();
                return await db.SampleInvoiceDetails.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBSampleInvoiceDetailServices] Error fetching all sample invoice details: {ex}");
                return [];
            }
        }

        public static async Task<SampleInvoiceDetail?> GetSampleInvoiceDetailById(int id)
        {
            try
            {
                Console.WriteLine($"[DBSampleInvoiceDetailServices] Fetching sample invoice detail with ID {id}.");
                using B2cNorthwindContext db = new();
                return await db.SampleInvoiceDetails.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.SampleInvoiceId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBSampleInvoiceDetailServices] Error fetching sample invoice detail with ID {id}: {ex}");
                return null;
            }
        }

        public static async Task<int> UpdateSampleInvoiceDetail(int id, SampleInvoiceDetail sampleInvoiceDetail)
        {
            try
            {
                Console.WriteLine($"[DBSampleInvoiceDetailServices] Updating sample invoice detail with ID {id}.");
                using B2cNorthwindContext db = new();
                return await db.SampleInvoiceDetails
                    .Where(model => model.SampleInvoiceId == id)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(m => m.SampleInvoiceId, sampleInvoiceDetail.SampleInvoiceId)
                        .SetProperty(m => m.CustomerInformationName, sampleInvoiceDetail.CustomerInformationName)
                        .SetProperty(m => m.CustomerInformationEmail, sampleInvoiceDetail.CustomerInformationEmail)
                        .SetProperty(m => m.CustomerInformationMerchantCustomerId, sampleInvoiceDetail.CustomerInformationMerchantCustomerId)
                        .SetProperty(m => m.CustomerInformationCompanyName, sampleInvoiceDetail.CustomerInformationCompanyName)
                        .SetProperty(m => m.InvoiceInformationInvoiceNumber, sampleInvoiceDetail.InvoiceInformationInvoiceNumber)
                        .SetProperty(m => m.InvoiceInformationDueDate, sampleInvoiceDetail.InvoiceInformationDueDate)
                        .SetProperty(m => m.InvoiceInformationSendImmediately, sampleInvoiceDetail.InvoiceInformationSendImmediately)
                        .SetProperty(m => m.InvoiceInformationAllowPartialPayments, sampleInvoiceDetail.InvoiceInformationAllowPartialPayments)
                        .SetProperty(m => m.InvoiceInformationDeliveryMode, sampleInvoiceDetail.InvoiceInformationDeliveryMode)
                        .SetProperty(m => m.OrderInformationAmountDetailsTotalAmount, sampleInvoiceDetail.OrderInformationAmountDetailsTotalAmount)
                        .SetProperty(m => m.OrderInformationAmountDetailsCurrency, sampleInvoiceDetail.OrderInformationAmountDetailsCurrency)
                        .SetProperty(m => m.OrderInformationAmountDetailsDiscountAmount, sampleInvoiceDetail.OrderInformationAmountDetailsDiscountAmount)
                        .SetProperty(m => m.OrderInformationAmountDetailsDiscountPercent, sampleInvoiceDetail.OrderInformationAmountDetailsDiscountPercent)
                        .SetProperty(m => m.OrderInformationAmountDetailsSubAmount, sampleInvoiceDetail.OrderInformationAmountDetailsSubAmount)
                        .SetProperty(m => m.OrderInformationAmountDetailsMinimumPartialAmount, sampleInvoiceDetail.OrderInformationAmountDetailsMinimumPartialAmount)
                        .SetProperty(m => m.OrderInformationAmountDetailsTaxDetailsType, sampleInvoiceDetail.OrderInformationAmountDetailsTaxDetailsType)
                        .SetProperty(m => m.OrderInformationAmountDetailsTaxDetailsAmount, sampleInvoiceDetail.OrderInformationAmountDetailsTaxDetailsAmount)
                        .SetProperty(m => m.OrderInformationAmountDetailsTaxDetailsRate, sampleInvoiceDetail.OrderInformationAmountDetailsTaxDetailsRate)
                        .SetProperty(m => m.OrderInformationAmountDetailsFreightAmount, sampleInvoiceDetail.OrderInformationAmountDetailsFreightAmount)
                        .SetProperty(m => m.OrderInformationAmountDetailsFreightTaxable, sampleInvoiceDetail.OrderInformationAmountDetailsFreightTaxable)
                        .SetProperty(m => m.OrderInformationLineItemsProductSku, sampleInvoiceDetail.OrderInformationLineItemsProductSku)
                        .SetProperty(m => m.OrderInformationLineItemsQuantity, sampleInvoiceDetail.OrderInformationLineItemsQuantity)
                        .SetProperty(m => m.OrderInformationLineItemsUnitPrice, sampleInvoiceDetail.OrderInformationLineItemsUnitPrice)
                        .SetProperty(m => m.OrderInformationLineItemsDiscountAmount, sampleInvoiceDetail.OrderInformationLineItemsDiscountAmount)
                        .SetProperty(m => m.OrderInformationLineItemsDiscountRate, sampleInvoiceDetail.OrderInformationLineItemsDiscountRate)
                        .SetProperty(m => m.OrderInformationLineItemsTaxAmount, sampleInvoiceDetail.OrderInformationLineItemsTaxAmount)
                        .SetProperty(m => m.OrderInformationLineItemsTaxRate, sampleInvoiceDetail.OrderInformationLineItemsTaxRate)
                        .SetProperty(m => m.OrderInformationLineItemsTotalAmount, sampleInvoiceDetail.OrderInformationLineItemsTotalAmount));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBSampleInvoiceDetailServices] Error updating sample invoice detail with ID {id}: {ex}");
                return 0;
            }
        }

        public static async Task<SampleInvoiceDetail?> CreateSampleInvoiceDetail(SampleInvoiceDetail sampleInvoiceDetail)
        {
            try
            {
                Console.WriteLine("[DBSampleInvoiceDetailServices] Inserting new sample invoice detail.");
                using B2cNorthwindContext db = new();
                db.SampleInvoiceDetails.Add(sampleInvoiceDetail);
                await db.SaveChangesAsync();
                Console.WriteLine($"[DBSampleInvoiceDetailServices] Sample invoice detail created with ID {sampleInvoiceDetail.SampleInvoiceId}.");
                return sampleInvoiceDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBSampleInvoiceDetailServices] Error creating sample invoice detail: {ex}");
                return null;
            }
        }

        public static async Task<int> DeleteSampleInvoiceDetail(int id)
        {
            try
            {
                Console.WriteLine($"[DBSampleInvoiceDetailServices] Deleting sample invoice detail with ID {id}.");
                using B2cNorthwindContext db = new();
                return await db.SampleInvoiceDetails
                    .Where(model => model.SampleInvoiceId == id)
                    .ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBSampleInvoiceDetailServices] Error deleting sample invoice detail with ID {id}: {ex}");
                return 0;
            }
        }
    }
}
