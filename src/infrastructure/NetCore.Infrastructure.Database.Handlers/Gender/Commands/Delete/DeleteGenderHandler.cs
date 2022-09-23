using MediatR;
using NetCore.Infrastructure.Database.Entities;
using System.Threading;
using System.Threading.Tasks;
using NetCore.Infrastructure.Database.Repositories;

namespace  NetCore.Infrastructure.Database.Handlers.Commands;

public class DeleteGenderHandler : IRequestHandler<DeleteGenderCommand>
{
    private readonly IRepository<Gender> _repository;

    public DeleteGenderHandler(IRepository<Gender> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteGenderCommand request, CancellationToken cancellationToken)
    {
        _repository.Remove(new Gender { Id = request.Id});
        await _repository.SaveChangesAsync();

        return Unit.Value;
    }
}
