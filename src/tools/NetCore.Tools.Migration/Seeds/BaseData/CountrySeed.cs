using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using NetCore.Tools.Migration.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetCore.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace NetCore.Tools.Migration.Seeds.BaseData;

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
        //var input = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Seed\languages.csv");
        //var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        //csvConfiguration.MissingFieldFound = null;
        //csvConfiguration.BadDataFound = null;

        ////purposely create new context for bulk insert
        //var dbContext = GetNewDatabaseContext();

        //using (var stream = new StreamReader(input))
        //using (var csv = new CsvReader(stream, csvConfiguration))
        //{
        //    csv.Context.RegisterClassMap<LanguageMap>();

        //    await csv.ReadAsync();
        //    csv.ReadHeader();
        //    while (await csv.ReadAsync())
        //    {
        //        var language = csv.GetRecord<Language>();
        //        var isExisted = await _databaseContext.Set<Language>().AnyAsync(x => x.Alpha2 == language.Alpha2 && x.Alpha3 == language.Alpha3);
        //        if (!isExisted)
        //        {
        //            await _databaseContext.Set<Language>().AddAsync(language);
        //        }
        //    }
        //}

        //await _databaseContext.SaveChangesAsync();
    }

    private DatabaseContext GetNewDatabaseContext()
    {
        var databaseContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        databaseContext.ChangeTracker.AutoDetectChangesEnabled = false;

        return databaseContext;
    }
}