using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record QueryGenderRequest(Guid Id) : IRequest<QueryGenderResponse>;
}
