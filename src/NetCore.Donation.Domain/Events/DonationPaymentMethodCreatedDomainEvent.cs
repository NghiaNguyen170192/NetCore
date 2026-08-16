using MediatR;

namespace NetCore.Donation.Domain.Events;

public sealed record DonationPaymentMethodCreatedDomainEvent(
    Guid PaymentMethodId,
    Guid ContactId,
    string DisplayName) : INotification;
