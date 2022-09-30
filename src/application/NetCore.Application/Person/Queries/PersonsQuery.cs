using MediatR;
using NetCore.Application.Queries.Dtos;

namespace NetCore.Application.Queries;
public record PersonsQuery : IRequest<IEnumerable<PersonQueryDto>>;