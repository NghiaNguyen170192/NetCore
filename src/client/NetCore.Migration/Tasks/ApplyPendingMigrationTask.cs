using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database;
using NetCore.Migration.Common.Interface;
using Serilog;

namespace NetCore.Migration.Tasks;

public class ApplyPendingMigrationTask : IMigrationTask
{
	private readonly ApplicationDatabaseContext _context;
	private readonly ILogger _logger;
	private readonly string _taskName;

	public ApplyPendingMigrationTask(ApplicationDatabaseContext applicationDatabaseContext, ILogger logger)
	{
		_logger = logger;
		_context = applicationDatabaseContext;
		_taskName = GetType().FullName ?? string.Empty;
	}

	public IEnumerable<Type> Dependencies => new List<Type>()
	{
		typeof(DeleteDatabaseTask)
	};

	public async Task ExecuteAsync(string[] args)
	{
		_logger.Information($"Start: {_taskName}");
		if (!args.Contains("-m"))
		{
			_logger.Information($"No command for running {_taskName}");
			_logger.Information($"End: {_taskName}]");
			return;
		}

		var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
		if (!pendingMigrations.Any())
		{
			_logger.Information($"No command for running {_taskName}");
			_logger.Information($"End: {_taskName}]");
			return;
		}

		_logger.Information("Applying Migration");
		await _context.Database.MigrateAsync();

		_logger.Information($"End {_taskName}");
	}
}
