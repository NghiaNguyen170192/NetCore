using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCore.Application.Queries.Dtos;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using System;

namespace NetCore.Application.Queries;

public class PersonsQueryHandler : IRequestHandler<PersonsQuery, IEnumerable<PersonQueryDto>>
{
    private readonly IRepository<Person> _repository;

    public PersonsQueryHandler(IRepository<Person> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PersonQueryDto>> Handle(PersonsQuery request, CancellationToken cancellationToken)
    {
        return _repository.Collection
            .AsNoTracking()
            .Select(x => new PersonQueryDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                BirthDate = x.BirthDate,
                Website = x.Website,
            })
            .AsEnumerable();
    }
}
