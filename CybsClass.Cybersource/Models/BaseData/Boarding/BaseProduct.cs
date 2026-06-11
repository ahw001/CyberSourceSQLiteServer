using System.Text.Json.Serialization;

#nullable enable

namespace CybsClass.Cybersource.Models.BaseData.Boarding;

public sealed class BaseProductDto
{
    [JsonPropertyName("setups")]
    public BaseProductSetups? Setups { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public sealed class BaseProductSetups
{
    [JsonPropertyName("payments")]
    public BaseProductPayments? Payments { get; set; }
}

public sealed class BaseProductPayments
{
}
