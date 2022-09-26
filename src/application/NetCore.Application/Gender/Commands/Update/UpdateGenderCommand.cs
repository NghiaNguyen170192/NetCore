#nullable enable

using MediatR;
using System;

namespace NetCore.Application.Commands;

public record UpdateGenderCommand(Guid Id, string Name) : IRequest;