using CybsClass.Cybersource.Models.BaseData;
using CybsClass.Cybersource.Models.DTOs;
using System.Text.Json.Nodes;

namespace CybsClass.Cybersource.Models.OutboundTransObjects
{
    public class SessionTransJson
    {
        public string? TransactionId { get; set; } = string.Empty;
        public string? TransactionType { get; set; }
        public FollowOnTransactions? OriginalTransactionType { get; set; }
        public FollowOnTransactions? CurrentTransactionType { get; set; }
        public JsonNode? TransactionJson { get; set; }
        public B2cCustomerDto? Customer { get; set; }
        public CloudPaymentResponseJsonDto? CloudPaymentResponseJson { get; set; }
        public string? TransactionState { get; set; }
        public string? TransactionStatus { get; set; }
        public string? TransactionOrderId { get; set; }
        public string? OriginalTransactionId { get; set; }
        public string? TransactionAmount { get; set; }
        public string? ReconciliationId { get; set; }
        public string? TransientToken { get; set; }
        public TransactionStateValues? JsonTransactionStateValues { get; set; }
        public FollowOnTransactions? FollowOnTransaction { get; set; }
        public string error { get; set; } = string.Empty;

    }
}
