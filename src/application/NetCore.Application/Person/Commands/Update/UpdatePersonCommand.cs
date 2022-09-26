#nullable enable

using MediatR;
using System;

namespace NetCore.Application.Commands;

public record UpdatePersonCommand(Guid Id, string NameConst, string? PrimaryName, int? BirthYear, int? DeathYear) : IRequest;