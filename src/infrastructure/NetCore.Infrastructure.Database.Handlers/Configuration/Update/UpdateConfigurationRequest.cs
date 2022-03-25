using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record UpdateConfigurationRequest(Guid Id) : IRequest<UpdateConfigurationResponse>;
}