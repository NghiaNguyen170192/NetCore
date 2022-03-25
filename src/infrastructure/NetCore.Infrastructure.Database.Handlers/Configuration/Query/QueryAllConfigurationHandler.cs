using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace NetCore.Infrastructure.Database.Handlers.Models.Entities.Configuration.Query
{
    public class QueryAllConfigurationHandler : IRequestHandler<QueryAllConfigurationRequest, ICollection<QueryConfigurationResponse>>
    {
        private readonly DatabaseContext _databaseContext;

        public QueryAllConfigurationHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ICollection<QueryConfigurationResponse>> Handle(QueryAllConfigurationRequest request, CancellationToken cancellationToken)
        {
            var configurations = _databaseContext
                .Set<Database.Models.Entities.Configuration>()
                .ToList();

            var response = new List<QueryConfigurationResponse>();
            foreach (var configuration in configurations)
            {
                response.Add(MapResponse(configuration));
            }

            return await Task.FromResult(response);
        }

        private QueryConfigurationResponse MapResponse(Database.Models.Entities.Configuration configuration)
        {
            return new QueryConfigurationResponse();
        }
    }
}
