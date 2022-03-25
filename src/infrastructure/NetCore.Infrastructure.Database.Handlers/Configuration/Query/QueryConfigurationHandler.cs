using MediatR;
using NetCore.Infrastructure.Database.Models.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Database.Handlers
{
    public class QueryConfigurationHandler : IRequestHandler<QueryConfigurationRequest, QueryConfigurationResponse>
    {
        private readonly DatabaseContext _databaseContext;

        public QueryConfigurationHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<QueryConfigurationResponse> Handle(QueryConfigurationRequest request, CancellationToken cancellationToken)
        {
            var configuration = _databaseContext
                .Set<Configuration>()
                .SingleOrDefault(x => x.Id == request.Id);

            var response = MapResponse(configuration);
            return await Task.FromResult(response);
        }

        private QueryConfigurationResponse MapResponse(Configuration configuration)
        {
            return new QueryConfigurationResponse ();
        }
    }
}
