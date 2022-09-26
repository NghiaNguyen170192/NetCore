using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Application.Queries.Dtos;
using NetCore.Infrastructure.Database.Repositories;

namespace NetCore.Application.Queries;

public class CountryQueryHandler : IRequestHandler<CountryQuery, CountryQueryDto>
{
    private readonly IRepository<Country> _repository;

    public CountryQueryHandler(IRepository<Country> repository)
    {
        _repository = repository;
    }

    public async Task<CountryQueryDto> Handle(CountryQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.Collection.SingleOrDefaultAsync(x => x.Id == request.Id);
        return MapResponse(entity);
    }

    private CountryQueryDto MapResponse(Country entity)
    {
        return new CountryQueryDto (entity.Id, entity.Name, entity.CountryCode, entity.Alpha2, entity.Alpha3);
    }
}
