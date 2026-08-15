using MediatR;

namespace NetCore.Donation.Application.PaymentMethod.Update;

public sealed record UpdatePaymentMethodCommand(Guid Id, string DisplayName) : IRequest<bool>;