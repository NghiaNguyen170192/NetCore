using MediatR;

namespace NetCore.Donation.Application.Journal.Create;

public sealed record CreateJournalCommand : IRequest<Guid>;
