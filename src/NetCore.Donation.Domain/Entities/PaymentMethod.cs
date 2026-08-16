#nullable disable

using NetCore.Donation.Domain.Events;
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
        var paymentMethod = new PaymentMethod
        {
            Id = Guid.NewGuid(),
            ContactId = contactId,
            DisplayName = displayName.Trim(),
        };

        paymentMethod.AddDomainEvent(new DonationPaymentMethodCreatedDomainEvent(
            paymentMethod.Id,
            paymentMethod.ContactId,
            paymentMethod.DisplayName));
        return paymentMethod;
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