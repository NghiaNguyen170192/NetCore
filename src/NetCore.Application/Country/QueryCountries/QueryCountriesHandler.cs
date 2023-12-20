using MediatR;
using NetCore.Application.Country.DTOs;
using NetCore.Domain.IRepositories;

namespace NetCore.Application.Country.QueryCountries;

public class QueryCountriesHandler : IRequestHandler<QueryCountries, IQueryable<QueryCountryDto>>
{
	private readonly ICountryRepository _countryRepository;

	public QueryCountriesHandler(ICountryRepository countryRepository)
	{
		_countryRepository = countryRepository;
	}

	public Task<IQueryable<QueryCountryDto>> Handle(QueryCountries request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_countryRepository.GetAll()
			.Select(country => new QueryCountryDto
			{
				Id = country.Id,
				Name = country.Name,
				CountryCode = country.CountryCode,
				Alpha2 = country.Alpha2,
				Alpha3 = country.Alpha3
			}));
	}
}
