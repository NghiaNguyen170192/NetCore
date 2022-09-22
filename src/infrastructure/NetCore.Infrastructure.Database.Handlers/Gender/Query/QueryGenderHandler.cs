using MediatR;
using NetCore.Infrastructure.Database.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Database.Handlers
{
    public class QueryGenderHandler : IRequestHandler<QueryGenderRequest, QueryGenderResponse>
    {
        private readonly DatabaseContext _databaseContext;

        public QueryGenderHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<QueryGenderResponse> Handle(QueryGenderRequest request, CancellationToken cancellationToken)
        {
            var person = _databaseContext
                .Set<Person>()
                .SingleOrDefault(x => x.Id == request.Id);

            var response = MapResponse(person);
            return await Task.FromResult(response);
        }

        private QueryGenderResponse MapResponse(Person person)
        {
            return new QueryGenderResponse (person.Id, person.FirstName, person.LastName,  person.Email, person.Phone, person.BirthDate, person.Website);

        }
    }
}
