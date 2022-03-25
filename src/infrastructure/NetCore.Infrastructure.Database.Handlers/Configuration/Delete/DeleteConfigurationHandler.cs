using MediatR;
using NetCore.Infrastructure.Database.Models.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Database.Handlers
{
    public class DeleteConfigurationHandler : IRequestHandler<DeleteConfigurationRequest>
    {
        private readonly DatabaseContext _databaseContext;

        public DeleteConfigurationHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Unit> Handle(DeleteConfigurationRequest request, CancellationToken cancellationToken)
        {
            var configuration = _databaseContext.Set<Configuration>().Single(x => x.Id == request.Id);
            _databaseContext.Set<Configuration>().Remove(configuration);

            return await Task.FromResult(Unit.Value);
        }
    }
}
