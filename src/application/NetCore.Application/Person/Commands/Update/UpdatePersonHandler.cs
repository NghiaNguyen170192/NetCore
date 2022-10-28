using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Application.Commands;

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
        //existingEntity.FirstName = request.FirstName;
        //existingEntity.DeathYear = request.DeathYear.Value;
        //existingEntity.NameConst = request.NameConst;
        //existingEntity.PrimaryName = request.PrimaryName;

        //if (request.FirstName != null && request.FirstName != existingEntity.FirstName)
        //{
        //    existingEntity.FirstName = request.FirstName.;
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
