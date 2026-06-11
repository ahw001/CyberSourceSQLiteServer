using Microsoft.EntityFrameworkCore;
using CybsClass.EntityModels;
using CybsClass.Cybersource.Models.DTOs;
using CybsClass.Cybersource.Models.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CybsClass.WebApi.Service.Services.DBOperations
{
    public class DBFollowOnTransResponseServices
    {
        public static async Task<int> GetFollowOnTransResponseCountAsync()
        {
            using B2cNorthwindContext db = new();
            return await db.FollowOnTransResponses.CountAsync();
        }
        public static async Task<List<FollowOnTransResponseDto>> GetFollowOnTransResponses()
        {
            try
            {
                Console.WriteLine("Geting full list of Follow On Transactions ...");
                using B2cNorthwindContext db = new();
                var followOnTransResponses = await db.FollowOnTransResponses.ToListAsync();
                List<FollowOnTransResponseDto> FollowOnTransResponseDtos = FollowOnTransResponseMapper.Map(followOnTransResponses)!;
                return FollowOnTransResponseDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting full list of Follow On Transactions: {ex.ToString()}");
                var followOnTransResponses = new List<FollowOnTransResponseDto>();
                FollowOnTransResponseDto followOnTransResponseDto = new FollowOnTransResponseDto();
                followOnTransResponseDto.Error = ex.ToString();
                followOnTransResponses.Add(followOnTransResponseDto);
                return followOnTransResponses;
            }

        }

        public static async Task<FollowOnTransResponseDto?> GetFollowOnTransResponseByUsingId([FromRoute] int id)
        {
            try
            {
                Console.WriteLine($"Geting Follow On Transactions for: {id}");
                using B2cNorthwindContext db = new();
                Task<FollowOnTransResponse?> task = db.FollowOnTransResponses.AsNoTracking()
                            .FirstOrDefaultAsync(model => model.TransResponseId == id);

                var followOnTransResponse = await task;
                if (followOnTransResponse == null)
                {
                    return null;
                }
                return FollowOnTransResponseMapper.Map(followOnTransResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Follow On Transactions for ID: {ex.ToString()}");
                var followOnTransResponseDto = new FollowOnTransResponseDto
                {
                    Error = ex.ToString()
                };
                return followOnTransResponseDto;
            }
        }
        public static async Task<int> CreateFollowOnTransResponse(FollowOnTransResponseDto followOnTransResponseDto)
        {
            try
            {
                FollowOnTransResponse followOnTransResponse = FollowOnTransResponseMapper.Map(followOnTransResponseDto)!;
                using B2cNorthwindContext db = new();
                db.FollowOnTransResponses.Add(followOnTransResponse);
                var affected = await db.SaveChangesAsync();
                return affected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Creating Follow On Transaction - {ex.ToString()}");
                return 0;
            }

        }
    }
}
