using CommandLine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCore.Tools.Migration
{
    public class MigrationService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly DatabaseContext _databaseContext;

        public MigrationService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        }

        public void Run(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunCommands)
                .WithNotParsed(HandleParseError);
        }

        private void RunMigration()
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

        private void RunSeedsTestData()
        {
        }

        private void DeleteDatabase()
        {
            Console.WriteLine($"Delete database Start");
            _databaseContext.Database.EnsureDeleted();
            Console.WriteLine($"Delete database End");
        }

        private void RunCommands(Options commands)
        {
            try
            {
                if (commands.RunDeleteDatabase)
                {
                    DeleteDatabase();
                }

                if (commands.RunMigration)
                {
                    RunMigration();
                }

                if (commands.RunSeeds)
                {
                    RunSeeds();
                }

                if (commands.RunSeedsTestData)
                {
                    RunSeedsTestData();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
        }

        private void HandleParseError(IEnumerable<Error> errors)
        {

        }
    }
}
