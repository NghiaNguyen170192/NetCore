using System.Collections.Generic;

namespace NetCore.Infrastructure.Database.Model
{
    public class Person : Entity
    {
        public string NameConst { get; set; }

        public string PrimaryName { get; set; }
        
        public int BirthYear { get; set; }
        
        public int? DeathYear { get; set; }

        public IList<PersonProfession> PersonProfessions { get; set; }

        public IList<PersonTitle> PersonTitles { get; set; }
    }
}
