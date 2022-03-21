namespace NetCore.Infrastructure.Database.Models.Entities
{
    public class Language : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
