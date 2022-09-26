using MediatR;
using System;

namespace NetCore.Application.Commands;

public record CreateGenderCommand(string Name) : IRequest<Guid>;
