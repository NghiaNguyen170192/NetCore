using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCore.Infrastructure.Database.Dtos;
using NetCore.Infrastructure.Database.Handlers;
using NetCore.Infrastructure.Database.Handlers.Models.Entities.Configuration.Query;
using System;
using System.Threading.Tasks;

namespace NetCore.Api.Controllers
{
    public class ConfigurationsController : AuthorizedBaseController
    {
        private readonly IMediator _mediator;

        public ConfigurationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var response = await _mediator.Send(new QueryAllConfigurationRequest());
            return Ok(response);
        }

        // GET api/values
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(QueryConfigurationRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateConfigurationRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateConfigurationModel model)
        {
            var request = new UpdateConfigurationRequest(id);
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteConfigurationRequest(id));
            return Ok(response);
        }
    }
}
