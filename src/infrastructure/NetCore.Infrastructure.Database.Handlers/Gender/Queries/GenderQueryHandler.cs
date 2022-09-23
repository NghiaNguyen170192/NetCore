using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Database.Handlers.Queries;

public class GenderQueryHandler : IRequestHandler<GenderQuery, QueryGenderResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IRepository<Gender> _repository;

    public GenderQueryHandler(IRepository<Gender> repository)
    {
        _repository = repository;
    }

    public async Task<QueryGenderResponse> Handle(GenderQuery request, CancellationToken cancellationToken)
    {
        var entity = _repository.Collection.SingleOrDefault(x => x.Id == request.Id);
        return MapResponse(entity);
    }

    private QueryGenderResponse MapResponse(Gender entity)
    {
        return new QueryGenderResponse (entity.Id, entity.Name);
    }
}
