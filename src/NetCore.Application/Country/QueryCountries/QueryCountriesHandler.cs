﻿using MediatR;
using NetCore.Application.Country.DTOs;
using NetCore.Domain.IRepositories;

namespace NetCore.Application.Country.QueryCountries;

public class QueryCountriesHandler(ICountryRepository countryRepository)
    : IRequestHandler<QueryCountries, IQueryable<QueryCountryDto>>
{
    public Task<IQueryable<QueryCountryDto>> Handle(QueryCountries request, CancellationToken cancellationToken)
	{
		return Task.FromResult(countryRepository.GetAll()
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
