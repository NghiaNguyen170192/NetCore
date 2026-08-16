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
