using NetCore.Domain.Events;
using NetCore.Domain.Messaging;
using Microsoft.Extensions.Logging.Abstractions;
using NetCore.Application.Country.Events;

namespace NetCore.Application.Tests.Country.Events;

[TestClass]
public class CountryCreatedDomainEventHandlerTests
{
    [TestMethod]
    public async Task HandleAsync_WithValidEvent_LogsInformation()
    {
        // Arrange
        var logger = new NullLogger<CountryCreatedDomainEventHandler>();
        var handler = new CountryCreatedDomainEventHandler(logger);
        var countryId = Guid.NewGuid();
        var domainEvent = new CountryCreatedDomainEvent(countryId, "United States");

        // Act
        var result = await handler.HandleAsync(domainEvent, CancellationToken.None);

        // Assert
        Assert.AreEqual(Unit.Value, result);
    }

    [TestMethod]
    public void Handler_ImplementsIDomainEventHandler()
    {
        // Arrange
        var logger = new NullLogger<CountryCreatedDomainEventHandler>();

        // Act
        var handler = new CountryCreatedDomainEventHandler(logger);

        // Assert
        Assert.IsInstanceOfType(handler, typeof(IDomainEventHandler<CountryCreatedDomainEvent>));
    }
}