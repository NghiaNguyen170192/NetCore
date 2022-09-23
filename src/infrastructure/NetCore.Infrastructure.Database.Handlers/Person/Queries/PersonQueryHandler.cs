using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Handlers.Queries.Dtos;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Database.Handlers.Queries;

public class PersonQueryHandler : IRequestHandler<PersonQuery, PersonQueryDto>
{
    private readonly DatabaseContext _databaseContext;

    public PersonQueryHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<PersonQueryDto> Handle(PersonQuery request, CancellationToken cancellationToken)
    {
        var person = _databaseContext
            .Set<Person>()
            .SingleOrDefault(x => x.Id == request.Id);

        var response = MapResponse(person);
        return await Task.FromResult(response);
    }

    private PersonQueryDto MapResponse(Person person)
    {
        return new PersonQueryDto (person.Id, person.FirstName, person.LastName,  person.Email, person.Phone, person.BirthDate, person.Website);

    }
}
