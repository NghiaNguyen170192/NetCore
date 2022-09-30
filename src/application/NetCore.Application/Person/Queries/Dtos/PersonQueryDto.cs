#nullable disable
namespace NetCore.Application.Queries.Dtos;


public record PersonQueryDto
{
    public Guid Id { get; init;}
    public string FirstName { get; init;}
    public string LastName { get; init;}
    public string Email { get; init;}
    public string Phone { get; init;}
    public DateTime BirthDate { get; init;}
    public string Website { get; init;}
};
