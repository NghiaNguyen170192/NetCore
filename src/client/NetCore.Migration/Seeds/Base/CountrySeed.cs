using CsvHelper;
using CsvHelper.Configuration;
using MediatR;
using NetCore.Application.Country.Create;
using NetCore.Application.Country.CsvMap;
using NetCore.Migration.Common.Interface;
using System.Globalization;

namespace NetCore.Migration.Seeds.Base;

public class CountrySeed(ISender dispatcher) : IDataSeed
{
    private readonly string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Seeds");

    public IEnumerable<Type> Dependencies => new List<Type>();

    public async Task SeedAsync()
    {
        var input = Path.Combine(basePath, "countries.csv");
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            BadDataFound = null,
        };

        using var stream = new StreamReader(File.OpenRead(input));
        using var csv = new CsvReader(stream, csvConfiguration);
        csv.Context.RegisterClassMap<CountryCsvMap>();

        await csv.ReadAsync();
        csv.ReadHeader();

        while (await csv.ReadAsync())
        {
            await dispatcher.Send(GetCommand(csv));
        }
    }

    private CreateCountryCommand GetCommand(IReaderRow csv)
    {
        return new CreateCountryCommand(
            csv.GetField("name") ?? string.Empty,
            csv.GetField("country-code") ?? string.Empty,
            csv.GetField("alpha-2") ?? string.Empty,
            csv.GetField("alpha-3") ?? string.Empty
        );
    }
}