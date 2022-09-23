using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database;
using NetCore.Migration.Common;
using NetCore.Migration.Common.Interface;
using Serilog;

namespace NetCore.Migration.Tasks;

public class ApplyPendingMigrationTask : IMigrationTask
{
    private readonly DatabaseContext _databaseContext;
    private readonly ILogger _logger;
    private readonly string TASK_NAME;

    public ApplyPendingMigrationTask(DatabaseContext databaseContext, ILogger logger)
    {
        _logger = logger;
        _databaseContext = databaseContext;
        TASK_NAME = this.GetType().FullName;
    }

    public IEnumerable<Type> Dependencies => new List<Type>()
    {
        typeof(DeleteDatabaseTask)
    };

    public async Task ExecuteAsync(Command command)
    {
        _logger.Information($"Start: {TASK_NAME}");
        if (!command.RunMigration)
        {
            _logger.Information($"No command for running {TASK_NAME}");
            _logger.Information($"End: {TASK_NAME}]");
            return;
        }

        var pendingMigrations = await _databaseContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            _logger.Information($"Pending migrations: {pendingMigrations.Count()}");
            await _databaseContext.Database.MigrateAsync();
        }

        _logger.Information($"End {TASK_NAME}");
    }
}
