using NetCore.Infrastructure.Database.Entities;
using System.Globalization;
using CsvHelper.Configuration;

namespace NetCore.Application.CsvMap
{
    public sealed class CountryCsvMap : ClassMap<Country>
    {
        public CountryCsvMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Name).Name("name");
            Map(m => m.Alpha2).Name("alpha-2");
            Map(m => m.Alpha3).Name("alpha-3");
            Map(m => m.CountryCode).Name("country-code");
        }
    }
}
