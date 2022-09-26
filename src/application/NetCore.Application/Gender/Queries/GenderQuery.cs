using MediatR;
using NetCore.Application.Queries.Dtos;
using System;

namespace NetCore.Application.Queries;

public record GenderQuery(Guid Id) : IRequest<GenderQueryDto>;
