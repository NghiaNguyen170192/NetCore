using NetCore.Migration.Common.Interface;
using Serilog;

namespace NetCore.Migration.Tasks;

public class SeedDataTask : IMigrationTask
{
	private readonly ILogger _logger;
	private readonly IDataSeedRunner _dataSeeds;
	private readonly string _taskName;

	public SeedDataTask(ILogger logger,
		IDataSeedRunner dataSeeds)
	{
		_logger = logger;
		_dataSeeds = dataSeeds;
		_taskName = GetType().FullName ?? string.Empty;
	}

	public IEnumerable<Type> Dependencies => new List<Type>()
	{
		typeof(ApplyPendingMigrationTask)
	};

	public async Task ExecuteAsync(string[] args)
	{
		_logger.Information($"Start [{_taskName}]");
		if (args.Contains("-s")) 
			await _dataSeeds.RunSeedsAsync();

		_logger.Information($"End [{_taskName}]");
	}
}
