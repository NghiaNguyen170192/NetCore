using Microsoft.Data.SqlClient;

namespace NetCore.Infrastructure.Database.Tests;

[TestClass]
public class MigrationTest
{
	private readonly ApplicationDatabaseContext _context;
	private readonly IMigrationsAssembly _migrationsAssembly;

	public MigrationTest()
	{
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDatabaseContext>();
		optionsBuilder.UseSqlServer(new SqlConnection(), o => o.MigrationsAssembly("NetCore.Infrastructure.Database"));
		_context = new ApplicationDatabaseContext(optionsBuilder.Options, new NoMediator());
		_migrationsAssembly = _context.GetService<IMigrationsAssembly>();
	}

	[TestMethod]
	public void MigrationsAssemblyModelSnapshotNotNull()
	{
		Assert.IsNotNull(_migrationsAssembly.ModelSnapshot);
		Assert.IsNotNull(_migrationsAssembly.ModelSnapshot?.Model);
	}

	[TestMethod]
	public void ModelSnapshotHasNoDifferencesModel()
	{
		var modelSnapshot = _context.GetService<IModelRuntimeInitializer>().Initialize(_migrationsAssembly.ModelSnapshot?.Model!);
		var sourceModel = modelSnapshot.GetRelationalModel();
		var targetModel = _context.GetService<IDesignTimeModel>().Model.GetRelationalModel();
		var hasDifferences = _context.GetService<IMigrationsModelDiffer>().HasDifferences(sourceModel, targetModel);

		Assert.IsNotNull(sourceModel);
		Assert.IsNotNull(targetModel);
		Assert.IsFalse(hasDifferences);
	}
}