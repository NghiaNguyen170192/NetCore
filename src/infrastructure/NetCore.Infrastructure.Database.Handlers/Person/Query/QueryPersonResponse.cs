using System;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record QueryPersonResponse(Guid Id, string NameConst, string PrimaryName, int BirthYear, int? DeathYear);
}
