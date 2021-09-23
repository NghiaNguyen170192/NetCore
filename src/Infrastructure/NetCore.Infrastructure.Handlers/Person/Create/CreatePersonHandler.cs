using MediatR;
using NetCore.Infrastructure.Database.Contexts;
using NetCore.Infrastructure.Database.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Handlers
{
    public class CreatePersonHandler : IRequestHandler<CreatePersonRequest, CreatePersonResponse>
    {
        private readonly DatabaseContext _databaseContext;

        public CreatePersonHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<CreatePersonResponse> Handle(CreatePersonRequest request, CancellationToken cancellationToken)
        {
            if (IsExisted(request))
            {
                //todo: handle exception
            }

            var person = MapPerson(request);
            await _databaseContext.Set<Person>().AddAsync(person);
            await _databaseContext.SaveChangesAsync();

            return await Task.FromResult(MapResponse(person));
        }

        private bool IsExisted(CreatePersonRequest request)
        {
            return _databaseContext.Set<Person>().Any(x => x.NameConst == request.NameConst);
        }

        private Person MapPerson(CreatePersonRequest request)
        {
            return new Person
            {
                BirthYear = request.BirthYear ?? 0,
                DeathYear = request.DeathYear,
                NameConst = request.NameConst,
                PrimaryName = request.PrimaryName
            };
        }

        private CreatePersonResponse MapResponse(Person person)
        {
            return new CreatePersonResponse(person.Id);
        }
    }
}
