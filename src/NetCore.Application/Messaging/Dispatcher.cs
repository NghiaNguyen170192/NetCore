using Microsoft.Extensions.DependencyInjection;
using NetCore.Domain.Messaging;

namespace NetCore.Application.Messaging;

/// <summary>
/// Default implementation of the CQRS dispatcher.
/// </summary>
public sealed class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        var responseType = typeof(TResponse);

        // Get the handler type
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

        // Resolve the handler
        var handler = _serviceProvider.GetRequiredService(handlerType);

        // Get all pipeline behaviors for the specific request/response type
        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
        var behaviors = _serviceProvider.GetServices(behaviorType).Reverse().ToList();

        // Build the pipeline
        async Task<TResponse> Handler()
        {
            var handleMethod = handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.HandleAsync))
                ?? throw new InvalidOperationException($"Handler method not found for {handlerType.Name}");

            var result = handleMethod.Invoke(handler, new object[] { request, cancellationToken });
            if (result is Task<TResponse> task)
            {
                return await task;
            }

            throw new InvalidOperationException($"Handler did not return a Task<{responseType.Name}>");
        }

        // Execute the pipeline
        Func<Task<TResponse>> pipeline = Handler;
        foreach (var behaviorObj in behaviors)
        {
            var currentBehavior = behaviorObj;
            var next = pipeline;
            pipeline = () =>
            {
                var handleAsyncMethod = behaviorType.GetMethod(nameof(IPipelineBehavior<IRequest<TResponse>, TResponse>.HandleAsync))
                    ?? throw new InvalidOperationException($"HandleAsync method not found on {behaviorType.Name}");

                var result = handleAsyncMethod.Invoke(currentBehavior, new object[] { request, next, cancellationToken });
                if (result is Task<TResponse> task)
                {
                    return task;
                }

                throw new InvalidOperationException($"Behavior did not return a Task<{responseType.Name}>");
            };
        }

        return await pipeline();
    }
}