using CybsClass.Cybersource.Models.BaseData;
using System.Text.Json.Serialization;

namespace CybsClass.Cybersource.Models.DTOs;

public partial class FollowOnTransResponseDto
{
    public int TransResponseId { get; set; }

    public string FollowOnTransResponseId { get; set; } = null!;

    public string OriginalTransactionId { get; set; } = null!;

    public int OrderId { get; set; }

    public string TransactionType { get; set; } = null!;

    public string? ResponseTransactionJson { get; set; }
    public string? Error { get; set; }

    [JsonPropertyName("_links")]
    public Links? Links { get; set; }

    [JsonPropertyName("clientReferenceInformation")]
    public ClientReferenceInformation? ClientReferenceInformation { get; set; }

    [JsonPropertyName("consumerAuthenticationInformation")]
    public ConsumerAuthenticationInformation? ConsumerAuthenticationInformation { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("issuerInformation")]
    public IssuerInformation? IssuerInformation { get; set; }

    [JsonPropertyName("orderInformation")]
    public OrderInformation? OrderInformation { get; set; }

    [JsonPropertyName("paymentAccountInformation")]
    public PaymentAccountInformation? PaymentAccountInformation { get; set; }

    [JsonPropertyName("paymentInformation")]
    public PaymentInformation? PaymentInformation { get; set; }

    [JsonPropertyName("pointOfSaleInformation")]
    public PointOfSaleInformation? PointOfSaleInformation { get; set; }

    [JsonPropertyName("processorInformation")]
    public ProcessorInformation? ProcessorInformation { get; set; }

    [JsonPropertyName("reconciliationId")]
    public string? ReconciliationId { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("submitTimeUtc")]
    public string? SubmitTimeUtc { get; set; }

    [JsonPropertyName("tokenInformation")]
    public TokenInformation? TokenInformation { get; set; }

    [JsonPropertyName("object")]
    public string? TokenObject { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("tokenReferenceId")]
    public string? TokenReferenceId { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("requestorId")]
    public string? RequestorId { get; set; }

    [JsonPropertyName("PaymentCardId")]
    public string? PaymentCardId { get; set; }

    [JsonPropertyName("errorInformation")]
    public ErrorInformation? ErrorInformation { get; set; }

    [JsonPropertyName("tokenizedCard")]
    public TokenizedCard? TokenizedCard { get; set; }

    [JsonPropertyName("card")]
    public CardDetails? Card { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata? Metadata { get; set; }

}