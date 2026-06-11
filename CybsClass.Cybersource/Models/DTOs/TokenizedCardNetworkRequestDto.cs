using CybsClass.Cybersource.Models.BaseData;

namespace CybsClass.Cybersource.Models.DTOs;

public class TokenizedCardNetworkRequestDto
{
    public string Source { get; set; } = "ONFILE";
    public string? CardNumber { get; set; }
    public string? ExpMonth { get; set; }
    public string? ExpYear { get; set; }
    public string? SecurityCode { get; set; }
    public ErrorObject? Error { get; set; }
}
