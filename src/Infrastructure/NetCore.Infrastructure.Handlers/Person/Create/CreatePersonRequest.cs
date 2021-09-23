using MediatR;

namespace NetCore.Infrastructure.Handlers
{
    public record CreatePersonRequest(string NameConst, string PrimaryName, int? BirthYear, int? DeathYear) : IRequest<CreatePersonResponse>;
}
