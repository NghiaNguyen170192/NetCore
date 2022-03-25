using MediatR;
using NetCore.Infrastructure.Database.Models.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Database.Handlers
{
    public class UpdateConfigurationHandler : IRequestHandler<UpdateConfigurationRequest, UpdateConfigurationResponse>
    {
        private readonly DatabaseContext _databaseContext;

        public UpdateConfigurationHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<UpdateConfigurationResponse> Handle(UpdateConfigurationRequest request, CancellationToken cancellationToken)
        {
            var existingPerson = GetExistingPerson(request);
            if (existingPerson == null)
            {
                //todo: handle exception
                return await Task.FromResult(new UpdateConfigurationResponse(false));
            }

            MapPerson(request, existingPerson);
            await _databaseContext.Set<Configuration>().AddAsync(existingPerson, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(new UpdateConfigurationResponse(true));
        }

        private Configuration GetExistingPerson(UpdateConfigurationRequest request)
        {
            return _databaseContext.Set<Configuration>().SingleOrDefault(x => x.Id == request.Id);
        }

        private void MapPerson(UpdateConfigurationRequest request, Configuration existingPerson)
        {
        }
    }
}
