using Microsoft.Extensions.Logging;
using NetCore.Domain.Events;
using NetCore.Domain.Messaging;

namespace NetCore.Application.Country.Events;

/// <summary>
/// Example domain event handler for CountryCreatedDomainEvent.
/// </summary>
public class CountryCreatedDomainEventHandler : IDomainEventHandler<CountryCreatedDomainEvent>
{
    private readonly ILogger<CountryCreatedDomainEventHandler> _logger;

    public CountryCreatedDomainEventHandler(ILogger<CountryCreatedDomainEventHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<Unit> HandleAsync(CountryCreatedDomainEvent request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Domain Event: Country '{CountryName}' with ID '{CountryId}' was created",
            request.Name,
            request.CountryId);

        // Add additional domain logic here (e.g., send notifications, update related aggregates, etc.)

        return Task.FromResult(Unit.Value);
    }
}
