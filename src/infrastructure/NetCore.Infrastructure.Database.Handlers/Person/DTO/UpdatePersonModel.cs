#nullable enable

namespace NetCore.Infrastructure.Database.Handlers.Dtos
{
    public record UpdatePersonModel(string NameConst, string? PrimaryName, int? BirthYear, int? DeathYear);
}
