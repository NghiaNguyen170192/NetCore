using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database.Extensions;
using NetCore.Infrastructure.Database.Models.Entities;
using NetCore.Infrastructure.Models.Identity;

namespace NetCore.Infrastructure.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions databaseContextOptions)
            : base(databaseContextOptions)
        {

        }

        #region DbSet
        public DbSet<Person> Person { get; set; }

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SetDefaultValueTableName();
        }
    }
}
