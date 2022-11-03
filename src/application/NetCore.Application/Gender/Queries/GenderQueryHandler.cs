using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Application.Queries.Dtos;
using NetCore.Application.Repositories.Interfaces;
using NetCore.Application.Repositories;
using NetCore.Infrastructure.Database.Commons;
using Azure.Core;

namespace NetCore.Application.Queries;

public class GenderQueryHandler : IRequestHandler<GenderQuery, GenderQueryDto>
{
    private readonly IRepository<Gender> _repository;
	private readonly IRepository<Gender> _cacheRepository;

	public GenderQueryHandler(IRepository<Gender> repository, IRepository<Gender> redisRepository)
    {
		_repository = repository;
		_cacheRepository = redisRepository;
	}

	public async Task<GenderQueryDto> Handle(GenderQuery request, CancellationToken cancellationToken)
    {
        var cacheEntity = await GetEntityFromCacheAsync(request.Id);
        if(cacheEntity is not null)
        {
            return MapResponse(cacheEntity);
        }

        var entity = await _repository.GetByIdAsync(request.Id);
        return MapResponse(entity);
    }

    private async Task<Gender>GetEntityFromCacheAsync(Guid id)
    {
        return await _cacheRepository.GetByIdAsync(id);
    }

    private async Task<Gender> GetEntityAsync(Guid id)
    {
		return await _repository.GetByIdAsync(id);
	}

    private GenderQueryDto MapResponse(Gender entity)
    {
        return new GenderQueryDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
}
