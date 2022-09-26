using MediatR;
using System;

namespace NetCore.Application.Commands;

public record DeletePersonCommand(Guid Id) : IRequest;
