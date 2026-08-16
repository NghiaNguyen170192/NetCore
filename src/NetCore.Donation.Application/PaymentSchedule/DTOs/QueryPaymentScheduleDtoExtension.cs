using NetCore.Donation.Domain.Enums;

namespace NetCore.Donation.Application.PaymentSchedule.DTOs;

public static class QueryPaymentScheduleDtoExtension
{
    public static IQueryable<QueryPaymentScheduleDto> ToQueryDto(
        this IQueryable<Domain.Entities.PaymentSchedule> paymentSchedules)
    {
        return paymentSchedules.Select(paymentSchedule => new QueryPaymentScheduleDto
        {
            Id = paymentSchedule.Id,
            ContactId = paymentSchedule.ContactId,
            PaymentMethodId = paymentSchedule.PaymentMethodId,
            Amount = paymentSchedule.Amount,
            BookDate = paymentSchedule.BookDate,
            RecurringInterval = paymentSchedule.RecurringInterval,
            IsRecurring = paymentSchedule.RecurringInterval != RecurringInterval.OneOff,
        });
    }
}