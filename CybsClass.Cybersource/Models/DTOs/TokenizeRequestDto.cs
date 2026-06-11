using CybsClass.Cybersource.Models.BaseData;

namespace CybsClass.Cybersource.Models.DTOs;

public class TokenizeRequestDto
{
    public string? AccountNumber { get; set; }
    public string? ExpMonth { get; set; }
    public string? ExpYear { get; set; }
    public string? CardType { get; set; }
    public int B2cCustomerId { get; set; }
    public ErrorObject? Error { get; set; }
}
