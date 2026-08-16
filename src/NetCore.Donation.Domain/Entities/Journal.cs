#nullable disable

using NetCore.Donation.Domain.Events;
using NetCore.Donation.Domain.SharedKernel;

namespace NetCore.Donation.Domain.Entities;

public class Journal : Entity, IAggregateRoot
{
    public Guid TransactionId { get; private set; }

    public Transaction Transaction { get; private set; }

    public static Journal Create(Guid transactionId)
    {
        if (transactionId == Guid.Empty)
        {
            throw new ArgumentException("Transaction ID is required.", nameof(transactionId));
        }

        var journal = new Journal
        {
            Id = Guid.NewGuid(),
            TransactionId = transactionId,
        };

        journal.AddDomainEvent(new JournalEntryCreatedDomainEvent(journal.Id, journal.TransactionId));
        return journal;
    }
}
