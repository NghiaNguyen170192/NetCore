#nullable enable

using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers.Commands;

public record UpdateGenderCommand(Guid Id, string Name) : IRequest;