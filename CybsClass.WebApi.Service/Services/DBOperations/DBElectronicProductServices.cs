using Microsoft.EntityFrameworkCore;
using CybsClass.EntityModels;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public class DBElectronicProductServices
    {
        public static async Task<List<ElectronicProduct>> GetAllElectronicProducts()
        {
            try
            {
                Console.WriteLine("[DBElectronicProductServices] Fetching all electronic products.");
                using B2cNorthwindContext db = new();
                return await db.ElectronicProducts.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBElectronicProductServices] Error fetching all electronic products: {ex}");
                return [];
            }
        }

        public static async Task<ElectronicProduct?> GetElectronicProductById(int id)
        {
            try
            {
                Console.WriteLine($"[DBElectronicProductServices] Fetching electronic product with ID {id}.");
                using B2cNorthwindContext db = new();
                return await db.ElectronicProducts.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.ElectronicProductId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBElectronicProductServices] Error fetching electronic product with ID {id}: {ex}");
                return null;
            }
        }

        public static async Task<int> UpdateElectronicProduct(int id, ElectronicProduct electronicProduct)
        {
            try
            {
                Console.WriteLine($"[DBElectronicProductServices] Updating electronic product with ID {id}.");
                using B2cNorthwindContext db = new();
                return await db.ElectronicProducts
                    .Where(model => model.ElectronicProductId == id)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(m => m.ProductName, electronicProduct.ProductName)
                        .SetProperty(m => m.ProductLabel, electronicProduct.ProductLabel)
                        .SetProperty(m => m.UnitPrice, electronicProduct.UnitPrice)
                        .SetProperty(m => m.ProductSku, electronicProduct.ProductSku)
                        .SetProperty(m => m.Picture, electronicProduct.Picture)
                        .SetProperty(m => m.Brand, electronicProduct.Brand));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBElectronicProductServices] Error updating electronic product with ID {id}: {ex}");
                return 0;
            }
        }

        public static async Task<ElectronicProduct?> CreateElectronicProduct(ElectronicProduct electronicProduct)
        {
            try
            {
                Console.WriteLine($"[DBElectronicProductServices] Inserting new electronic product with SKU {electronicProduct.ProductSku}.");
                using B2cNorthwindContext db = new();
                db.ElectronicProducts.Add(electronicProduct);
                await db.SaveChangesAsync();
                Console.WriteLine($"[DBElectronicProductServices] Electronic product created with ID {electronicProduct.ElectronicProductId}.");
                return electronicProduct;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBElectronicProductServices] Error creating electronic product: {ex}");
                return null;
            }
        }

        public static async Task<int> DeleteElectronicProduct(int id)
        {
            try
            {
                Console.WriteLine($"[DBElectronicProductServices] Deleting electronic product with ID {id}.");
                using B2cNorthwindContext db = new();
                return await db.ElectronicProducts
                    .Where(model => model.ElectronicProductId == id)
                    .ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBElectronicProductServices] Error deleting electronic product with ID {id}: {ex}");
                return 0;
            }
        }
    }
}
