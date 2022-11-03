using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Application.Repositories.Interfaces;
using NetCore.Application.Repositories;

namespace NetCore.Application.Commands;

public class DeleteGenderHandler : IRequestHandler<DeleteGenderCommand>
{
    private readonly IRepository<Gender> _repository;
	private readonly IRepository<Gender> _cacheRepository;

	public DeleteGenderHandler(IRepository<Gender> repository, IRepository<Gender> redisRepository)
    {
		_repository = repository;
		_cacheRepository = redisRepository;
	}

	public async Task<Unit> Handle(DeleteGenderCommand request, CancellationToken cancellationToken)
    {
        _repository.Remove(new Gender { Id = request.Id});
        await _repository.SaveChangesAsync();

        return Unit.Value;
    }
}
