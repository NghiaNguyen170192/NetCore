namespace NetCore.Infrastructure.Handlers
{
    public record QueryPersonResponse(int Id, string NameConst, string PrimaryName, int BirthYear, int? DeathYear);
}
