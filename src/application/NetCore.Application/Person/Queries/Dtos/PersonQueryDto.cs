using System;

namespace NetCore.Application.Queries.Dtos;

public record PersonQueryDto(Guid Id, string FirstName, string LastName, string Email, string Phone, DateTime BirthDate, string Website);
