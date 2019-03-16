using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NetCore.Infrastructure.Models;
using NetCore.Infrastructure.Models.IMDB;
using System.IO;

namespace NetCore.Infrastructure.Migrations.ApplicationDb
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }       
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<TitleBasic> TitleBasics { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<TitleBasicGenre> TitleBasicGenres { get; set; }
        public DbSet<TitleType> TitleTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TitleBasicGenre>()
                .HasKey(t => new { t.TitleBasicId, t.GenreId });


            modelBuilder.Entity<TitleBasicGenre>()
                .HasOne(sc => sc.TitleBasic)
                .WithMany(s => s.TitleBasicGenres)
                .HasForeignKey(sc => sc.TitleBasicId);


            modelBuilder.Entity<TitleBasicGenre>()
                .HasOne(sc => sc.Genre)
                .WithMany(s => s.TitleBasicGenres)
                .HasForeignKey(sc => sc.GenreId);
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();            
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new ApplicationDbContext(builder.Options);
        }
    }
}
