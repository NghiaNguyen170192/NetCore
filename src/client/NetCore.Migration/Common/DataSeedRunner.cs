using NetCore.Migration.Common.Interface;
using NetCore.Migration.Extensions;
using Serilog;

namespace NetCore.Migration.Common;

public class DataSeedRunner<T> : IDataSeedRunner
		where T : IDataSeed
{
	private readonly IEnumerable<T> _sortedSeeds;
	private readonly ILogger _logger;

	public DataSeedRunner(IEnumerable<T> seeds, ILogger logger)
	{
		_sortedSeeds = seeds.TopologicalSort(x => x.Dependencies).ToList();
		_logger = logger;
	}

	public async Task RunSeedsAsync()
	{
		var seedName = "";

		try
		{
			foreach (var seed in _sortedSeeds)
			{
				seedName = seed.GetType().Name;
				await seed.SeedAsync();
			}
		}
		catch (Exception ex)
		{
			_logger.Error(seedName);
			_logger.Error(ex.Message);
			_logger.Error(ex.StackTrace!);
			throw;
		}
	}
}
