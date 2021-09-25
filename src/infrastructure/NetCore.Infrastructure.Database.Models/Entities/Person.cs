namespace NetCore.Infrastructure.Database.Models.Entities
{
    public class Person : BaseEntity
    {
        public string NameConst { get; set; }

        public string PrimaryName { get; set; }
        
        public int BirthYear { get; set; }
        
        public int? DeathYear { get; set; }
    }
}
