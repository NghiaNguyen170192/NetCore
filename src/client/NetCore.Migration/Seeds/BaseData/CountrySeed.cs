using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using NetCore.Migration.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetCore.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using NetCore.Application.CsvMap;
using Microsoft.EntityFrameworkCore;

namespace NetCore.Migration.Seeds.BaseData;

public class CountrySeed : BaseDataSeed
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IRepository<Country> _repository;

    public CountrySeed(IRepository<Country> repository, IServiceScopeFactory scopeFactory)
    {
        _repository = repository;
        _scopeFactory = scopeFactory;
    }

    public override IEnumerable<Type> Dependencies => new List<Type>();

    public async override Task SeedAsync()
    {
        // Get file name our CSV file
        var input = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Seed\country.csv");
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        csvConfiguration.MissingFieldFound = null;
        csvConfiguration.BadDataFound = null;

        //purposely create new context for bulk insert
        var dbContext = GetNewDatabaseContext();

        using (var stream = new StreamReader(input))
        using (var csv = new CsvReader(stream, csvConfiguration))
        {
            csv.Context.RegisterClassMap<CountryCsvMap>();

            await csv.ReadAsync();
            csv.ReadHeader();
            while (await csv.ReadAsync())
            {
                var countrySeed = csv.GetRecord<Country>();
                var isExisted = await _repository.Collection.AnyAsync(x => x.Alpha2 == countrySeed.Alpha2 && x.Alpha3 == countrySeed.Alpha3);
                if (!isExisted)
                {
                    await _repository.AddAsync(countrySeed);
                }
            }
        }

        await _repository.SaveChangesAsync();
    }

    private DatabaseContext GetNewDatabaseContext()
    {
        var databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        databaseContext.ChangeTracker.AutoDetectChangesEnabled = false;

        return databaseContext;
    }
}