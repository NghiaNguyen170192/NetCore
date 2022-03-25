using MediatR;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record CreateConfigurationRequest() : IRequest<CreateConfigurationResponse>;
}
