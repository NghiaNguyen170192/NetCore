using System;

namespace NetCore.Infrastructure.Database.Models.Entities
{
    public class ConfigurationTransation : BaseEntity
    {
        public string Value { get; set; }
        public string Type { get; set; }

        public Guid ConfigurationId { get; set; }
        public Configuration Configuration { get; set; }

        public Guid LanguageId { get; set; }
        public Language Language { get; set; }
    }
}
