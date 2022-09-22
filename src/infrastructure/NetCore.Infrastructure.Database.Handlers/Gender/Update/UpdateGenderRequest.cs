#nullable enable

using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record UpdateGenderRequest(Guid Id, string Name) : IRequest<UpdateGenderResponse>;
}