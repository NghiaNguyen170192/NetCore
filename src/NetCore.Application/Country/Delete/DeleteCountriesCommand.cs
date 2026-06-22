using MediatR;

namespace NetCore.Application.Country.Delete;

public sealed record DeleteCountriesCommand(IEnumerable<Guid> Ids) : IRequest<bool>;