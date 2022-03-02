using CommandLine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Infrastructure.Database;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var input = @"C:\Users\nghia.nguyenquoc.STS\source\customer_db\vng_db\raw-data";
            string[] fileEntries = Directory.GetFiles(input);

            var clockBulkInsert = new Stopwatch();
            clockBulkInsert.Start();
            var taskList = new List<Task>();
            foreach (string fileEntry in fileEntries)
            {
                taskList.Add(ProcessFile(fileEntry));
            }

            await Task.WhenAll(taskList);

            clockBulkInsert.Stop();

            _logger.Information($"Time Elapsed: {clockBulkInsert.ElapsedMilliseconds}");
        }

        private async Task ProcessFile(string fileEntry)
        {
            _logger.Information($"Processing file: {fileEntry}");
            //var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
            //csvConfiguration.MissingFieldFound = null;
            //csvConfiguration.BadDataFound = null;

            //long totalFileRecords = 0;

            ////purposely create new context for bulk insert
            //var dbContext = GetNewDatabaseContext();

            //using (var stream = new StreamReader(fileEntry))
            //using (var csv = new CsvReader(stream, csvConfiguration))
            //{
            //    await csv.ReadAsync();
            //    csv.ReadHeader();
            //    while (await csv.ReadAsync())
            //    {
            //        var customer = csv.GetRecord<Customer>();
            //        await dbContext.AddAsync(customer);
            //        if (totalFileRecords % 1000 == 0)
            //        {
            //            _logger.Information($"Saving batch: {totalFileRecords / 1000}");
            //            await dbContext.SaveChangesAsync();
            //            await dbContext.DisposeAsync();
            //            dbContext = GetNewDatabaseContext();
            //        }

            //        totalFileRecords++;
            //    }
            //}
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

                //if (commands.RunSeeds)
                //{
                //    _logger.Information($"{nameof(RunSeeds)} Start");
                //    await RunSeeds();
                //    _logger.Information($"{nameof(RunSeeds)} End");
                //}

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
