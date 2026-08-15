using System.Text.Json.Serialization;

namespace NetCore.Donation.Application.Contact.DTOs;

public sealed record QueryContactDto
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

    [JsonPropertyName("country-id")]
    public Guid CountryId { get; set; }

    [JsonPropertyName("country-name")]
    public string CountryName { get; set; } = string.Empty;
}