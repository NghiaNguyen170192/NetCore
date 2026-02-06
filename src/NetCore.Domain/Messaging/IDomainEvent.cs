namespace NetCore.Domain.Messaging;

/// <summary>
/// Marker interface for domain events that don't return a value.
/// Domain events represent something that happened in the domain that you want other parts of the same domain to be aware of.
/// </summary>
public interface IDomainEvent : IRequest
{
}