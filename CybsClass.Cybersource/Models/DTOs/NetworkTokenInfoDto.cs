namespace CybsClass.Cybersource.Models.DTOs;

public partial class NetworkTokenInfoDto
{
    public int PaymentTokenId { get; set; }

    public int PaymentCardId { get; set; }

    public string TokenAccountNumber { get; set; } = null!;

    public string? TokenValue { get; set; }

    public string? TokenExpMonth { get; set; }

    public string? TokenExpYear { get; set; }

    public string? OriginalAccountSuffix { get; set; }

    public string? OriginalAccountExpMonth { get; set; }

    public string? OriginalAccountExpYear { get; set; }

    public string? OriginalAccountNumber { get; set; }

    public string? PaymentAccountReferenceNumber { get; set; }

    public string? TokenizedCardType { get; set; }

    public string? TokenState { get; set; }

    public string? TokenRequestorId { get; set; }

    public string? EnrollmentId { get; set; }

    public string? MitpreviousTransactionId { get; set; }

    public string? ResponseTransactionJson { get; set; }
    public string? Error { get; set; }

}
