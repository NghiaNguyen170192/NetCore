using MediatR;

namespace NetCore.Donation.Application.PaymentMethod.Create;

public sealed record CreatePaymentMethodCommand(Guid ContactId, string DisplayName) : IRequest<Guid>;