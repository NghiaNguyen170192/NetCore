using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database.Extensions;
using NetCore.Infrastructure.Database.Model;

namespace NetCore.Infrastructure.Database.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions databaseContextOptions)
            : base(databaseContextOptions)
        {
        }

        public DbSet<Genre> Genre { get; set; }

        public DbSet<Person> Person { get; set; }

        public DbSet<PersonProfession> PersonProfession { get; set; }

        public DbSet<PersonTitle> PersonTitle { get; set; }

        public DbSet<Profession> Profession { get; set; }

        public DbSet<Title> Title { get; set; }

        public DbSet<TitleGenre> TitleGenre { get; set; }

        public DbSet<TitleType> TitleType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SetDefaultValueTableName();
        }
    }
}
