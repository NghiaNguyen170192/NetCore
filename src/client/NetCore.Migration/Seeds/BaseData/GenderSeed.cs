using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using NetCore.Migration.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Migration.Seeds.BaseData;

public class GenderSeed : BaseDataSeed
{
    private readonly IRepository<Gender> _repository;

    public GenderSeed(IRepository<Gender> repository)
    {
        _repository = repository;
    }

    public override IEnumerable<Type> Dependencies => new List<Type>();

    public async override Task SeedAsync()
    {
        var seeds = new List<Gender>()
        {
            new Gender
            {
                Name = "Male"
            },
            new Gender
            {
                Name = "Female"
            }
        };

        foreach (var seed in seeds)
        {
            var existing = await _repository.Collection
                                          .Select(x => x.Name)
                                          .FirstOrDefaultAsync(x => x == seed.Name);

            var existing2 = await _repository.ExistAsync(x => x.Name == seed.Name);

            if (existing == null)
            {
                await _repository.AddAsync(seed);
            }
        }
        
        await _repository.SaveChangesAsync();
    }
}