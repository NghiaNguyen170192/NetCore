using Microsoft.EntityFrameworkCore;

namespace NetCore.Infrastructure.Database.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions databaseContextOptions)
            : base(databaseContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
