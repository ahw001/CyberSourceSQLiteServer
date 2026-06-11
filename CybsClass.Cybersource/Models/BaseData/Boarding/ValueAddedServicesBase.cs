using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable

namespace CybsClass.Cybersource.Models.BaseData.Boarding;

public sealed class ValueAddedServicesBaseDto
{
    [JsonPropertyName("setups")]
    public ValueAddedServicesSetups? Setups { get; set; }

    [JsonPropertyName("error")]
    public ErrorObject? Error { get; set; }
}

public sealed class ValueAddedServicesSetups
{
    [JsonPropertyName("valueAddedServices")]
    public ValueAddedServicesProducts? ValueAddedServices { get; set; }

    [JsonPropertyName("disputeManagement")]
    public Dictionary<string, object?>? DisputeManagement { get; set; }
}

public sealed class ValueAddedServicesProducts
{
    [JsonPropertyName("transactionSearch")]
    public TransactionSearchProduct? TransactionSearch { get; set; }

    [JsonPropertyName("reporting")]
    public ReportingProduct? Reporting { get; set; }
}

public sealed class TransactionSearchProduct
{
    [JsonPropertyName("subscriptionInformation")]
    public BasicSubscriptionInformation? SubscriptionInformation { get; set; }
}

public sealed class ReportingProduct
{
    [JsonPropertyName("subscriptionInformation")]
    public BasicSubscriptionInformation? SubscriptionInformation { get; set; }
}
