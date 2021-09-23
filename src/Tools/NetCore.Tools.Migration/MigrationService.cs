using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Infrastructure.Database.Contexts;
using System;
using System.Linq;

namespace NetCore.Tools.Migration
{
    public class MigrationService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly DatabaseContext _databaseContext;

        //public MigrationService(IServiceScopeFactory scopeFactory)
        //{
        //    _scopeFactory = scopeFactory;
        //    _databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        //}

        public MigrationService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void Run(string[] args)
        {
            RunMigrations();
            RunSeeds();
            RunSeedsBySql();
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
            try
            {
                Console.WriteLine($"Delete database Start");
                _databaseContext.Database.EnsureDeleted();
                Console.WriteLine($"Delete database End");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
        }
    }
}
