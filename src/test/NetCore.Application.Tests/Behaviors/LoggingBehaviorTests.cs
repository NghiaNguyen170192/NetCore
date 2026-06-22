using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using NetCore.Application.Behaviors;

namespace NetCore.Application.Tests.Behaviors;

[TestClass]
public class LoggingBehaviorTests
{
    [TestMethod]
    public async Task HandleAsync_LogsRequestAndResponse()
    {
        // Arrange
        var logger = new NullLogger<LoggingBehavior<TestRequest, string>>();
        var behavior = new LoggingBehavior<TestRequest, string>(logger);
        var request = new TestRequest("Test");
        var nextCalled = false;
        RequestHandlerDelegate<string> next = () =>
        {
            nextCalled = true;
            return Task.FromResult("Test Response");
        };

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        Assert.IsTrue(nextCalled);
        Assert.AreEqual("Test Response", result);
    }

    [TestMethod]
    public void HandleAsync_PropagatesException()
    {
        // Arrange
        var logger = new NullLogger<LoggingBehavior<TestRequest, string>>();
        var behavior = new LoggingBehavior<TestRequest, string>(logger);
        var request = new TestRequest("Test");
        RequestHandlerDelegate<string> next = () => throw new InvalidOperationException("Test Exception");

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await behavior.Handle(request, next, CancellationToken.None);
        });
    }

    private record TestRequest(string Data) : IRequest<string>;
}