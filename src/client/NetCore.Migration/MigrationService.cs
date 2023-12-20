using NetCore.Migration.Common.Interface;
using NetCore.Migration.Extensions;
using Serilog;

namespace NetCore.Migration;

public class MigrationService
{
	private readonly ILogger _logger;
	private readonly IEnumerable<IMigrationTask> _sortedMigrationTasks;

	public MigrationService(ILogger logger, IEnumerable<IMigrationTask> migrationTasks)
	{
		_logger = logger;
		_sortedMigrationTasks = migrationTasks.TopologicalSort(x => x.Dependencies).ToList();
	}

	public async Task RunAsync(string[] args)
	{
		_logger.Information($"Start with args: {string.Join(" ", args, 0, args.Length)}");
		try
		{
			foreach (var migrationTask in _sortedMigrationTasks)
			{
				_logger.Information("-----------------------------------");
				await migrationTask.ExecuteAsync(args);
				_logger.Information("-----------------------------------");
			}
		}
		catch (Exception exception)
		{
			_logger.Error(exception.Message);
			_logger.Error(exception.StackTrace!);
		}

		_logger.Information("End");
	}
}