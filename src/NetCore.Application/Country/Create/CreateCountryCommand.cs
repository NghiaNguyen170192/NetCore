using NetCore.Domain.Messaging;

namespace NetCore.Application.Country.Create;

public sealed record CreateCountryCommand(string Name, string CountryCode, string Alpha2, string Alpha3)
    : IRequest<Guid>;