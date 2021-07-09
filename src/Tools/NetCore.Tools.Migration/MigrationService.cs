using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NetCore.Infrastructure.Database.Contexts;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Tools.Migration
{
    public class MigrationService: IHostedService
    {
        private readonly DatabaseContext _databaseContext;
        
        public MigrationService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            RunMigrations();
            RunSeeds();
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
            var numberOfRecords = 10000;
            if(_databaseContext.Activity.Count() == numberOfRecords)
            {
                return;
            }

            for (int i = 0; i < numberOfRecords; i++)
            {
                _databaseContext.Activity.Add(new Infrastructure.Database.Model.Activity());
            }

            _databaseContext.SaveChanges();
        }
    }
}
