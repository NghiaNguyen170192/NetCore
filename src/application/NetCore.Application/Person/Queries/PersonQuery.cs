using MediatR;
using NetCore.Application.Queries.Dtos;
using System;

namespace NetCore.Application.Queries;

public record PersonQuery(Guid Id) : IRequest<PersonQueryDto>;
