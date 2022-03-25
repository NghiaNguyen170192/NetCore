using MediatR;
using System.Collections.Generic;

namespace NetCore.Infrastructure.Database.Handlers.Models.Entities.Configuration.Query
{
    public record QueryAllConfigurationRequest() : IRequest<ICollection<QueryConfigurationResponse>>;
}
