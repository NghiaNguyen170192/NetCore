using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Infrastructure.Database;
using NetCore.Tools.Migration.Common;
using NetCore.Tools.Migration.Common.Interface;
using Serilog;

namespace NetCore.Tools.Migration.Tasks;

public class RunPendingMigrationTask : IMigrationTask
{
    private readonly DatabaseContext _databaseContext;
    private readonly ILogger _logger;

    public RunPendingMigrationTask(DatabaseContext databaseContext, ILogger logger)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }


    public IEnumerable<Type> Dependencies => new List<Type>()
    {
        typeof(DeleteDatabaseTask)
    };

    public async Task ExecuteAsync(Command command)
    {
        _logger.Information($"Start [{typeof(RunPendingMigrationTask)}]");
        if (!command.RunMigration)
        {
            return;
        }

        var pendingMigrations = await _databaseContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            _logger.Information($"Pending migrations:  {pendingMigrations.Count()}");
            await _databaseContext.Database.MigrateAsync();
        }

        _logger.Information($"End [{typeof(RunPendingMigrationTask)}]");
    }
}
