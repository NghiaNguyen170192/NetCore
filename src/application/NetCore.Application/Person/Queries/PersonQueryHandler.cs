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
        return new PersonQueryDto (person.Id, person.FirstName, person.LastName,  person.Email, person.Phone, person.BirthDate, person.Website);
    }
}
