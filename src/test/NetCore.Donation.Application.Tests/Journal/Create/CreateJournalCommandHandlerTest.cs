using NetCore.Donation.Application.Journal.Create;
using NetCore.Donation.Application.Journal.Delete;
using NetCore.Donation.Application.Journal.GetJournal;
using NetCore.Donation.Domain.IRepositories;
using NetCore.Donation.Domain.SharedKernel;
using NetCore.Donation.Infrastructure.Database.Repositories;

namespace NetCore.Donation.Application.Tests.Journal.Create;

[TestClass]
public class CreateJournalCommandHandlerTest : BaseTest
{
    private readonly IJournalRepository journalRepository;
    private readonly IUnitOfWork unitOfWork;

    public CreateJournalCommandHandlerTest()
    {
        var context = GetContext().Result;
        unitOfWork = context;
        journalRepository = new JournalRepository(context);
    }

    [TestMethod]
    public async Task CreateJournalCommand_ShouldReturnValidGuid()
    {
        var handler = new CreateJournalCommandHandler(unitOfWork, journalRepository);

        var id = await handler.Handle(new CreateJournalCommand(), default);

        Assert.AreNotEqual(Guid.Empty, id);

        var journal = await new GetJournalQueryHandler(journalRepository)
            .Handle(new GetJournalQuery(id), default);

        Assert.IsNotNull(journal);
        Assert.AreEqual(id, journal.Id);
    }

    [TestMethod]
    public async Task DeleteJournalCommand_ShouldReturnFalseWhenMissing()
    {
        var handler = new DeleteJournalCommandHandler(unitOfWork, journalRepository);

        var deleted = await handler.Handle(new DeleteJournalCommand(Guid.NewGuid()), default);

        Assert.IsFalse(deleted);
    }
}
