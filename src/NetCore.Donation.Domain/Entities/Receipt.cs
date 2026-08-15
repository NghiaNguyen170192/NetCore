#nullable disable

using NetCore.Donation.Domain.SharedKernel;

namespace NetCore.Donation.Domain.Entities;

public class Receipt : Entity, IAggregateRoot
{
    public Guid ContactId { get; private set; }

    public Contact Contact { get; private set; }

    public Guid? TransactionId { get; private set; }

    public Transaction Transaction { get; private set; }

    public static Receipt Create(Guid contactId, Guid? transactionId = null)
    {
        Validate(contactId, transactionId);
        return new Receipt
        {
            ContactId = contactId,
            TransactionId = transactionId,
        };
    }

    public void AssignTransaction(Guid transactionId)
    {
        if (transactionId == Guid.Empty)
        {
            throw new ArgumentException("Transaction ID cannot be empty.", nameof(transactionId));
        }

        TransactionId = transactionId;
    }

    public void ClearTransaction() => TransactionId = null;

    private static void Validate(Guid contactId, Guid? transactionId)
    {
        if (contactId == Guid.Empty)
        {
            throw new ArgumentException("Contact ID is required.", nameof(contactId));
        }

        if (transactionId == Guid.Empty)
        {
            throw new ArgumentException("Transaction ID cannot be empty.", nameof(transactionId));
        }
    }
}