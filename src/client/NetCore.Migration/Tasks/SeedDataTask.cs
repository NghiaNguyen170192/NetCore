using NetCore.Migration.Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetCore.Migration.Common.Interface;

namespace NetCore.Migration.Tasks;

public class SeedDataTask : IMigrationTask
{
    private readonly ILogger _logger;
    private readonly IDataSeedRunner<IBaseDataSeed> _baseDataSeeds;
    private readonly IDataSeedRunner<ITestDataSeed> _testDataSeeds;
    private readonly string TASK_NAME;

    public SeedDataTask(ILogger logger, 
        IDataSeedRunner<IBaseDataSeed> baseDataSeeds,
        IDataSeedRunner<ITestDataSeed> testDataSeeds)
    {
        _logger = logger;
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
}
