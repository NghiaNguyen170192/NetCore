namespace NetCore.Domain.Messaging;

/// <summary>
/// Dispatcher interface for sending requests through the CQRS pipeline.
/// </summary>
public interface IDispatcher
{
    /// <summary>
    /// Sends a request to its handler.
    /// </summary>
    /// <typeparam name="TResponse">Response type.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the response.</returns>
    Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default);
}
