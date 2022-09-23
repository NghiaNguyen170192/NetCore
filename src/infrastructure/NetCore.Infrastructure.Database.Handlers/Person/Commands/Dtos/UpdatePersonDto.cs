#nullable enable

namespace NetCore.Infrastructure.Database.Handlers.Commands.Dtos;

public record UpdatePersonDto(string NameConst, string? PrimaryName, int? BirthYear, int? DeathYear);
