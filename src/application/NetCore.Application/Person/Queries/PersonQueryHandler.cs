using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Application.Queries.Dtos;
using NetCore.Infrastructure.Database.Repositories;

namespace NetCore.Application.Queries;

public class PersonQueryHandler : IRequestHandler<PersonQuery, PersonQueryDto>
{
    private readonly IRepository<Person> _repository;

    public PersonQueryHandler(IRepository<Person> repository)
    {
        _repository = repository;
    }

    public async Task<PersonQueryDto> Handle(PersonQuery request, CancellationToken cancellationToken)
    {
        var person = await _repository.GetByIdAsync(request.Id);
        var response = MapResponse(person);
        return await Task.FromResult(response);
    }

    private PersonQueryDto MapResponse(Person person)
    {
        return new PersonQueryDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Email = person.Email,
            BirthDate = person.BirthDate,
            Website = person.Website,
        };
    }
}
