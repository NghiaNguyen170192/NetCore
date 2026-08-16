namespace NetCore.Donation.Application.Journal.DTOs;

public static class QueryJournalDtoExtension
{
    public static IQueryable<QueryJournalDto> ToQueryDto(this IQueryable<Domain.Entities.Journal> journals)
    {
        return journals.Select(journal => new QueryJournalDto
        {
            Id = journal.Id,
            TransactionId = journal.TransactionId,
            CreatedDate = journal.CreatedDate,
            ModifiedDate = journal.ModifiedDate,
        });
    }
}
