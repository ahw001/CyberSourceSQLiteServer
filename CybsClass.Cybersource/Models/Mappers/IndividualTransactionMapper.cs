using CybsClass.Cybersource.Models.DTOs;
using CybsClass.EntityModels;

namespace CybsClass.Cybersource.Models.Mappers
{
    public static class IndividualTransactionMapper
    {
        public static IndividualTransactionDto? Map(IndividualTransaction individualTransaction)
        {
            return individualTransaction != null ? new IndividualTransactionDto
            {
                TransactionId = individualTransaction.TransactionId,
                RequestId = individualTransaction.RequestId,
                TransactionType = individualTransaction.TransactionType,
                ReferenceTransactionId = individualTransaction.ReferenceTransactionId,
                ResponseTransactionJson = individualTransaction.ResponseTransactionJson
            } : null;
        }

        public static IndividualTransaction? Map(IndividualTransactionDto individualTransactionDto)
        {
            return individualTransactionDto != null ? new IndividualTransaction
            {
                TransactionId = individualTransactionDto.TransactionId,
                RequestId = individualTransactionDto.RequestId,
                TransactionType = individualTransactionDto.TransactionType,
                ReferenceTransactionId = individualTransactionDto.ReferenceTransactionId,
                ResponseTransactionJson = individualTransactionDto.ResponseTransactionJson
            } : null;
        }

        public static List<IndividualTransaction> Map(List<IndividualTransactionDto> individualTransactionDtos)
        {
            return individualTransactionDtos.Select(Map).ToList()!;
        }

        public static List<IndividualTransactionDto> Map(List<IndividualTransaction> individualTransactions)
        {
            return individualTransactions.Select(Map).ToList()!;
        }
    }
}
