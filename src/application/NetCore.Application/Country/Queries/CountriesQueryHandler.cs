using MediatR;
using NetCore.Application.Queries.Dtos;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;

namespace NetCore.Application.Queries;

public class CountriesQueryHandler : IRequestHandler<CountriesQuery, IEnumerable<CountryQueryDto>>
{
    private readonly IRepository<Country> _repository;

    public CountriesQueryHandler(IRepository<Country> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CountryQueryDto>> Handle(CountriesQuery request, CancellationToken cancellationToken)
    {
        var query = GetQuery(request);
        return query.Select(x => MapResponse(x)).AsEnumerable(); 
    }

    private IQueryable<Country> GetQuery(CountriesQuery request)
    {
        var query = _repository.Collection;
        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(x => x.Name == request.Name);
        }

        if (!string.IsNullOrEmpty(request.CountryCode))
        {
            query = query.Where(x => x.CountryCode == request.CountryCode);
        }

        if (!string.IsNullOrEmpty(request.Alpha2))
        {
            query = query.Where(x => x.Alpha2 == request.Alpha2);
        }

        if (!string.IsNullOrEmpty(request.Alpha3))
        {
            query = query.Where(x => x.Alpha3 == request.Alpha3);
        }

        return query;
    }

    private CountryQueryDto MapResponse(Country entity)
    {
        return new CountryQueryDto(entity.Id, entity.Name, entity.CountryCode, entity.Alpha2, entity.Alpha3);
    }
}
