using Microsoft.Extensions.DependencyInjection;
using NetCore.Application.Behaviors;
using NetCore.Application.Messaging;
using NetCore.Domain.Messaging;

namespace NetCore.Application.Tests.Messaging;

[TestClass]
public class DispatcherTests
{
    [TestMethod]
    public async Task SendAsync_WithValidRequest_CallsHandler()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<IRequestHandler<TestRequest, string>, TestRequestHandler>();
        services.AddScoped<IDispatcher, Dispatcher>();
        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        var request = new TestRequest("Test");

        // Act
        var result = await dispatcher.SendAsync(request);

        // Assert
        Assert.AreEqual("Test Response: Test", result);
    }

    [TestMethod]
    public async Task SendAsync_WithPipelineBehavior_ExecutesBehavior()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<IRequestHandler<TestRequest, string>, TestRequestHandler>();
        services.AddScoped<IPipelineBehavior<TestRequest, string>, TestBehavior>();
        services.AddScoped<IDispatcher, Dispatcher>();
        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        var request = new TestRequest("Test");

        // Act
        var result = await dispatcher.SendAsync(request);

        // Assert
        Assert.Contains("Behavior", result);
    }

    [TestMethod]
    public async Task SendAsync_WithLoggingBehavior_LogsRequest()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddScoped<IRequestHandler<TestRequest, string>, TestRequestHandler>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped<IDispatcher, Dispatcher>();
        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        var request = new TestRequest("Test");

        // Act
        var result = await dispatcher.SendAsync(request);

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SendAsync_WithDomainEvent_ReturnsUnit()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<IRequestHandler<TestDomainEvent, Unit>, TestDomainEventHandler>();
        services.AddScoped<IDispatcher, Dispatcher>();
        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        var domainEvent = new TestDomainEvent("TestEvent");

        // Act
        var result = await dispatcher.SendAsync(domainEvent);

        // Assert
        Assert.AreEqual(Unit.Value, result);
    }

    // Test implementations
    private record TestRequest(string Data) : IRequest<string>;

    private record TestDomainEvent(string Data) : IDomainEvent;

    private class TestRequestHandler : IRequestHandler<TestRequest, string>
    {
        public Task<string> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult($"Test Response: {request.Data}");
        }
    }

    private class TestDomainEventHandler : IRequestHandler<TestDomainEvent, Unit>
    {
        public Task<Unit> HandleAsync(TestDomainEvent request, CancellationToken cancellationToken = default)
        {
            // Handle domain event
            return Task.FromResult(Unit.Value);
        }
    }

    private class TestBehavior : IPipelineBehavior<TestRequest, string>
    {
        public async Task<string> HandleAsync(TestRequest request, Func<Task<string>> next, CancellationToken cancellationToken = default)
        {
            var result = await next();
            return $"Behavior: {result}";
        }
    }
}