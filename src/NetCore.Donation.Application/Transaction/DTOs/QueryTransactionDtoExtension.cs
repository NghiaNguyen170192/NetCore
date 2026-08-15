namespace NetCore.Donation.Application.Transaction.DTOs;

public static class QueryTransactionDtoExtension
{
    public static IQueryable<QueryTransactionDto> ToQueryDto(this IQueryable<Domain.Entities.Transaction> transactions)
    {
        return transactions.Select(transaction => new QueryTransactionDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            PaymentScheduleId = transaction.PaymentScheduleId,
            ContactId = transaction.ContactId,
            PaymentMethodId = transaction.PaymentMethodId,
            PaymentType = transaction.PaymentType,
            BookDate = transaction.BookDate,
            ReceivedDate = transaction.ReceivedDate,
        });
    }
}