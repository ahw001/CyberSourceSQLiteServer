using Microsoft.EntityFrameworkCore;
using CybsClass.EntityModels;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public class DBAuthTransResponseServices
    {
        public static async Task<int> GetAuthTransResponseCountAsync()
        {
            using B2cNorthwindContext db = new();
            return await db.AuthTransResponses.CountAsync();
        }
        public static async Task<List<AuthTransResponseDto>> GetAuthTransResponses()
        {
            try
            {
                Console.WriteLine("Geting full list of Payment Cards ...");
                using B2cNorthwindContext db = new();
                var authTransResponses = await db.AuthTransResponses.ToListAsync();
                List<AuthTransResponseDto> authTransResponseDtos = AuthTransResponseMapper.Map(authTransResponses)!;
                return authTransResponseDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting full list of Payment Cards: {ex.ToString()}");
                var authTransResponses = new List<AuthTransResponseDto>();
                AuthTransResponseDto authTransResponseDto = new AuthTransResponseDto();
                authTransResponseDto.Error = ex.ToString();
                authTransResponses.Add(authTransResponseDto);
                return authTransResponses;
            }

        }

        public static async Task<AuthTransResponseDto?> GetAuthTransResponseByUsingId([FromRoute] int id)
        {
            try
            {
                Console.WriteLine($"Geting Auth Trans Response for: {id}");
                using B2cNorthwindContext db = new();
                Task<AuthTransResponse?> task = db.AuthTransResponses.AsNoTracking()
                            .FirstOrDefaultAsync(model => model.AuthTransResponsesId == id);

                var authTransResponse = await task;
                if (authTransResponse == null)
                {
                    return null;
                }
                return AuthTransResponseMapper.Map(authTransResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Auth Trans Response for ID: {ex.ToString()}");
                var authTransResponseDto = new AuthTransResponseDto();
                authTransResponseDto.Error = ex.ToString();
                return authTransResponseDto;
            }
        }
        public static async Task<int> CreateAuthTransResponse(AuthTransResponseDto authTransResponseDto)
        {
            try
            {
                AuthTransResponse authTransResponse = AuthTransResponseMapper.Map(authTransResponseDto)!;
                using B2cNorthwindContext db = new();
                db.AuthTransResponses.Add(authTransResponse);
                var affected = await db.SaveChangesAsync();
                return affected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Creating Auth Trans Reponse - {ex.ToString()}");
                return 0;
            }

        }
    }
}
