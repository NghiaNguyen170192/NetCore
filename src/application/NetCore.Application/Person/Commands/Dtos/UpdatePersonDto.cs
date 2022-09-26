#nullable enable

namespace NetCore.Application.Commands.Dtos;

public record UpdatePersonDto(string NameConst, string? PrimaryName, int? BirthYear, int? DeathYear);
