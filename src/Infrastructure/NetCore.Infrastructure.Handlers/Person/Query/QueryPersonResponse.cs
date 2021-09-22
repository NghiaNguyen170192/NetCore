namespace NetCore.Infrastructure.Handlers.Person
{
    public record QueryPersonResponse(int Id, string NameConst, string PrimaryName, int BirthYear, int? DeathYear);
}
