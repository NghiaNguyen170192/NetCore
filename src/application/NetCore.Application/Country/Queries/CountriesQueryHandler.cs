using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using NetCore.Application.Queries.Dtos;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;

namespace NetCore.Application.Queries;

public class CountriesQueryHandler : IRequestHandler<CountriesQuery, IQueryable<CountryQueryDto>>
{
    private readonly IRepository<Country> _repository;

    public CountriesQueryHandler(IRepository<Country> repository)
    {
        _repository = repository;
    }

    public async Task<IQueryable<CountryQueryDto>> Handle(CountriesQuery request, CancellationToken cancellationToken)
    {

        var dbCountries = _repository.Collection
            .AsNoTracking()
            .Select(x => new CountryQueryDto()
            {
                Id = x.Id,
                Name = x.Name,
                CountryCode = x.CountryCode,
                Alpha2 = x.Alpha2,
                Alpha3 = x.Alpha3
            });

        return dbCountries;
    }
}
