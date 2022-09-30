namespace NetCore.Application.Queries.Dtos;

public record CountryQueryDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? CountryCode { get; init; }
    public string? Alpha2 { get; init; }
    public string? Alpha3 { get; init; }
}