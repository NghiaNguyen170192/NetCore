using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Database.Handlers.Commands;

public class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand>
{
    private readonly IRepository<Person> _repository;

    public UpdatePersonHandler(IRepository<Person> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var existingEntity = await _repository.GetByIdAsync(request.Id);
        if (existingEntity == null)
        {
            //todo: handle exception
            throw new NotImplementedException();
        }

        MapPerson(request, existingEntity);
        await _repository.AddAsync(existingEntity);
        await _repository.SaveChangesAsync();

        return Unit.Value;
    }

    private void MapPerson(UpdatePersonCommand request, Person existingEntity)
    {
        //existingEntity.BirthYear = request.BirthYear.Value;
        //existingEntity.DeathYear = request.DeathYear.Value;
        //existingEntity.NameConst = request.NameConst;
        //existingEntity.PrimaryName = request.PrimaryName;

        //if (request.BirthYear != null && request.BirthYear != existingEntity.BirthYear)
        //{
        //    existingEntity.BirthYear = request.BirthYear.Value;
        //}

        //if (request.DeathYear != null && request.DeathYear != existingEntity.DeathYear)
        //{
        //    existingEntity.DeathYear = request.DeathYear.Value;
        //}

        //if (request.NameConst != null && request.NameConst != existingEntity.NameConst)
        //{
        //    existingEntity.NameConst = request.NameConst;
        //}

        //if (request.PrimaryName != null && request.PrimaryName != existingEntity.PrimaryName)
        //{
        //    existingEntity.PrimaryName = request.PrimaryName;
        //}
    }
}
