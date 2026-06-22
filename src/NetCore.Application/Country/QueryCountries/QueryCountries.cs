using MediatR;
using NetCore.Application.Country.DTOs;

namespace NetCore.Application.Country.QueryCountries;

public sealed record QueryCountries : IRequest<IQueryable<QueryCountryDto>>;