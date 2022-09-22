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

public class DeleteDatabaseTask : IMigrationTask
{
    private readonly DatabaseContext _databaseContext;
    private readonly ILogger _logger;

    public DeleteDatabaseTask(DatabaseContext databaseContext, ILogger logger)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }

    public IEnumerable<Type> Dependencies => new List<Type>();

    public async Task ExecuteAsync(Command command)
    {
        _logger.Information($"Start [{typeof(DeleteDatabaseTask)}]");
       
        if (!command.RunDeleteDatabase)
        {
            return;
        }

        await _databaseContext.Database.EnsureDeletedAsync();
        _logger.Information($"End [{typeof(DeleteDatabaseTask)}]");
    }
}
