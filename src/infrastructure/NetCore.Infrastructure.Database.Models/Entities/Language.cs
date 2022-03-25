namespace NetCore.Infrastructure.Database.Models.Entities
{
    public class Language : BaseEntity
    {
        public string Name { get; set; }
        public string Alpha2 { get; set; }
        public string Alpha3 { get; set; }
        public string CountryCode { get; set; }
        public string Region { get; set; }
        public string SubRegion { get; set; }
        public string RegionCode { get; set; }
        public string SubRegionCode { get; set; }
    }
}
