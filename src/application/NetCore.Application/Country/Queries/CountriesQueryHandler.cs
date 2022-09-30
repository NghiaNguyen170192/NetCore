using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using NetCore.Application.Queries.Dtos;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using Newtonsoft.Json;
using System.Text;

namespace NetCore.Application.Queries;

public class CountriesQueryHandler : IRequestHandler<CountriesQuery, IEnumerable<CountryQueryDto>>
{
    private readonly IRepository<Country> _repository;
    private readonly IDistributedCache _distributedCache;

    public CountriesQueryHandler(IRepository<Country> repository, IDistributedCache distributedCache)
    {
        _repository = repository;
        _distributedCache = distributedCache;
    }

    public async Task<IEnumerable<CountryQueryDto>> Handle(CountriesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "countries";
        string serializedCountries;
        var cachedCountries = await _distributedCache.GetAsync(cacheKey, cancellationToken);

        if (cachedCountries != null)
        {
            serializedCountries = Encoding.UTF8.GetString(cachedCountries);
            var countries = JsonConvert.DeserializeObject<IEnumerable<CountryQueryDto>>(serializedCountries);
            return countries;
        }

        var dbCountries = _repository.Collection
            .AsNoTracking()
            .Select(x => new CountryQueryDto()
            {
                Id = x.Id,
                Name = x.Name,
                CountryCode = x.CountryCode,
                Alpha2 = x.Alpha2,
                Alpha3 = x.Alpha3
            })
            .AsEnumerable();

        serializedCountries = JsonConvert.SerializeObject(dbCountries);
        cachedCountries = Encoding.UTF8.GetBytes(serializedCountries);
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
            .SetSlidingExpiration(TimeSpan.FromMinutes(2));
        await _distributedCache.SetAsync(cacheKey, cachedCountries, options);
        return dbCountries;
    }
}
