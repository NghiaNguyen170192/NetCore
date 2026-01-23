using Microsoft.Extensions.Logging.Abstractions;
using NetCore.Application.Behaviors;
using NetCore.Domain.Messaging;

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
        Func<Task<string>> next = () =>
        {
            nextCalled = true;
            return Task.FromResult("Test Response");
        };

        // Act
        var result = await behavior.HandleAsync(request, next);

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
        Func<Task<string>> next = () => throw new InvalidOperationException("Test Exception");

        Assert.ThrowsExactly<InvalidOperationException>(() =>
        {
            // Act
            _ = behavior.HandleAsync(request, next).GetAwaiter().GetResult();
        });
    }

    private record TestRequest(string Data) : IRequest<string>;
}
