using MediatR;

namespace NetCore.Donation.Domain.Events;

public sealed record TransactionSucceededDomainEvent(
    Guid TransactionId,
    Guid ContactId,
    Guid PaymentScheduleId,
    decimal Amount) : INotification;
