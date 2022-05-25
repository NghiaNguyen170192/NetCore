using CommandLine;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Infrastructure.Database;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Tools.Migration
{
    public class MigrationService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger _logger;

        public MigrationService(IServiceScopeFactory scopeFactory, ILogger logger)
        {
            _scopeFactory = scopeFactory;
            _databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
            _logger = logger;
        }

        public async Task RunAsync(string[] args)
        {
            _logger.Information($"Start with args: {  string.Join(" ", args, 0, args.Length)}");

            await Parser.Default.ParseArguments<Options>(args)
                .WithParsedAsync(RunCommands);

            _logger.Information("End");
        }

        private async Task RunMigration()
        {
            var pendingMigrations = await _databaseContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                _logger.Information($"Pending migrations:  { pendingMigrations.Count()}");
                await _databaseContext.Database.MigrateAsync();
            }
        }

        private async Task RunSeeds()
        {
            // Get file name our CSV file
            var input = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Seed\languages.csv");
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
            csvConfiguration.MissingFieldFound = null;
            csvConfiguration.BadDataFound = null;

            //purposely create new context for bulk insert
            //var dbContext = GetNewDatabaseContext();

            //using (var stream = new StreamReader(input))
            //using (var csv = new CsvReader(stream, csvConfiguration))
            //{
            //    csv.Context.RegisterClassMap<LanguageMap>();

            //    await csv.ReadAsync();
            //    csv.ReadHeader();
            //    while (await csv.ReadAsync())
            //    {
            //        var language = csv.GetRecord<Language>();
            //        var isExisted = await _databaseContext.Set<Language>().AnyAsync(x => x.Alpha2 == language.Alpha2 && x.Alpha3 == language.Alpha3);
            //        if (!isExisted)
            //        {
            //            await _databaseContext.Set<Language>().AddAsync(language);
            //        }
            //    }
            //}

            await _databaseContext.SaveChangesAsync();
        }

        private async Task ProcessFile(string fileEntry)
        {
            _logger.Information($"Processing file: {fileEntry}");
        }

        private void RunSeedsTestData()
        {
        }

        private async Task<bool> DeleteDatabase()
        {
            if (!_databaseContext.Database.CanConnect())
            {
                return false;
            }

            return await _databaseContext.Database.EnsureDeletedAsync();
        }

        private async Task RunCommands(Options commands)
        {
            try
            {
                var taskList = new List<Task>();
                if (commands.RunDeleteDatabase)
                {
                    _logger.Information($"{nameof(DeleteDatabase)} Start");
                    await DeleteDatabase();
                    _logger.Information($"{nameof(DeleteDatabase)}  End");
                }

                if (commands.RunMigration)
                {
                    _logger.Information($"{nameof(RunMigration)} Start");
                    await RunMigration();
                    _logger.Information($"{nameof(RunMigration)} End");
                }

                if (commands.RunSeeds)
                {
                    _logger.Information($"{nameof(RunSeeds)} Start");
                    await RunSeeds();
                    _logger.Information($"{nameof(RunSeeds)} End");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
        }

        private async Task HandleParseError(IEnumerable<Error> errors)
        {

        }
        private DatabaseContext GetNewDatabaseContext()
        {
            var dbContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            return dbContext;
        }
    }
}
