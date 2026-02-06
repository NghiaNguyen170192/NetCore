using NetCore.Domain.Messaging;

namespace NetCore.Domain.Tests.Messaging;

[TestClass]
public class DispatcherInterfaceTests
{
    [TestMethod]
    public void IRequest_CanBeImplemented()
    {
        // Arrange & Act
        var request = new TestRequest();

        // Assert
        Assert.IsInstanceOfType(request, typeof(IRequest<string>));
    }

    [TestMethod]
    public void IRequestHandler_CanBeImplemented()
    {
        // Arrange & Act
        var handler = new TestRequestHandler();

        // Assert
        Assert.IsInstanceOfType(handler, typeof(IRequestHandler<TestRequest, string>));
    }

    [TestMethod]
    public void IDomainEvent_CanBeImplemented()
    {
        // Arrange & Act
        var domainEvent = new TestDomainEvent("TestData");

        // Assert
        Assert.IsInstanceOfType(domainEvent, typeof(IDomainEvent));
        Assert.IsInstanceOfType(domainEvent, typeof(IRequest<Unit>));
    }

    [TestMethod]
    public void Unit_HasValueInstance()
    {
        // Arrange & Act
        var unit1 = Unit.Value;
        var unit2 = Unit.Value;

        // Assert
        Assert.AreEqual(unit1, unit2);
        Assert.IsTrue(unit1 == unit2);
        Assert.IsTrue(unit1.Equals(unit2));
    }

    [TestMethod]
    public void IPipelineBehavior_CanBeImplemented()
    {
        // Arrange & Act
        var behavior = new TestPipelineBehavior();

        // Assert
        Assert.IsInstanceOfType(behavior, typeof(IPipelineBehavior<TestRequest, string>));
    }

    // Test implementations
    private record TestRequest : IRequest<string>;

    private record TestDomainEvent(string Data) : IDomainEvent;

    private class TestRequestHandler : IRequestHandler<TestRequest, string>
    {
        public Task<string> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult("Test Response");
        }
    }

    private class TestPipelineBehavior : IPipelineBehavior<TestRequest, string>
    {
        public Task<string> HandleAsync(TestRequest request, Func<Task<string>> next, CancellationToken cancellationToken = default)
        {
            return next();
        }
    }
}