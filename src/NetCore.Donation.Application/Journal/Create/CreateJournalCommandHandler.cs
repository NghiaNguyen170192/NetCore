using MediatR;
using NetCore.Donation.Domain.IRepositories;
using NetCore.Donation.Domain.SharedKernel;

namespace NetCore.Donation.Application.Journal.Create;

public class CreateJournalCommandHandler(
    IUnitOfWork unitOfWork,
    IJournalRepository journalRepository)
    : IRequestHandler<CreateJournalCommand, Guid>
{
    public async Task<Guid> Handle(CreateJournalCommand request, CancellationToken cancellationToken)
    {
        var journal = request.ToDbEntity();

        await journalRepository.AddAsync(journal, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return journal.Id;
    }
}
