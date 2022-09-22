using NetCore.Infrastructure.Database.Commons;

namespace NetCore.Infrastructure.Database.Entities;
public class Country : BaseEntity
{
    public string Name { get; set; }
    public string CountryCode { get; set; }
    public string Alpha2 { get; set; }
    public string Alpha3 { get; set; }
}
