using System;

namespace NetCore.Infrastructure.Handlers
{
    public record QueryPersonResponse(Guid Id, string NameConst, string PrimaryName, int BirthYear, int? DeathYear);
}
