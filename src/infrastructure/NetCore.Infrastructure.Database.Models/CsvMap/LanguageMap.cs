using CsvHelper.Configuration;
using NetCore.Infrastructure.Database.Models.Entities;
using System.Globalization;

namespace NetCore.Infrastructure.Database.Models.CsvMap
{
    public sealed class LanguageMap : ClassMap<Language>
    {
        public LanguageMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Name).Name("name");
            Map(m => m.Alpha2).Name("alpha-2");
            Map(m => m.Alpha3).Name("alpha-3");
            Map(m => m.CountryCode).Name("country-code");
            Map(m => m.Region).Name("region");
            Map(m => m.SubRegion).Name("sub-region");
            Map(m => m.RegionCode).Name("region-code");
            Map(m => m.SubRegionCode).Name("sub-region-code");
        }
    }
}
