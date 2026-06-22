using MediatR;
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
        Assert.IsInstanceOfType(request, typeof(MediatR.IRequest<string>));
    }

    [TestMethod]
    public void IRequestHandler_CanBeImplemented()
    {
        // Arrange & Act
        var handler = new TestRequestHandler();

        // Assert
        Assert.IsInstanceOfType(handler, typeof(MediatR.IRequestHandler<TestRequest, string>));
    }

    [TestMethod]
    public void IDomainEvent_CanBeImplemented()
    {
        // Arrange & Act
        var domainEvent = new TestDomainEvent("TestData");

        // Assert
        Assert.IsInstanceOfType(domainEvent, typeof(IDomainEvent));
        Assert.IsInstanceOfType(domainEvent, typeof(INotification));
    }

    [TestMethod]
    public void Unit_HasValueInstance()
    {
        // Arrange & Act
        var unit1 = MediatR.Unit.Value;
        var unit2 = MediatR.Unit.Value;

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
        Assert.IsInstanceOfType(behavior, typeof(MediatR.IPipelineBehavior<TestRequest, string>));
    }

    // Test implementations
    private record TestRequest : MediatR.IRequest<string>;

    private record TestDomainEvent(string Data) : IDomainEvent;

    private class TestRequestHandler : MediatR.IRequestHandler<TestRequest, string>
    {
        public Task<string> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult("Test Response");
        }
    }

    private class TestPipelineBehavior : MediatR.IPipelineBehavior<TestRequest, string>
    {
        public Task<string> Handle(TestRequest request, MediatR.RequestHandlerDelegate<string> next, CancellationToken cancellationToken)
        {
            return next();
        }
    }
}