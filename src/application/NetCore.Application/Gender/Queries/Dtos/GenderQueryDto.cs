
namespace NetCore.Application.Queries.Dtos;

//public record GenderQueryDto(Guid Id, string Name);

public record GenderQueryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}
