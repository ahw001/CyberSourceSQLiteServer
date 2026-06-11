using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.Cybersource.Models.Mappers
{
    public static class FollowOnTransResponseMapper
    {
        public static FollowOnTransResponseDto? Map(FollowOnTransResponse followOnTransResponse)
        {
            return followOnTransResponse != null ? new FollowOnTransResponseDto
            {
                TransResponseId = followOnTransResponse.TransResponseId,
                FollowOnTransResponseId = followOnTransResponse.FollowOnTransResponseId,
                OriginalTransactionId = followOnTransResponse.OriginalTransactionId,
                OrderId = followOnTransResponse.OrderId,
                TransactionType = followOnTransResponse.TransactionType,
                ResponseTransactionJson = followOnTransResponse.ResponseTransactionJson
            } : null;
        }

        public static FollowOnTransResponse? Map(FollowOnTransResponseDto followOnTransResponseDto)
        {
            return followOnTransResponseDto != null ? new FollowOnTransResponse
            {
                TransResponseId = followOnTransResponseDto.TransResponseId,
                FollowOnTransResponseId = followOnTransResponseDto.FollowOnTransResponseId,
                OriginalTransactionId = followOnTransResponseDto.OriginalTransactionId,
                OrderId = followOnTransResponseDto.OrderId,
                TransactionType = followOnTransResponseDto.TransactionType,
                ResponseTransactionJson = followOnTransResponseDto.ResponseTransactionJson
            } : null;
        }

        public static List<FollowOnTransResponse> Map(List<FollowOnTransResponseDto> followOnTransResponseDtos)
        {
            return followOnTransResponseDtos.Select(Map).ToList()!;
        }

        public static List<FollowOnTransResponseDto> Map(List<FollowOnTransResponse> followOnTransResponses)
        {
            return followOnTransResponses.Select(Map).ToList()!;
        }
    }
}
