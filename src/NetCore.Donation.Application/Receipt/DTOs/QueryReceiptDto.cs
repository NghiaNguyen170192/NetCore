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

    [JsonPropertyName("payment-schedule-id")]
    public Guid? PaymentScheduleId { get; set; }

    [JsonPropertyName("document-object-key")]
    public string? DocumentObjectKey { get; set; }

    [JsonPropertyName("document-file-name")]
    public string? DocumentFileName { get; set; }

    [JsonPropertyName("document-content-type")]
    public string? DocumentContentType { get; set; }

    [JsonPropertyName("document-generated-at-utc")]
    public DateTime? DocumentGeneratedAtUtc { get; set; }

    [JsonPropertyName("document-size-bytes")]
    public long? DocumentSizeBytes { get; set; }

    [JsonPropertyName("has-document")]
    public bool HasDocument { get; set; }
}
