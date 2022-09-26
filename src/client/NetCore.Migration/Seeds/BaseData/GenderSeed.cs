using MediatR;
using NetCore.Application.Commands;
using NetCore.Migration.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Migration.Seeds.BaseData;

public class GenderSeed : BaseDataSeed
{
    private readonly IMediator _mediator;

    public GenderSeed(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override IEnumerable<Type> Dependencies => new List<Type>();

    public async override Task SeedAsync()
    {
        var seeds = new string[]
        {
                "Male",
                "Female"
        };

        foreach (var seed in seeds)
        {
            await _mediator.Send(new CreateGenderCommand(seed));
        }
    }
}