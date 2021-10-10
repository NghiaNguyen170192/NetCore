using MediatR;
using NetCore.Infrastructure.Database.Models.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Database.Handlers
{
    public class QueryPersonHandler : IRequestHandler<QueryPersonRequest, QueryPersonResponse>
    {
        private readonly DatabaseContext _databaseContext;

        public QueryPersonHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<QueryPersonResponse> Handle(QueryPersonRequest request, CancellationToken cancellationToken)
        {
            var person = _databaseContext
                .Set<Person>()
                .SingleOrDefault(x => x.Id == request.Id);

            var response = MapResponse(person);
            return await Task.FromResult(response);
        }

        private QueryPersonResponse MapResponse(Person person)
        {
            return new QueryPersonResponse (person.Id, person.NameConst, person.PrimaryName, person.BirthYear, person.DeathYear);
        }
    }
}
