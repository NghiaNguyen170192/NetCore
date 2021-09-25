using MediatR;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record CreatePersonRequest(string NameConst, string PrimaryName, int? BirthYear, int? DeathYear) : IRequest<CreatePersonResponse>;
}
