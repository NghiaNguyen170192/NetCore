using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record UpdatePersonRequest(Guid Id, string NameConst, string? PrimaryName, int? BirthYear, int? DeathYear) : IRequest<UpdatePersonResponse>;
}