#nullable disable

using NetCore.Donation.Domain.Enums;
using NetCore.Donation.Domain.SharedKernel;

namespace NetCore.Donation.Domain.Entities;

public class Transaction : Entity, IAggregateRoot
{
    public decimal Amount { get; private set; }

    public Guid PaymentScheduleId { get; private set; }

    public PaymentSchedule PaymentSchedule { get; private set; }

    public Guid ContactId { get; private set; }

    public Contact Contact { get; private set; }

    public Guid PaymentMethodId { get; private set; }

    public PaymentMethod PaymentMethod { get; private set; }

    public PaymentType PaymentType { get; private set; }

    public DateOnly BookDate { get; private set; }

    public DateOnly ReceivedDate { get; private set; }

    public static Transaction Create(
        decimal amount,
        Guid paymentScheduleId,
        Guid contactId,
        Guid paymentMethodId,
        PaymentType paymentType,
        DateOnly bookDate,
        DateOnly receivedDate)
    {
        Validate(amount, paymentScheduleId, contactId, paymentMethodId, paymentType, bookDate, receivedDate);
        return new Transaction
        {
            Amount = amount,
            PaymentScheduleId = paymentScheduleId,
            ContactId = contactId,
            PaymentMethodId = paymentMethodId,
            PaymentType = paymentType,
            BookDate = bookDate,
            ReceivedDate = receivedDate,
        };
    }

    public void UpdateReceiptDetails(decimal amount, PaymentType paymentType, DateOnly receivedDate)
    {
        Validate(amount, PaymentScheduleId, ContactId, PaymentMethodId, paymentType, BookDate, receivedDate);
        Amount = amount;
        PaymentType = paymentType;
        ReceivedDate = receivedDate;
    }

    private static void Validate(
        decimal amount,
        Guid paymentScheduleId,
        Guid contactId,
        Guid paymentMethodId,
        PaymentType paymentType,
        DateOnly bookDate,
        DateOnly receivedDate)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
        }

        if (paymentScheduleId == Guid.Empty)
        {
            throw new ArgumentException("Payment schedule ID is required.", nameof(paymentScheduleId));
        }

        if (contactId == Guid.Empty)
        {
            throw new ArgumentException("Contact ID is required.", nameof(contactId));
        }

        if (paymentMethodId == Guid.Empty)
        {
            throw new ArgumentException("Payment method ID is required.", nameof(paymentMethodId));
        }

        if (!Enum.IsDefined(paymentType))
        {
            throw new ArgumentOutOfRangeException(nameof(paymentType));
        }

        if (receivedDate < bookDate)
        {
            throw new ArgumentOutOfRangeException(nameof(receivedDate), "Received date cannot precede book date.");
        }
    }
}