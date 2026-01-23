using NetCore.Infrastructure.Database;
using NetCore.Migration.Common.Interface;
using Serilog;

namespace NetCore.Migration.Tasks;

public class DeleteDatabaseTask : IMigrationTask
{
	private readonly ApplicationDatabaseContext _context;
	private readonly ILogger _logger;
	private readonly string _taskName;

	public DeleteDatabaseTask(ApplicationDatabaseContext applicationDatabaseContext, ILogger logger)
	{
		_logger = logger;
		_context = applicationDatabaseContext;
		_taskName = GetType().FullName ?? string.Empty;
	}

	public IEnumerable<Type> Dependencies => new List<Type>();

	public async Task ExecuteAsync(string[] args)
	{
		_logger.Information($"Start {_taskName}");

		if (!args.Contains("-d"))
		{
			_logger.Information($"No command for running {_taskName}");
			_logger.Information($"End {_taskName}");
			return;
		}

		if (!await _context.Database.CanConnectAsync())
		{
			_logger.Information("Cannot connect to database.");
			_logger.Information($"End {_taskName}");
			return;
		}

		_logger.Information("Deleting Database");
		await _context.Database.EnsureDeletedAsync();
		_logger.Information($"End [{_taskName}]");
	}
}
