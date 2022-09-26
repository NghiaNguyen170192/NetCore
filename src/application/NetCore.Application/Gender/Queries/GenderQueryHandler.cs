using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Application.Queries.Dtos;
using NetCore.Infrastructure.Database.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Application.Queries;

public class GenderQueryHandler : IRequestHandler<GenderQuery, GenderQueryDto>
{
    private readonly IRepository<Gender> _repository;

    public GenderQueryHandler(IRepository<Gender> repository)
    {
        _repository = repository;
    }

    public async Task<GenderQueryDto> Handle(GenderQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.Collection.SingleOrDefaultAsync(x => x.Id == request.Id);
        return MapResponse(entity);
    }

    private GenderQueryDto MapResponse(Gender entity)
    {
        return new GenderQueryDto (entity.Id, entity.Name);
    }
}
