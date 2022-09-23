using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers.Commands;

public record DeletePersonCommand(Guid Id) : IRequest;
