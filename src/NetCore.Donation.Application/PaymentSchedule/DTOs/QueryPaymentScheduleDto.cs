using NetCore.Donation.Domain.Enums;
using System.Text.Json.Serialization;

namespace NetCore.Donation.Application.PaymentSchedule.DTOs;

public sealed record QueryPaymentScheduleDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("contact-id")]
    public Guid ContactId { get; set; }

    [JsonPropertyName("payment-method-id")]
    public Guid PaymentMethodId { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("book-date")]
    public DateOnly BookDate { get; set; }

    [JsonPropertyName("recurring-interval")]
    public RecurringInterval RecurringInterval { get; set; }
}