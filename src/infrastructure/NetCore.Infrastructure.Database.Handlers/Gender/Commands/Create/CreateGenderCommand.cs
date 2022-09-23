using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers.Commands;

public record CreateGenderCommand(string Name) : IRequest<Guid>;
