namespace NetCore.Domain.Messaging;

/// <summary>
/// Defines a handler for a domain event.
/// </summary>
/// <typeparam name="TDomainEvent">The type of domain event being handled.</typeparam>
public interface IDomainEventHandler<in TDomainEvent> : IRequestHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
}