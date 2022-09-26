using NetCore.Infrastructure.Database.Entities;
using NetCore.Migration.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using NetCore.Application.CsvMap;
using MediatR;
using NetCore.Application.Commands;

namespace NetCore.Migration.Seeds.BaseData;

public class CountrySeed : BaseDataSeed
{
    private readonly IMediator _mediator;
    public CountrySeed(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override IEnumerable<Type> Dependencies => new List<Type>();

    public async override Task SeedAsync()
    {
        // Get file name our CSV file
        var input = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Seed\country.csv");
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            BadDataFound = null
        };

        using (var stream = new StreamReader(input))
        using (var csv = new CsvReader(stream, csvConfiguration))
        {
            csv.Context.RegisterClassMap<CountryCsvMap>();

            await csv.ReadAsync();
            csv.ReadHeader();
            while (await csv.ReadAsync())
            {
                var countrySeed = csv.GetRecord<Country>();
                await _mediator.Send(new CreateCountryCommand(countrySeed.Name, countrySeed.CountryCode, countrySeed.Alpha2, countrySeed.Alpha3));
            }
        }
    }
}