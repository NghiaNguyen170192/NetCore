using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetCore.Infrastructure.Database;
using NetCore.Migration.Common;
using NetCore.Migration.Common.Interface;
using Serilog;

namespace NetCore.Migration.Tasks;

public class DeleteDatabaseTask : IMigrationTask
{
    private readonly DatabaseContext _databaseContext;
    private readonly ILogger _logger;
    private readonly string TASK_NAME;

    public DeleteDatabaseTask(DatabaseContext databaseContext, ILogger logger)
    {
        _logger = logger;
        _databaseContext = databaseContext;
        TASK_NAME = this.GetType().FullName;
    }

    public IEnumerable<Type> Dependencies => new List<Type>();

    public async Task ExecuteAsync(Command command)
    {
        _logger.Information($"Start {TASK_NAME}");

        if (!command.RunDeleteDatabase)
        {
            _logger.Information($"No command for running {TASK_NAME}");
            _logger.Information($"End {TASK_NAME}");
            return;
        }

        if(!await _databaseContext.Database.CanConnectAsync())
        {
            _logger.Information($"Cannot connect to database.");
            _logger.Information($"End {TASK_NAME}");
            return;
        }

        _logger.Information($"Deleting Database");
        await _databaseContext.Database.EnsureDeletedAsync();
        _logger.Information($"End [{TASK_NAME}]");
    }
}
