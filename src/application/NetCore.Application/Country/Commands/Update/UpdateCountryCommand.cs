using MediatR;

namespace NetCore.Application.Commands;

public record UpdateCountryCommand(Guid Id, string Name, string? CountryCode, string? Alpha2, string? Alpha3) : IRequest;