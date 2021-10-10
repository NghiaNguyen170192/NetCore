using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record QueryPersonRequest(Guid Id) : IRequest<QueryPersonResponse>;
}
