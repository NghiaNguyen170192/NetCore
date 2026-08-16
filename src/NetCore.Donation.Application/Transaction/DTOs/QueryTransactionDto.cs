using NetCore.Donation.Domain.Enums;
using System.Text.Json.Serialization;

namespace NetCore.Donation.Application.Transaction.DTOs;

public sealed record QueryTransactionDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("payment-schedule-id")]
    public Guid PaymentScheduleId { get; set; }

    [JsonPropertyName("contact-id")]
    public Guid ContactId { get; set; }

    [JsonPropertyName("payment-method-id")]
    public Guid PaymentMethodId { get; set; }

    [JsonPropertyName("payment-type")]
    public PaymentType PaymentType { get; set; }

    [JsonPropertyName("status")]
    public TransactionStatus Status { get; set; }

    [JsonPropertyName("book-date")]
    public DateOnly BookDate { get; set; }

    [JsonPropertyName("received-date")]
    public DateOnly ReceivedDate { get; set; }
}