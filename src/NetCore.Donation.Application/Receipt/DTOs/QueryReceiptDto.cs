using System.Text.Json.Serialization;

namespace NetCore.Donation.Application.Receipt.DTOs;

public sealed record QueryReceiptDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("contact-id")]
    public Guid ContactId { get; set; }

    [JsonPropertyName("transaction-id")]
    public Guid? TransactionId { get; set; }
}