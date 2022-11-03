using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Application.Repositories.Interfaces;

namespace NetCore.Application.Commands;

public class DeleteCountryHandler : IRequestHandler<DeleteCountryCommand>
{
    private readonly IRepository<Country> _repository;

    public DeleteCountryHandler(IRepository<Country> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        _repository.Remove(new Country { Id = request.Id});
        await _repository.SaveChangesAsync();

        return Unit.Value;
    }
}
