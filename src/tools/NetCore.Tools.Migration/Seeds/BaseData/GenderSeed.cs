using Microsoft.EntityFrameworkCore;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using NetCore.Tools.Migration.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Tools.Migration.Seeds.BaseData;

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
        var genders = new List<Gender>()
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