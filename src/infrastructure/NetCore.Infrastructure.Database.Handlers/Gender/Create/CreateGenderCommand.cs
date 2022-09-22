using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record CreateGenderCommand(string Name) : IRequest<CreateGenderResponse>;
}
