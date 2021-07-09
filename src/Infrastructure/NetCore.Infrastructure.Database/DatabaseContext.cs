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

        public DbSet<Activity> Activity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SetDefaultValueTableName();
        }
    }
}
