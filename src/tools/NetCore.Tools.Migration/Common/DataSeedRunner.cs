using NetCore.Shared.Extensions;
using NetCore.Tools.Migration.Common.Interface;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Tools.Migration.Common;

public class DataSeedRunner<T> : IDataSeedRunner<T>
        where T : IDataSeed
{
    private readonly IEnumerable<T> _seeds;
    private readonly ILogger _logger;

    public DataSeedRunner(IEnumerable<T> seeds, ILogger logger)
    {
        _seeds = seeds;
        _logger = logger;
    }

    public async Task RunSeedsAsync()
    {
        var sortedSeeds = _seeds.TopologicalSort(x => x.Dependencies).ToList();
        var seedName = "";

        try
        {
            foreach (var seed in sortedSeeds)
            {
                seedName = seed.GetType().Name;
                await seed.SeedAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            _logger.Error(ex.StackTrace);
            throw;
        }
    }
}
