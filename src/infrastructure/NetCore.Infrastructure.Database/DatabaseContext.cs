using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database.Extensions;
using NetCore.Infrastructure.Database.Models.Entities;

namespace NetCore.Infrastructure.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> databaseContextOptions)
            : base(databaseContextOptions)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Configuration> Configurations{ get; set; }
        public DbSet<ConfigurationTransation> ConfigurationTransations { get; set; }
        public DbSet<Language> Languages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SetDefaultValueTableName();
        }
    }
}
