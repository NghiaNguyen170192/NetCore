namespace NetCore.Domain.Messaging;

/// <summary>
/// Base interface for all requests (commands and queries) in the CQRS pattern.
/// </summary>
public interface IRequest<out TResponse>
{
}

/// <summary>
/// Base interface for requests that don't return a value.
/// </summary>
public interface IRequest : IRequest<Unit>
{
}

/// <summary>
/// Represents a void response type.
/// </summary>
public readonly struct Unit : IEquatable<Unit>
{
    public static readonly Unit Value = default;

    public bool Equals(Unit other) => true;

    public override bool Equals(object? obj) => obj is Unit;

    public override int GetHashCode() => 0;

    public static bool operator ==(Unit left, Unit right) => true;

    public static bool operator !=(Unit left, Unit right) => false;
}
