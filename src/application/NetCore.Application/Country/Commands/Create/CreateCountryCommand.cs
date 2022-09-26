using MediatR;

namespace NetCore.Application.Commands;

public record CreateCountryCommand(string Name, string CountryCode, string Alpha2, string Alpha3) : IRequest<Guid>;
