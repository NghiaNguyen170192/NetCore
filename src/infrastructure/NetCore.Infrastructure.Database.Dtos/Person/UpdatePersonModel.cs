namespace NetCore.Infrastructure.Database.Dtos
{
    public record UpdatePersonModel(string NameConst, string? PrimaryName, int? BirthYear, int? DeathYear);
}
