using MediatR;
using System.Linq;

namespace NetCore.Infrastructure.Handlers
{
    public record QueryPersonRequest(int Id, string NameConst, string PrimaryName, int BirthYear, int? DeathYear) : IRequest<IQueryable<QueryPersonResponse>>;
}
