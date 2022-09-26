using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
namespace NetCore.Application.Commands;

public class CreateCountryHandler : IRequestHandler<CreateCountryCommand, Guid>
{
    private readonly IRepository<Country> _repository;

    public CreateCountryHandler(IRepository<Country> repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        if (!await Exist(request))
        {
            var entity = Map(request);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }

        return Guid.Empty;
    }

    private async Task<bool> Exist(CreateCountryCommand request)
    {
        return await _repository.ExistAsync(entity => 
                entity.Name == request.Name && entity.CountryCode == request.CountryCode
                && entity.Alpha2 == request.Alpha2 && entity.Alpha3 == entity.Alpha3);
    }

    private Country Map(CreateCountryCommand request)
    {
        return new Country
        {
            Name = request.Name,
            CountryCode = request.CountryCode,
            Alpha2 = request.Alpha2,
            Alpha3 = request.Alpha3,
        };
    }
}
