#nullable disable

using NetCore.Donation.Domain.SharedKernel;

namespace NetCore.Donation.Domain.Entities;

public class PaymentMethod : Entity, IAggregateRoot
{
    public Guid ContactId { get; private set; }

    public Contact Contact { get; private set; }

    public string DisplayName { get; private set; }

    public static PaymentMethod Create(Guid contactId, string displayName)
    {
        Validate(contactId, displayName);
        return new PaymentMethod
        {
            ContactId = contactId,
            DisplayName = displayName.Trim(),
        };
    }

    public void UpdateDisplayName(string displayName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        DisplayName = displayName.Trim();
    }

    private static void Validate(Guid contactId, string displayName)
    {
        if (contactId == Guid.Empty)
        {
            throw new ArgumentException("Contact ID is required.", nameof(contactId));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
    }
}