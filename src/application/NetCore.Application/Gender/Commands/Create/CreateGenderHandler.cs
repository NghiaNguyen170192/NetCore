using MediatR;
using NetCore.Application.Repositories.Interfaces;
using NetCore.Infrastructure.Database.Commons;
using NetCore.Infrastructure.Database.Entities;

namespace NetCore.Application.Commands;

public class CreateGenderHandler : IRequestHandler<CreateGenderCommand, Guid>
{
    private readonly IRepository<Gender> _repository;
    private readonly IRepository<Gender> _cacheRepository;

	public CreateGenderHandler(IRepository<Gender> repository, IRepository<Gender> redisRepository)
    {
        _repository = repository;
		_cacheRepository = redisRepository;
	}

    public async Task<Guid> Handle(CreateGenderCommand request, CancellationToken cancellationToken)
    {
        if (await Exist(request))
        {
			throw new ArgumentException("Data Exists");
		}

        var entity = await CreateEntity(request);
        await CreateEntityInCache(entity);

        return entity.Id;
    }

    private async Task<Gender> CreateEntity(CreateGenderCommand request)
    {
		var entity = Map(request);
		await _repository.AddAsync(entity);
		await _repository.SaveChangesAsync();

		return entity;
	}

	private async Task CreateEntityInCache(Gender entity)
    {
        await _cacheRepository.AddAsync(entity);
    }

	private async Task<bool> Exist(CreateGenderCommand request)
    {
        return await _repository.ExistAsync(entity => entity.Name == request.Name);
    }

    private Gender Map(CreateGenderCommand request)
    {
        return new Gender
        {
            Name = request.Name,
        };
    }
}
