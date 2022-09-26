using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Application.Commands;

public class CreatePersonHandler : IRequestHandler<CreatePersonCommand, Guid>
{
    private readonly IRepository<Person> _repository;

    public CreatePersonHandler(IRepository<Person> repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        if (await Exist(request))
        {
            throw new NotImplementedException();
        }

        var entity = Map(request);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return entity.Id;
    }

    private async Task<bool> Exist(CreatePersonCommand request)
    {
        return await _repository.ExistAsync(entity => entity.FirstName == request.FirstName && entity.LastName == request.LastName && entity.Email == request.Email);
    }

    private Person Map(CreatePersonCommand request)
    {
        return new Person
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            BirthDate = request.BirthDate,
            Website =  request.Website,
        };
    }
}
