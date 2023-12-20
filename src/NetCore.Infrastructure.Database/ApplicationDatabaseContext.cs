using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetCore.Domain.Entities;
using NetCore.Domain.SharedKernel;
using NetCore.Infrastructure.Database.Extensions;

namespace NetCore.Infrastructure.Database;

public class ApplicationDatabaseContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;
    public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> databaseContextOptions, IMediator mediator)
        : base(databaseContextOptions)
    {
        _mediator = mediator;
    }

    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        modelBuilder.SetDefaultValueTableName();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDatabaseContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        AuditableSaveChanges();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _mediator.DispatchDomainEventsAsync(this);
        AuditableSaveChanges();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AuditableSaveChanges()
    {
        var entries = ChangeTracker
            .Entries<Entity>()
            .Where(entity => entity.State is not EntityState.Unchanged);

        foreach (var entry in entries)
        {
            AppendAuditableProperties(entry);
        }
    }

    private static void AppendAuditableProperties(EntityEntry<Entity> entry)
    {
        entry.Entity.ModifiedDate = DateTime.Now;
        entry.Entity.ModifiedBy = Guid.Empty;

        if (entry.State != EntityState.Added) return;

        entry.Entity.CreatedDate = DateTime.Now;
        entry.Entity.CreatedBy = Guid.Empty;
    }
}