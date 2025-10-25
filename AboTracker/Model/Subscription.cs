using System.Text.Json.Serialization;

namespace AboTracker.Model;

public class Subscription
{
    // JsonPropertyName maps the JSON name to the C# name
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("paymentPeriod")]
    public string PaymentPeriod { get; set; } = string.Empty;

    [JsonPropertyName("purchaseDate")]
    public string PurchaseDate { get; set; } = string.Empty;

    [JsonPropertyName("nextPaymentDate")]
    public string NextPaymentDate { get; set; } = string.Empty;
}