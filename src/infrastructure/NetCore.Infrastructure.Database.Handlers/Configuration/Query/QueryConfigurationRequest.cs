using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record QueryConfigurationRequest(Guid Id) : IRequest<QueryConfigurationResponse>;
}
