using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Infrastructure.Database;
using NetCore.Tools.Migration.Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using NetCore.Tools.Migration.Common.Interface;

namespace NetCore.Tools.Migration.Tasks;

public class SeedDataTask : IMigrationTask
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly DatabaseContext _databaseContext;
    private readonly ILogger _logger;
    private readonly IDataSeedRunner<IBaseDataSeed> _baseDataSeeds;
    private readonly IDataSeedRunner<ITestDataSeed> _testDataSeeds;
    private readonly string TASK_NAME;

    public SeedDataTask(IServiceScopeFactory scopeFactory, ILogger logger, 
        IDataSeedRunner<IBaseDataSeed> baseDataSeeds,
        IDataSeedRunner<ITestDataSeed> testDataSeeds)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _databaseContext = GetNewDatabaseContext();
        _baseDataSeeds = baseDataSeeds;
        _testDataSeeds= testDataSeeds;
        TASK_NAME = this.GetType().FullName;
    }

    public IEnumerable<Type> Dependencies => new List<Type>()
    {
        typeof(ApplyPendingMigrationTask)
    };

    public async Task ExecuteAsync(Command command)
    {
        _logger.Information($"Start [{TASK_NAME}]");
        if (command.RunSeeds)
        {
            await _baseDataSeeds.RunSeedsAsync();
        }

        if (command.RunSeedsTestData)
        {
            await _testDataSeeds.RunSeedsAsync();
        }

        _logger.Information($"End [{TASK_NAME}]");
    }

    private void RunSeedsFromCsv()
    {
      
    }

    private DatabaseContext GetNewDatabaseContext()
    {
        var dbContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        return dbContext;
    }
}
