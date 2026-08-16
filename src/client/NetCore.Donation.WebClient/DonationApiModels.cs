using System.Text.Json.Serialization;

namespace NetCore.Donation.WebClient;

public enum PaymentType
{
    Bank = 0,
    CreditCard = 1,
    DebitCard = 2,
    Cash = 3,
    PayPal = 4,
}

public enum RecurringInterval
{
    Daily = 0,
    Weekly = 1,
    Biweekly = 2,
    Monthly = 3,
    Quarterly = 4,
    Yearly = 5,
    OneOff = 6,
}

public sealed class UserMakesDonationResponse
{
    public Guid ContactId { get; set; }

    public Guid PaymentMethodId { get; set; }

    public Guid PaymentScheduleId { get; set; }

    public bool IsRecurring { get; set; }
}

public sealed class IdResponse
{
    public Guid Id { get; set; }
}

public sealed class CountryDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public sealed class ContactDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("first-name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("last-name")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("date-of-birth")]
    public DateOnly DateOfBirth { get; set; }

    [JsonPropertyName("address-line")]
    public string AddressLine { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("phone-number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("is-active")]
    public bool IsActive { get; set; }

    [JsonPropertyName("do-not-email")]
    public bool DoNotEmail { get; set; }

    [JsonPropertyName("do-not-sms")]
    public bool DoNotSms { get; set; }

    [JsonPropertyName("country-id")]
    public Guid CountryId { get; set; }

    [JsonPropertyName("country-name")]
    public string CountryName { get; set; } = string.Empty;
}

public sealed class PaymentMethodDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("contact-id")]
    public Guid ContactId { get; set; }

    [JsonPropertyName("display-name")]
    public string DisplayName { get; set; } = string.Empty;
}

public sealed class PaymentScheduleDto
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

    [JsonPropertyName("is-recurring")]
    public bool IsRecurring { get; set; }
}

public enum TransactionStatus
{
    Pending = 0,
    Succeeded = 1,
    Failed = 2,
}

public sealed class TransactionDto
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

public sealed class JournalDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("transaction-id")]
    public Guid TransactionId { get; set; }

    [JsonPropertyName("created-date")]
    public DateTime CreatedDate { get; set; }
}

public sealed class ReceiptDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("contact-id")]
    public Guid ContactId { get; set; }

    [JsonPropertyName("transaction-id")]
    public Guid? TransactionId { get; set; }

    [JsonPropertyName("payment-schedule-id")]
    public Guid? PaymentScheduleId { get; set; }

    [JsonPropertyName("document-file-name")]
    public string? DocumentFileName { get; set; }

    [JsonPropertyName("has-document")]
    public bool HasDocument { get; set; }
}

public sealed class OutboxMessageDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("message-type")]
    public string MessageType { get; set; } = string.Empty;

    [JsonPropertyName("payload")]
    public string Payload { get; set; } = string.Empty;

    [JsonPropertyName("correlation-id")]
    public string CorrelationId { get; set; } = string.Empty;

    [JsonPropertyName("idempotency-key")]
    public string IdempotencyKey { get; set; } = string.Empty;

    [JsonPropertyName("occurred-at-utc")]
    public DateTime OccurredAtUtc { get; set; }

    [JsonPropertyName("processed-at-utc")]
    public DateTime? ProcessedAtUtc { get; set; }

    [JsonPropertyName("attempt-count")]
    public int AttemptCount { get; set; }

    [JsonPropertyName("last-error")]
    public string? LastError { get; set; }
}

public sealed class DonationFlowStepDto
{
    [JsonPropertyName("event-name")]
    public string EventName { get; set; } = string.Empty;

    [JsonPropertyName("occurred-at-utc")]
    public DateTime OccurredAtUtc { get; set; }

    [JsonPropertyName("processed-at-utc")]
    public DateTime? ProcessedAtUtc { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("correlation-id")]
    public string CorrelationId { get; set; } = string.Empty;
}

public sealed class DonationFlowDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("payment-schedule-id")]
    public Guid? PaymentScheduleId { get; set; }

    [JsonPropertyName("contact-id")]
    public Guid? ContactId { get; set; }

    [JsonPropertyName("payment-method-id")]
    public Guid? PaymentMethodId { get; set; }

    [JsonPropertyName("transaction-id")]
    public Guid? TransactionId { get; set; }

    [JsonPropertyName("journal-id")]
    public Guid? JournalId { get; set; }

    [JsonPropertyName("receipt-id")]
    public Guid? ReceiptId { get; set; }

    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("money-path")]
    public string MoneyPath { get; set; } = string.Empty;

    [JsonPropertyName("started-at-utc")]
    public DateTime StartedAtUtc { get; set; }

    [JsonPropertyName("last-event-at-utc")]
    public DateTime LastEventAtUtc { get; set; }

    [JsonPropertyName("steps")]
    public List<DonationFlowStepDto> Steps { get; set; } = [];
}
