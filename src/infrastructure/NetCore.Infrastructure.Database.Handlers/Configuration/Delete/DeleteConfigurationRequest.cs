using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record DeleteConfigurationRequest(Guid Id) : IRequest;
}
