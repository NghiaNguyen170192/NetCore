using NetCore.Infrastructure.Database;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Tools.Migration.Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Tools.Migration.Seeds.BaseData;

public class GenderSeed : BaseDataSeed
{
    private readonly DatabaseContext _databaseContext;
    private readonly ILogger _logger;

    public GenderSeed(DatabaseContext databaseContext, ILogger logger)
    {
        _databaseContext = databaseContext;
        _logger = logger;
    }

    public override IEnumerable<Type> Dependencies => new List<Type>();

    public async override Task SeedAsync()
    {
        var genders = new List<Gender>()
        {
            new Gender
            {
                Name = "Male"
            },
            new Gender
            {
                Name = "Female"
            }
        };

        await _databaseContext.AddRangeAsync(genders);
        await _databaseContext.SaveChangesAsync();
    }
}