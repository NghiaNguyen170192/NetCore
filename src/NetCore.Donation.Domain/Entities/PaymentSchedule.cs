#nullable disable

using NetCore.Donation.Domain.Enums;
using NetCore.Donation.Domain.SharedKernel;

namespace NetCore.Donation.Domain.Entities;

public class PaymentSchedule : Entity, IAggregateRoot
{
    public Guid ContactId { get; private set; }

    public Contact Contact { get; private set; }

    public Guid PaymentMethodId { get; private set; }

    public PaymentMethod PaymentMethod { get; private set; }

    public decimal Amount { get; private set; }

    public DateOnly BookDate { get; private set; }

    public RecurringInterval RecurringInterval { get; private set; }

    public static PaymentSchedule Create(
        Guid contactId,
        Guid paymentMethodId,
        decimal amount,
        DateOnly bookDate,
        RecurringInterval recurringInterval)
    {
        Validate(contactId, paymentMethodId, amount, recurringInterval);
        return new PaymentSchedule
        {
            ContactId = contactId,
            PaymentMethodId = paymentMethodId,
            Amount = amount,
            BookDate = bookDate,
            RecurringInterval = recurringInterval,
        };
    }

    public void UpdateSchedule(
        Guid paymentMethodId,
        decimal amount,
        DateOnly bookDate,
        RecurringInterval recurringInterval)
    {
        Validate(ContactId, paymentMethodId, amount, recurringInterval);
        PaymentMethodId = paymentMethodId;
        Amount = amount;
        BookDate = bookDate;
        RecurringInterval = recurringInterval;
    }

    private static void Validate(
        Guid contactId,
        Guid paymentMethodId,
        decimal amount,
        RecurringInterval recurringInterval)
    {
        if (contactId == Guid.Empty)
        {
            throw new ArgumentException("Contact ID is required.", nameof(contactId));
        }

        if (paymentMethodId == Guid.Empty)
        {
            throw new ArgumentException("Payment method ID is required.", nameof(paymentMethodId));
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
        }

        if (!Enum.IsDefined(recurringInterval))
        {
            throw new ArgumentOutOfRangeException(nameof(recurringInterval));
        }
    }
}