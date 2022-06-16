using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Database.Handlers
{
    public class CreatePersonHandler : IRequestHandler<CreatePersonRequest, CreatePersonResponse>
    {
        private readonly IRepository<Person> _repository;

        public CreatePersonHandler(IRepository<Person> repository)
        {
            _repository = repository;
        }

        public async Task<CreatePersonResponse> Handle(CreatePersonRequest request, CancellationToken cancellationToken)
        {
            if (IsExisted(request))
            {
                //todo: handle exception
            }

            var person = MapPerson(request);
            await _repository.AddAsync(person, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(MapResponse(person));
        }

        private bool IsExisted(CreatePersonRequest request)
        {
            return _repository.Collection.Any(person => person.NameConst == request.NameConst);
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
