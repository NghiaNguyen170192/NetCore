using System.Text.Json.Serialization;

namespace NetCore.Donation.Application.Journal.DTOs;

public sealed record QueryJournalDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("transaction-id")]
    public Guid TransactionId { get; set; }

    [JsonPropertyName("created-date")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("modified-date")]
    public DateTime ModifiedDate { get; set; }
}
