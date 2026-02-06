using Microsoft.Extensions.Logging;
using NetCore.Domain.Events;
using NetCore.Domain.Messaging;

namespace NetCore.Application.Country.Events;

/// <summary>
/// Example domain event handler for CountryCreatedDomainEvent.
/// </summary>
public class CountryCreatedDomainEventHandler : IDomainEventHandler<CountryCreatedDomainEvent>
{
    private readonly ILogger<CountryCreatedDomainEventHandler> logger;

    public CountryCreatedDomainEventHandler(ILogger<CountryCreatedDomainEventHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<Unit> HandleAsync(CountryCreatedDomainEvent request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Domain Event: Country '{request.Name}' with ID '{request.CountryId}' was created");

        // Add additional domain logic here (e.g., send notifications, update related aggregates, etc.)
        return Task.FromResult(Unit.Value);
    }
}