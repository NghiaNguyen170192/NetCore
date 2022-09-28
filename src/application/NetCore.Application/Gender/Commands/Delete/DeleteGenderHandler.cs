using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;

namespace NetCore.Application.Commands;

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
