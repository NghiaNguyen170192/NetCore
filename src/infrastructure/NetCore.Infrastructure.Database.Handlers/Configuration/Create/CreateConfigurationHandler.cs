using MediatR;
using NetCore.Infrastructure.Database.Models.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Database.Handlers
{
    public class CreateConfigurationHandler : IRequestHandler<CreateConfigurationRequest, CreateConfigurationResponse>
    {
        private readonly DatabaseContext _databaseContext;

        public CreateConfigurationHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<CreateConfigurationResponse> Handle(CreateConfigurationRequest request, CancellationToken cancellationToken)
        {
            if (IsExisted(request))
            {
                //todo: handle exception
            }

            var configuration = MapConfiguration(request);
            await _databaseContext.Set<Configuration>().AddAsync(configuration, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(MapResponse(configuration));
        }

        private bool IsExisted(CreateConfigurationRequest request)
        {
            return false;
            //return _databaseContext.Set<Models.Entities.Configuration>().Any(x => x.NameConst == request.NameConst);
        }

        private Configuration MapConfiguration(CreateConfigurationRequest request)
        {
            return new Configuration
            {
                
            };
        }

        private CreateConfigurationResponse MapResponse(Configuration configuration)
        {
            return new CreateConfigurationResponse(configuration.Id);
        }
    }
}
