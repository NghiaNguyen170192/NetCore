#nullable enable

namespace NetCore.Infrastructure.Database.Handlers.DTO
{
    public record UpdatePersonModel(string NameConst, string? PrimaryName, int? BirthYear, int? DeathYear);
}
