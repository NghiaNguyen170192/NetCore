using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.Infrastructure.Database.Contexts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Tools.Migration
{
    public class MigrationService: IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly DatabaseContext _databaseContext;

        public MigrationService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            RunMigrations();
            RunSeeds();
            RunSeedsBySql();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void RunMigrations()
        {
            var pendingMigrations = _databaseContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                _databaseContext.Database.Migrate();
            }
        }

        private void RunSeeds()
        {
           
        }

        private void RunSeedsBySql()
        {
            
        }

        private void DeleteDatabase()
        {

        }
    }
}
