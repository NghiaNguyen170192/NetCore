using MediatR;
using NetCore.Infrastructure.Database.Handlers.Queries.Dtos;
using System;

namespace NetCore.Infrastructure.Database.Handlers.Queries;

public record GenderQuery(Guid Id) : IRequest<GenderQueryDto>;
