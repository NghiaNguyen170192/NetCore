#nullable disable

using NetCore.Donation.Domain.SharedKernel;

namespace NetCore.Donation.Domain.Entities;

public class Contact : Entity, IAggregateRoot
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public DateOnly DateOfBirth { get; private set; }

    public string AddressLine { get; private set; }

    public string Email { get; private set; }

    public string PhoneNumber { get; private set; }

    public bool IsActive { get; private set; }

    public Guid CountryId { get; private set; }

    public Country Country { get; private set; }

    public static Contact Create(
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        string addressLine,
        string email,
        string phoneNumber,
        Guid countryId)
    {
        Validate(firstName, lastName, dateOfBirth, addressLine, email, phoneNumber, countryId);
        return new Contact
        {
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            DateOfBirth = dateOfBirth,
            AddressLine = addressLine.Trim(),
            Email = email.Trim(),
            PhoneNumber = phoneNumber.Trim(),
            IsActive = true,
            CountryId = countryId,
        };
    }

    public void UpdateDetails(
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        string addressLine,
        string email,
        string phoneNumber,
        Guid countryId)
    {
        Validate(firstName, lastName, dateOfBirth, addressLine, email, phoneNumber, countryId);
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        DateOfBirth = dateOfBirth;
        AddressLine = addressLine.Trim();
        Email = email.Trim();
        PhoneNumber = phoneNumber.Trim();
        CountryId = countryId;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    private static void Validate(
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        string addressLine,
        string email,
        string phoneNumber,
        Guid countryId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber);

        if (dateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentOutOfRangeException(nameof(dateOfBirth), "Date of birth cannot be in the future.");
        }

        if (countryId == Guid.Empty)
        {
            throw new ArgumentException("Country ID is required.", nameof(countryId));
        }
    }
}