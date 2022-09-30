using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCore.Application.Queries.Dtos;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;

namespace NetCore.Application.Queries;

public class GendersQueryHandler : IRequestHandler<GendersQuery, IEnumerable<GenderQueryDto>>
{
    private readonly IRepository<Gender> _repository;

    public GendersQueryHandler(IRepository<Gender> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GenderQueryDto>> Handle(GendersQuery request, CancellationToken cancellationToken)
    {
        return _repository.Collection
            .AsNoTracking()
            .Select(x => new GenderQueryDto()
            {
                Id = x.Id,
                Name = x.Name
            })
            .AsEnumerable();
    }
}
