using System.Text.Json.Serialization;

namespace NetCore.Donation.Application.PaymentMethod.DTOs;

public sealed record QueryPaymentMethodDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("contact-id")]
    public Guid ContactId { get; set; }

    [JsonPropertyName("display-name")]
    public string DisplayName { get; set; } = string.Empty;
}