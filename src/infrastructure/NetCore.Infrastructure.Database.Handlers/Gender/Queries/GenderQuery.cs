using MediatR;
using System;

namespace NetCore.Infrastructure.Database.Handlers.Queries;

public record GenderQuery(Guid Id) : IRequest<QueryGenderResponse>;
