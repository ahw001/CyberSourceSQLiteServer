using CybsClass.Cybersource.Models.BaseData;

namespace CybsClass.Cybersource.Models.DTOs;

public class PaymentCardSampleDatumDto
{
    public int SamplePaymentCardId { get; set; }
    public string? CardBrand { get; set; }
    public string? AccountNumber { get; set; }
    public string? ExpMonth { get; set; }
    public string? ExpYear { get; set; }
    public string? Cvv { get; set; }
    public string? NtScenario { get; set; }
    public ErrorObject? Error { get; set; }
}
