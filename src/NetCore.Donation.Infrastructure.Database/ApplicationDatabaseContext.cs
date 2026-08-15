#nullable enable
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetCore.Donation.Domain.Entities;
using NetCore.Donation.Domain.SharedKernel;
using NetCore.Donation.Infrastructure.Database.Extensions;

namespace NetCore.Donation.Infrastructure.Database;

public class ApplicationDatabaseContext(
    DbContextOptions<ApplicationDatabaseContext> databaseContextOptions,
    IPublisher? publisher = null)
    : DbContext(databaseContextOptions), IUnitOfWork
{
    private readonly IPublisher? publisher = publisher;

    public DbSet<Country> Countries { get; set; }

    public DbSet<Contact> Contacts { get; set; }

    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    public DbSet<PaymentSchedule> PaymentSchedules { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<Receipt> Receipts { get; set; }

    public DbSet<IdempotencyLog> IdempotencyLogs { get; set; }

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
        AuditableSaveChanges();
        await DispatchDomainEventsAsync(cancellationToken);
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
        entry.Entity.ModifiedDate = DateTime.UtcNow;
        entry.Entity.ModifiedBy = Guid.Empty;

        if (entry.State != EntityState.Added)
        {
            return;
        }

        // Generate ID if not set
        if (entry.Entity.Id == Guid.Empty)
        {
            entry.Entity.Id = Guid.NewGuid();
        }

        entry.Entity.CreatedDate = DateTime.UtcNow;
        entry.Entity.CreatedBy = Guid.Empty;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        // Skip domain event dispatching if publisher is not available (e.g., during migrations)
        if (publisher is null)
        {
            return;
        }

        var domainEntities = ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }
    }
}