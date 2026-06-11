using Microsoft.EntityFrameworkCore.ChangeTracking;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;
using System.Text.Json;
using System.Text.Json.Nodes;
namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public static class PersistSessionState
    {
        private static Dictionary<string, string> dbResults = new Dictionary<string, string>();

        public static async Task<Dictionary<string, string>> SessionStateDBOps(B2cCustomerDto b2cCustomerDto)
        {
            try 
            {
                dbResults = new();
                var guid = Guid.NewGuid();
                var json = JsonSerializer.Serialize(b2cCustomerDto);

                Console.WriteLine("Inserting customer session state data ...");

                using B2cNorthwindContext db = new();

                var sessionStateStore = new SessionTransactionsStore();

                sessionStateStore.SerializedData = json;
                sessionStateStore.Id = guid;

                EntityEntry<SessionTransactionsStore> entity = db.SessionTransactionsStores.Add(sessionStateStore);

                Console.WriteLine($"Session State: {entity.State}, Guid: {sessionStateStore.Id}");

                int affected0 = await db.SaveChangesAsync();
                if (affected0 > 0) 
                {
                    dbResults = new();
                    dbResults.Add("GUID", sessionStateStore.Id.ToString());
                    return dbResults;
                }
            } 
            catch (Exception ex)
            {
                dbResults = new();
                dbResults.Add("error", ex.Message);
                Console.WriteLine($"Exception: {ex.Message}");
                return dbResults;
            }
            return dbResults;
        }

        public static async Task<SessionStateDto?> GetSessionStateByIdAsync(Guid id)
        {
            try
            {
                using B2cNorthwindContext db = new();
                var record = await db.SessionTransactionsStores.FindAsync(id);
                if (record == null) return null;
                return new SessionStateDto
                {
                    Id = record.Id,
                    SerializedData = record.SerializedData,
                    CreatedAt = record.CreatedAt
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving session state: {ex.Message}");
                return null;
            }
        }
    }
}
