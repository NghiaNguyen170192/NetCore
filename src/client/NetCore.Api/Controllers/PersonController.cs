using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCore.Infrastructure.Database.Handlers;
using NetCore.Infrastructure.Database.Handlers.Commands;
using NetCore.Infrastructure.Database.Handlers.Commands.Dtos;
using NetCore.Infrastructure.Database.Handlers.Queries;
using System;
using System.Threading.Tasks;

namespace NetCore.Api.Controllers;

public class PersonController : AuthorizedBaseController
{
    private readonly IMediator _mediator;

    public PersonController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET api/values
    [HttpGet]
    public async Task<ActionResult> Get(Guid id)
    {
        var response = await _mediator.Send(new PersonQuery(id));
        return Ok(response);
    }

    // POST api/values
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePersonCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] UpdatePersonDto model)
    {
        var request = new UpdatePersonCommand(id, model.NameConst, model.PrimaryName, model.BirthYear, model.DeathYear);
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeletePersonCommand(id));
        return Ok(response);
    }
}