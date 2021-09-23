using MediatR;
using NetCore.Infrastructure.Database.Contexts;
using NetCore.Infrastructure.Database.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Handlers
{
    public class QueryPersonHandler : IRequestHandler<QueryPersonRequest, IQueryable<QueryPersonResponse>>
    {
        private readonly DatabaseContext _databaseContext;

        public QueryPersonHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IQueryable<QueryPersonResponse>> Handle(QueryPersonRequest request, CancellationToken cancellationToken)
        {
            var response = _databaseContext
                .Set<Person>()
                .Select(person => MapResponse(person))
                .AsQueryable(); 
            
            return await Task.FromResult(response);
        }

        private QueryPersonResponse MapResponse(Person person)
        {
            return new QueryPersonResponse (person.Id, person.NameConst, person.PrimaryName, person.BirthYear, person.DeathYear);
        }
    }
}
