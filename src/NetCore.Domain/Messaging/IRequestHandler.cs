namespace NetCore.Domain.Messaging;

/// <summary>
/// Defines a handler for a request.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled.</typeparam>
/// <typeparam name="TResponse">The type of response from the handler.</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles a request.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Response from the request.</returns>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines a handler for a request with no return value.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled.</typeparam>
public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, Unit>
    where TRequest : IRequest<Unit>
{
}
