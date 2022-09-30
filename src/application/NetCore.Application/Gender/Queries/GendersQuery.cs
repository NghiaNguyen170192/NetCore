using MediatR;
using NetCore.Application.Queries.Dtos;

namespace NetCore.Application.Queries;
public record GendersQuery : IRequest<IEnumerable<GenderQueryDto>>;