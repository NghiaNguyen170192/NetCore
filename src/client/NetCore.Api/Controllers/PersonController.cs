using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCore.Infrastructure.Database.Handlers;
using NetCore.Infrastructure.Database.Handlers.DTO;
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
        var response = await _mediator.Send(new QueryPersonRequest(id));
        return Ok(response);
    }

    // POST api/values
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePersonRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] UpdatePersonModel model)
    {
        var request = new UpdatePersonRequest(id, model.NameConst, model.PrimaryName, model.BirthYear, model.DeathYear);
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeletePersonRequest(id));
        return Ok(response);
    }
}