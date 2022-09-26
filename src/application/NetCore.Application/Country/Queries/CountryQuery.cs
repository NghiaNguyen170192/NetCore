using MediatR;
using NetCore.Application.Queries.Dtos;

namespace NetCore.Application.Queries;

public record CountryQuery(Guid Id) : IRequest<CountryQueryDto>;
