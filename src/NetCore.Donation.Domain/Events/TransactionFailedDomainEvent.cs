using MediatR;

namespace NetCore.Donation.Domain.Events;

public sealed record TransactionFailedDomainEvent(
    Guid TransactionId,
    Guid ContactId,
    decimal Amount) : INotification;
