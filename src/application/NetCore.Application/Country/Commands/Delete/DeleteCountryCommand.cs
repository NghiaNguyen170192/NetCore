using MediatR;
using System;

namespace  NetCore.Application.Commands;

public record DeleteCountryCommand(Guid Id) : IRequest;
