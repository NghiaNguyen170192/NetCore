using NetCore.Infrastructure.Database.Entities;
using System;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record QueryPersonResponse(Guid Id, string FirstName, string LastName, string Email, string Phone, DateTime BirthDate, string Website);
}
