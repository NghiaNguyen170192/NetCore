using MediatR;
using System;

namespace  NetCore.Infrastructure.Database.Handlers.Commands;

public record DeleteGenderCommand(Guid Id) : IRequest;
