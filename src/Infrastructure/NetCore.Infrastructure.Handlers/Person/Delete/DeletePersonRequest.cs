using MediatR;

namespace NetCore.Infrastructure.Handlers.Person
{
    public record DeletePersonRequest(int Id): IRequest;
}
