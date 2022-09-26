using MediatR;
using System;

namespace  NetCore.Application.Commands;

public record DeleteGenderCommand(Guid Id) : IRequest;
