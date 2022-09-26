using CommandLine;
using NetCore.Shared.Extensions;
using NetCore.Migration.Common;
using NetCore.Migration.Common.Interface;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Migration
{
    public class MigrationService
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<IMigrationTask> _migrationTasks;

        public MigrationService(ILogger logger, IEnumerable<IMigrationTask> migrationTasks)
        {
            _logger = logger;
            _migrationTasks = migrationTasks;
        }

        public async Task RunAsync(string[] args)
        {
            _logger.Information($"Start with args: {string.Join(" ", args, 0, args.Length)}");
            await Parser.Default.ParseArguments<Command>(args)
                .WithParsedAsync(RunCommands);

            _logger.Information("End");
        }

        private async Task RunCommands(Command command)
        {
            try
            {
                var sortedMigrationTasks = _migrationTasks.TopologicalSort(x => x.Dependencies).ToList();
                foreach (var migrationTask in sortedMigrationTasks)
                {
                    _logger.Information("-----------------------------------");
                    await migrationTask.ExecuteAsync(command);
                    _logger.Information("-----------------------------------");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
        }

        private Task HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                _logger.Error(error.ToString());    
            }

            return Task.CompletedTask;
        }
    }
}
