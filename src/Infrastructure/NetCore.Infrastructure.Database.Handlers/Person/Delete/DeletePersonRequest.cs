using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record DeletePersonRequest(Guid Id) : IRequest;
}
