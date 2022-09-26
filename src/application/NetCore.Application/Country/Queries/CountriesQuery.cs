using MediatR;
using NetCore.Application.Queries.Dtos;

namespace NetCore.Application.Queries;

public record CountriesQuery : IRequest<IEnumerable<CountryQueryDto>>
{
    public string? Name { get; init; }
    public string? CountryCode { get; init; }
    public string? Alpha2 { get; init; }
    public string? Alpha3 { get; init; }
}
