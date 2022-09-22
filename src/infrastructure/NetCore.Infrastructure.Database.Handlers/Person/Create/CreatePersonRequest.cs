using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers
{
    public record CreatePersonRequest(string FirstName, string LastName, string Email, string Phone, DateTime BirthDate, string Website) : IRequest<CreatePersonResponse>;
}
