using MediatR;
using System;
using System.Linq;

namespace NetCore.Infrastructure.Handlers
{
    public record QueryPersonRequest(Guid Id, string NameConst, string PrimaryName, int BirthYear, int? DeathYear) : IRequest<IQueryable<QueryPersonResponse>>;
}
