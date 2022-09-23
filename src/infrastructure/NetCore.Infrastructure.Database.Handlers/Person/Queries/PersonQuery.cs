using MediatR;
using NetCore.Infrastructure.Database.Handlers.Queries.Dtos;
using System;

namespace NetCore.Infrastructure.Database.Handlers.Queries;

public record PersonQuery(Guid Id) : IRequest<PersonQueryDto>;
