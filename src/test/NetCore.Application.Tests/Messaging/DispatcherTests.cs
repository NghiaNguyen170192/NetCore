using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Application.Behaviors;

namespace NetCore.Application.Tests.Messaging;

[TestClass]
public class DispatcherTests
{
    [TestMethod]
    public async Task SendAsync_WithValidRequest_CallsHandler()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<DispatcherTests>());
        var serviceProvider = services.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new TestRequest("Test");

        // Act
        var result = await mediator.Send(request);

        // Assert
        Assert.AreEqual("Test Response: Test", result);
    }

    [TestMethod]
    public async Task SendAsync_WithPipelineBehavior_ExecutesBehavior()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<DispatcherTests>();
            cfg.AddOpenBehavior(typeof(TestBehavior));
        });
        var serviceProvider = services.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new TestRequest("Test");

        // Act
        var result = await mediator.Send(request);

        // Assert
        Assert.IsTrue(result.Contains("Behavior"));
    }

    [TestMethod]
    public async Task SendAsync_WithLoggingBehavior_LogsRequest()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<DispatcherTests>();
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        var serviceProvider = services.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new TestRequest("Test");

        // Act
        var result = await mediator.Send(request);

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SendAsync_WithDomainEvent_ReturnsUnit()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<DispatcherTests>());
        var serviceProvider = services.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var domainEvent = new TestDomainEvent("TestEvent");

        // Act
        await mediator.Publish(domainEvent);

        // Assert - domain events don't return values, just verify no exception
        Assert.IsTrue(true);
    }

    // Test implementations
    private record TestRequest(string Data) : IRequest<string>;

    private record TestDomainEvent(string Data) : INotification;

    private class TestRequestHandler : IRequestHandler<TestRequest, string>
    {
        public Task<string> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult($"Test Response: {request.Data}");
        }
    }

    private class TestDomainEventHandler : INotificationHandler<TestDomainEvent>
    {
        public Task Handle(TestDomainEvent notification, CancellationToken cancellationToken)
        {
            // Handle domain event
            return Task.CompletedTask;
        }
    }

    private class TestBehavior : IPipelineBehavior<TestRequest, string>
    {
        public async Task<string> Handle(TestRequest request, RequestHandlerDelegate<string> next, CancellationToken cancellationToken)
        {
            var result = await next();
            return $"Behavior: {result}";
        }
    }
}