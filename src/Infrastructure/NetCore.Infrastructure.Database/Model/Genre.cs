using System.Collections.Generic;

namespace NetCore.Infrastructure.Database.Model
{
    public class Genre : Entity
    {
        public string Name { get; set; }

        public IList<TitleGenre> TitleGenres { get; set; }
    }
}
