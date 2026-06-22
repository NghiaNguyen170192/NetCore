namespace NetCore.Domain.Messaging;

/// <summary>
/// Defines a handler for a domain event.
/// </summary>
/// <typeparam name="TDomainEvent">The type of domain event being handled.</typeparam>
public interface IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    /// <summary>
    /// Handles the domain event.
    /// </summary>
    /// <param name="notification">The domain event notification.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Handle(TDomainEvent notification, CancellationToken cancellationToken);
}