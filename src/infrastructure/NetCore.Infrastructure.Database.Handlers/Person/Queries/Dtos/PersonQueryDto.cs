using System;

namespace NetCore.Infrastructure.Database.Handlers.Queries.Dtos;

public record PersonQueryDto(Guid Id, string FirstName, string LastName, string Email, string Phone, DateTime BirthDate, string Website);
