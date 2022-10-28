using MediatR;
using NetCore.Infrastructure.Database.Entities;
using System.Threading;
using System.Threading.Tasks;
using NetCore.Application.Repositories;

namespace NetCore.Application.Commands;

public class DeletePersonHandler : IRequestHandler<DeletePersonCommand>
{
    private readonly IRepository<Person> _repository;

    public DeletePersonHandler(IRepository<Person> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        _repository.Remove(new Person { Id = request.Id});
        await _repository.SaveChangesAsync();
        return Unit.Value;
    }
}
