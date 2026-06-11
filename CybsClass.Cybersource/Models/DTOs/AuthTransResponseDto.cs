namespace CybsClass.Cybersource.Models.DTOs;

public partial class AuthTransResponseDto
{
    public int AuthTransResponsesId { get; set; }

    public string Id { get; set; } = null!;

    public int OrderId { get; set; }

    public string? ClientReferenceCode { get; set; }

    public string? ConsumerAuthenticationToken { get; set; }

    public string? IssuerResponseRaw { get; set; }

    public string? ReconciliationId { get; set; }

    public string? Status { get; set; }

    public DateTime? SubmitTimeUtc { get; set; }

    public string? Links { get; set; }

    public decimal? AuthorizedAmount { get; set; }

    public string? Currency { get; set; }

    public string? CardType { get; set; }

    public string? ConsumerAuthInfoToken { get; set; }

    public string? ClientRefInfoCode { get; set; }

    public string? PosinfoTerminalId { get; set; }

    public string? ProcInfoPayAcctReferenceNumber { get; set; }

    public string? ProcInfoMerchantNumber { get; set; }

    public string? ProcInfoApprovalCode { get; set; }

    public string? ProcInfoNetworkTransactionId { get; set; }

    public string? ProcInfoTransactionId { get; set; }

    public string? ProcInfoResponseCode { get; set; }

    public string? AvsCode { get; set; }

    public string? AvsCodeRaw { get; set; }

    public string? TokenInformationInstIdNew { get; set; }

    public string? TokenInformationInstId { get; set; }

    public string? InstrumentIdentifierState { get; set; }

    public string? InstrumentIdentifierId { get; set; }

    public string? PaymentInstrumentId { get; set; }

    public string? ResponseTransactionJson { get; set; }

    public string? Error { get; set; }

}
