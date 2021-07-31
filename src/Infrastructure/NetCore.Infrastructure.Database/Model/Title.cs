using System.Collections.Generic;

namespace NetCore.Infrastructure.Database.Model
{
    public class Title : Entity
    {
        public string Tconst { get; set; }
        
        public TitleType TitleType { get; set; }
        
        public string PrimaryTitle { get; set; }
        
        public string OriginalTitle { get; set; }
        
        public bool? IsAdult { get; set; }
        
        public int? StartYear { get; set; }
        
        public int? EndYear { get; set; }
        
        public int? RuntimeMinutes{ get; set; }

        //public IList<TitleGenre> TitleGenres { get; set; }

        //public IList<PersonTitle> PersonTitles { get; set; }
    }
}
