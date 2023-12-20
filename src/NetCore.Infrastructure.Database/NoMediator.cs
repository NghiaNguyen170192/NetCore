#nullable enable

using MediatR;

namespace NetCore.Infrastructure.Database;

public class NoMediator : IMediator
{
	public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
	{
		return default!;
	}

	public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
	{
		return default!;
	}

	public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
	{
		return Task.CompletedTask;
	}

	public Task Publish(object notification, CancellationToken cancellationToken = default)
	{
		return Task.CompletedTask;
	}

	public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
	{
		return Task.FromResult<TResponse>(default!);
	}

	public Task<object?> Send(object request, CancellationToken cancellationToken = default)
	{
		return Task.FromResult(default(object));
	}

	public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
	{
		return Task.CompletedTask;
	}
}
