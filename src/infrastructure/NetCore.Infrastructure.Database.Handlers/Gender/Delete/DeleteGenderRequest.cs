using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record DeleteGenderRequest(Guid Id) : IRequest;
}
