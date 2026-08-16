using MediatR;

namespace NetCore.Donation.Domain.Events;

public sealed record JournalEntryCreatedDomainEvent(Guid JournalId, Guid TransactionId) : INotification;
