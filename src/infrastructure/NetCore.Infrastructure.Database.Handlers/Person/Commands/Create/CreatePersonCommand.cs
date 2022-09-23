using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers.Commands;

public record CreatePersonCommand(string FirstName, string LastName, string Email, string Phone, DateTime BirthDate, string Website) : IRequest<Guid>;
