using System.Collections.Generic;

namespace NetCore.Infrastructure.Database.Models.Entities
{
    public class Configuration : BaseEntity
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public ICollection<ConfigurationTransation> ConfigurationTranslations { get; set; }
    }
}
