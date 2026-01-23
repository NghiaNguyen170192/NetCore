namespace NetCore.Domain.Messaging;

/// <summary>
/// Defines a behavior pipeline that runs before and after the request handler.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled.</typeparam>
/// <typeparam name="TResponse">The type of response from the handler.</typeparam>
public interface IPipelineBehavior<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Pipeline handler. Perform any additional behavior and await the next delegate as necessary.
    /// </summary>
    /// <param name="request">Incoming request.</param>
    /// <param name="next">Awaitable delegate for the next action in the pipeline.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Awaitable task returning the response.</returns>
    Task<TResponse> HandleAsync(
        TRequest request,
        Func<Task<TResponse>> next,
        CancellationToken cancellationToken = default);
}
