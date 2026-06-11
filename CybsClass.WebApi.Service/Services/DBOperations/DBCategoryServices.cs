using Microsoft.EntityFrameworkCore;
using CybsClass.EntityModels;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public class DBCategoryServices
    {
        public static async Task<List<Category>> GetAllCategories()
        {
            try
            {
                Console.WriteLine("[DBCategoryServices] Fetching all categories.");
                using B2cNorthwindContext db = new();
                return await db.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBCategoryServices] Error fetching all categories: {ex}");
                return [];
            }
        }

        public static async Task<Category?> GetCategoryById(int id)
        {
            try
            {
                Console.WriteLine($"[DBCategoryServices] Fetching category with ID {id}.");
                using B2cNorthwindContext db = new();
                return await db.Categories.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.CategoryId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBCategoryServices] Error fetching category with ID {id}: {ex}");
                return null;
            }
        }

        public static async Task<int> UpdateCategory(int id, Category category)
        {
            try
            {
                Console.WriteLine($"[DBCategoryServices] Updating category with ID {id}.");
                using B2cNorthwindContext db = new();
                return await db.Categories
                    .Where(model => model.CategoryId == id)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(m => m.CategoryId, category.CategoryId)
                        .SetProperty(m => m.CategoryName, category.CategoryName)
                        .SetProperty(m => m.Description, category.Description)
                        .SetProperty(m => m.Picture, category.Picture));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBCategoryServices] Error updating category with ID {id}: {ex}");
                return 0;
            }
        }

        public static async Task<Category?> CreateCategory(Category category)
        {
            try
            {
                Console.WriteLine("[DBCategoryServices] Inserting new category.");
                using B2cNorthwindContext db = new();
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                Console.WriteLine($"[DBCategoryServices] Category created with ID {category.CategoryId}.");
                return category;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBCategoryServices] Error creating category: {ex}");
                return null;
            }
        }

        public static async Task<int> DeleteCategory(int id)
        {
            try
            {
                Console.WriteLine($"[DBCategoryServices] Deleting category with ID {id}.");
                using B2cNorthwindContext db = new();
                return await db.Categories
                    .Where(model => model.CategoryId == id)
                    .ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DBCategoryServices] Error deleting category with ID {id}: {ex}");
                return 0;
            }
        }
    }
}
