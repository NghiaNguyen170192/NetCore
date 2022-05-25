using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Extensions;

namespace NetCore.Infrastructure.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> databaseContextOptions)
            : base(databaseContextOptions)
        {
        }

        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SetDefaultValueTableName();
        }
    }
}
