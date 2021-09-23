using MediatR;
using System;

namespace NetCore.Infrastructure.Handlers
{
    public record DeletePersonRequest(Guid Id) : IRequest;
}
