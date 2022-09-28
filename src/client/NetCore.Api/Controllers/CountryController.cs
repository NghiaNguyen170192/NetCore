using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using NetCore.Application.Commands;
using NetCore.Application.Commands.Dtos;
using NetCore.Application.Queries;
using NetCore.Application.Queries.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Api.Controllers;

public class CountryController : AuthorizedBaseController
{
    private readonly IMediator _mediator;

    public CountryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET api/values
    [HttpGet]
    public async Task<ActionResult> Get(Guid id)
    {
        var response = await _mediator.Send(new CountryQuery(id));
        return Ok(response);
    }      

    // POST api/values
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCountryCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] UpdateCountryDto model)
    {
        var request = new UpdateCountryCommand(id, model.Name, model.CountryCode, model.Alpha2, model.Alpha3);
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteCountryCommand(id));
        return Ok(response);
    }
    
    // GET api/values
    [HttpGet]
    [EnableQuery]
    [Route("~/api/v1/countries")]
    public async Task<ActionResult<IEnumerable<CountryQueryDto>>> GetCountries()
    {
        var response = await _mediator.Send(new CountriesQuery());
        return Ok(response);
    }
}