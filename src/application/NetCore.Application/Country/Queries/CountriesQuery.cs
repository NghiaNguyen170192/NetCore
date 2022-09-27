using MediatR;
using NetCore.Application.Queries.Dtos;

namespace NetCore.Application.Queries;
public record CountriesQuery2 : IRequest<IEnumerable<CountryQueryDto>>;