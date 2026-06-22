using MediatR;

namespace NetCore.Application.Country.Create;

public sealed record CreateCountriesCommand : IRequest<IEnumerable<Guid>>
{
    public required IEnumerable<CreateCountryCommand> Countries { get; init; }
}