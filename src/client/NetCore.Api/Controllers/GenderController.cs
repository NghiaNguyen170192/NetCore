using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCore.Infrastructure.Database.Handlers.Commands;
using NetCore.Infrastructure.Database.Handlers.Commands.Dtos;
using NetCore.Infrastructure.Database.Handlers.Queries;
using System;
using System.Threading.Tasks;

namespace NetCore.Api.Controllers;

public class GenderController : AuthorizedBaseController
{
    private readonly IMediator _mediator;

    public GenderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET api/values
    [HttpGet]
    public async Task<ActionResult> Get(Guid id)
    {
        var response = await _mediator.Send(new GenderQuery(id));
        return Ok(response);
    }

    // POST api/values
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateGenderCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] UpdateGenderDto model)
    {
        var request = new UpdateGenderCommand(id, model.Name);
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteGenderCommand(id));
        return Ok(response);
    }
}