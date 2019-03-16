using NetCore.Infrastructure.Crawler;
using NetCore.Infrastructure.Migrations.ApplicationDb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace NetCore.Infrastructure.Models.IMDB
{
    public class TitleBasicMapper: IProcessFile
    {
        private HashSet<string> _genres = new HashSet<string>();

        private void ProcessGenres(string genresStr)
        {
            var genreItems = genresStr.Split(',');
            var genres = genreItems.Where(x => !_genres.Contains(x) && x.Equals("\\N") 
                            && !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x));
            foreach (var genre in genres)
            {
                _genres.Add(genre);
            }
        }

        public void ProcessFIle(string filePath)
        {
            using (StreamReader sw = new StreamReader(filePath))
            {                string line = sw.ReadLine();
                string[] items = line.Split('\t');
                int index = 0;

                TitleBasicCSV titleBasic = new TitleBasicCSV
                {
                    Tconst = items[index++],
                    TitleType = items[index++],
                    PrimaryTitle = items[index++],
                    OriginalTitle = items[index++],
                    IsAdult = items[index++],
                    StartYear = items[index++],
                    EndYear = items[index++],
                    RuntimeMinutes = items[index++],
                    Genres = items[index],
                };

                ProcessGenres(titleBasic.Genres);
            }
        }
    }

    public class TitleBasicCSV
    {
        public string Tconst { get; set; }
        public string TitleType { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public string IsAdult { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string RuntimeMinutes { get; set; }
        public string Genres { get; set; }
    }

    public class TitleBasic
    {
        public TitleBasic()
        {
            TitleBasicGenres = new HashSet<TitleBasicGenre>();
        }

        [Key]
        public int Id { get; set; }

        public TitleType TitleType { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string RuntimeMinutes { get; set; }

        public virtual ICollection<TitleBasicGenre> TitleBasicGenres { get; set; }
    }

    public class TitleBasicGenre
    {
        public int TitleBasicId { get; set; }
        public TitleBasic TitleBasic { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }

    public class Genre
    {
        public Genre()
        {
            TitleBasicGenres = new HashSet<TitleBasicGenre>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }       
        public virtual ICollection<TitleBasicGenre> TitleBasicGenres { get; set; }
    }

    public class TitleType
    {
        public TitleType()
        {
            TitleBasics = new HashSet<TitleBasic>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }        
        public ICollection<TitleBasic> TitleBasics { get; set; }
    }
}
