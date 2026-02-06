using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Application.Messaging;
using NetCore.Domain.Entities;
using NetCore.Domain.Events;
using NetCore.Domain.Messaging;

namespace NetCore.Infrastructure.Database.Tests;

[TestClass]
public class ApplicationDatabaseContextTests
{
    [TestMethod]
    public async Task SaveChangesAsync_SetsAuditProperties()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var services = new ServiceCollection();
        services.AddScoped<IDispatcher, Dispatcher>();
        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

        var context = new ApplicationDatabaseContext(options, dispatcher);
        var country = Country.Create("Test", "001", "TS", "TST");

        // Act
        context.Countries.Add(country);
        await context.SaveChangesAsync(CancellationToken.None);

        // Assert
        Assert.AreNotEqual(default(DateTime), country.CreatedDate);
        Assert.AreNotEqual(default(DateTime), country.ModifiedDate);
        Assert.AreEqual(DateTime.UtcNow.Date, country.CreatedDate.Date);
        Assert.AreEqual(DateTime.UtcNow.Date, country.ModifiedDate.Date);
    }

    [TestMethod]
    public async Task SaveChangesAsync_DispatchesDomainEvents()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var eventDispatched = false;
        var services = new ServiceCollection();
        services.AddScoped<IDispatcher, Dispatcher>();
        services.AddScoped<IRequestHandler<CountryCreatedDomainEvent, Unit>, TestEventHandler>(
            _ => new TestEventHandler(() => eventDispatched = true));
        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

        var context = new ApplicationDatabaseContext(options, dispatcher);
        var country = new TestCountryWithEvent("Test", "001", "TS", "TST");

        // Act
        context.Countries.Add(country);
        await context.SaveChangesAsync(CancellationToken.None);

        // Assert
        Assert.IsTrue(eventDispatched);
        Assert.IsEmpty(country.DomainEvents); // Events should be cleared after dispatching
    }

    [TestMethod]
    public async Task SaveChangesAsync_WithoutDispatcher_DoesNotThrow()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDatabaseContext(options, null);
        var country = Country.Create("Test", "001", "TS", "TST");

        // Act
        context.Countries.Add(country);
        await context.SaveChangesAsync(CancellationToken.None);

        // Assert
        Assert.IsNotNull(country);
        Assert.AreNotEqual(Guid.Empty, country.Id);
    }

    [TestMethod]
    public void DbContext_HasCountriesDbSet()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        // Act
        var context = new ApplicationDatabaseContext(options, null);

        // Assert
        Assert.IsNotNull(context.Countries);
    }

    [TestMethod]
    public void DbContext_ImplementsIUnitOfWork()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        // Act
        var context = new ApplicationDatabaseContext(options, null);

        // Assert
        Assert.IsInstanceOfType(context, typeof(Domain.SharedKernel.IUnitOfWork));
    }

    // Test helpers
    private class TestEventHandler : IRequestHandler<CountryCreatedDomainEvent, Unit>
    {
        private readonly Action _onHandle;

        public TestEventHandler(Action onHandle)
        {
            _onHandle = onHandle;
        }

        public Task<Unit> HandleAsync(CountryCreatedDomainEvent request, CancellationToken cancellationToken = default)
        {
            _onHandle();
            return Task.FromResult(Unit.Value);
        }
    }

    private class TestCountryWithEvent : Country
    {
        public TestCountryWithEvent(string name, string countryCode, string alpha2, string alpha3)
            : base(name, countryCode, alpha2, alpha3)
        {
            var addMethod = typeof(Domain.SharedKernel.Entity).GetMethod("AddDomainEvent",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            addMethod?.Invoke(this, new object[] { new CountryCreatedDomainEvent(Id, name) });
        }
    }
}
