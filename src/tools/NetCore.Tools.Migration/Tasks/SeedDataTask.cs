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
using NetCore.Shared.Extensions;
using System.Linq;
using NetCore.Tools.Migration.Common.Interface;

namespace NetCore.Tools.Migration.Tasks;

public class SeedDataTask : IMigrationTask
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly DatabaseContext _databaseContext;
    private readonly ILogger _logger;
    private readonly IDataSeedRunner<IBaseDataSeed> _baseDataSeeds;
    private readonly IDataSeedRunner<ITestDataSeed> _testDataSeeds;

    public SeedDataTask(IServiceScopeFactory scopeFactory, ILogger logger, 
        IDataSeedRunner<IBaseDataSeed> baseDataSeeds,
        IDataSeedRunner<ITestDataSeed> testDataSeeds)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _databaseContext = GetNewDatabaseContext();
        _baseDataSeeds = baseDataSeeds;
        _testDataSeeds= testDataSeeds;
    }

    public IEnumerable<Type> Dependencies => new List<Type>()
    {
        typeof(RunPendingMigrationTask)
    };

    public async Task ExecuteAsync(Command command)
    {
        _logger.Information($"Start [{typeof(SeedDataTask)}]");
        if (command.RunSeeds)
        {
            await _baseDataSeeds.RunSeedsAsync();
        }

        if (command.RunSeedsTestData)
        {
            await _testDataSeeds.RunSeedsAsync();
        }
    }

    private void RunSeedsFromCsv()
    {
        // Get file name our CSV file
        var input = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Seed\languages.csv");
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        csvConfiguration.MissingFieldFound = null;
        csvConfiguration.BadDataFound = null;

        //purposely create new context for bulk insert
        var dbContext = GetNewDatabaseContext();

        using (var stream = new StreamReader(input))
        using (var csv = new CsvReader(stream, csvConfiguration))
        {
            //csv.Context.RegisterClassMap<LanguageMap>();

            //await csv.ReadAsync();
            //csv.ReadHeader();
            //while (await csv.ReadAsync())
            //{
            //    var language = csv.GetRecord<Language>();
            //    var isExisted = await _databaseContext.Set<Language>().AnyAsync(x => x.Alpha2 == language.Alpha2 && x.Alpha3 == language.Alpha3);
            //    if (!isExisted)
            //    {
            //        await _databaseContext.Set<Language>().AddAsync(language);
            //    }
            //}
        }

        //await _databaseContext.SaveChangesAsync();
    }

    private DatabaseContext GetNewDatabaseContext()
    {
        var dbContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        return dbContext;
    }
}
