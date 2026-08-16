using MediatR;

namespace NetCore.Donation.Domain.Events;

public sealed record DonationReceiptGeneratedDomainEvent(
    Guid ReceiptId,
    Guid ContactId,
    Guid? TransactionId) : INotification;
