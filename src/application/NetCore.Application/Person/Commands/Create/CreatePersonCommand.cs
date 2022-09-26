using MediatR;
using System;

namespace NetCore.Application.Commands;

public record CreatePersonCommand(string FirstName, string LastName, string Email, string Phone, DateTime BirthDate, string Website) : IRequest<Guid>;
