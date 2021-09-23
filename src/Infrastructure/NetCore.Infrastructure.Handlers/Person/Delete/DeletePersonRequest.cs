using MediatR;

namespace NetCore.Infrastructure.Handlers
{
    public record DeletePersonRequest(int Id): IRequest;
}
