using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database.Extensions;
using NetCore.Infrastructure.Database.Models.Entities;

namespace NetCore.Infrastructure.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions databaseContextOptions)
            : base(databaseContextOptions)
        {

        }

        public DbSet<Person> Person { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SetDefaultValueTableName();
        }
    }
}
