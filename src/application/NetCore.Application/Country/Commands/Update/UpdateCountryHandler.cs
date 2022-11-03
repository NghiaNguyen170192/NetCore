using MediatR;
using NetCore.Infrastructure.Database.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NetCore.Application.Repositories.Interfaces;

namespace NetCore.Application.Commands;

public class UpdateCountryHandler : IRequestHandler<UpdateCountryCommand>
{
    private readonly IRepository<Country> _repository;

    public UpdateCountryHandler(IRepository<Country> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        var existingEntity = await _repository.GetByIdAsync(request.Id);
        if (existingEntity == null)
        {
            //todo: handle exception
            throw new NotImplementedException();
        }

        existingEntity.Name = request.Name;
        existingEntity.CountryCode = request.CountryCode ?? existingEntity.CountryCode;
        existingEntity.Alpha2 = request.Alpha2 ?? existingEntity.Alpha2;
        existingEntity.Alpha3 = request.Alpha2 ?? existingEntity.Alpha3;

        _repository.Update(existingEntity);
        await _repository.SaveChangesAsync();

        return Unit.Value;
    }
}
