using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using NetCore.Migration.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Migration.Seeds.BaseData;

public class TestGenderSeed : TestDataSeed
{
    private readonly IRepository<Gender> _repository;

    public TestGenderSeed(IRepository<Gender> repository)
    {
        _repository = repository;
    }


    public override IEnumerable<Type> Dependencies => new List<Type>();

    public async override Task SeedAsync()
    {
        var genders = new List<Gender>()
        {
            new Gender
            {
                Name = "Other"
            },
        };

        foreach (var gender in genders)
        {
            var existing = await _repository.Collection
                                          .Select(x => x.Name)
                                          .FirstOrDefaultAsync(x => x == gender.Name);

            if (existing == null)
            {
                await _repository.AddAsync(gender);
            }
        }

        await _repository.SaveChangesAsync();
    }
}