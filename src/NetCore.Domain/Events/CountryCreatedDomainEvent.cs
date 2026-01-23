using NetCore.Domain.Messaging;

namespace NetCore.Domain.Events;

/// <summary>
/// Example domain event raised when a country is created.
/// </summary>
public sealed record CountryCreatedDomainEvent(Guid CountryId, string Name) : IDomainEvent;
