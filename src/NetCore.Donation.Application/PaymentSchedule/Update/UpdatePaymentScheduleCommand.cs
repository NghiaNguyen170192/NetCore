using MediatR;
using NetCore.Donation.Domain.Enums;

namespace NetCore.Donation.Application.PaymentSchedule.Update;

public sealed record UpdatePaymentScheduleCommand(
    Guid Id,
    Guid PaymentMethodId,
    decimal Amount,
    DateOnly BookDate,
    RecurringInterval RecurringInterval) : IRequest<bool>;