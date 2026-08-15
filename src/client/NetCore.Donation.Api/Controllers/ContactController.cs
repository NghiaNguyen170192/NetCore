using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using NetCore.Donation.Application.Contact.Create;
using NetCore.Donation.Application.Contact.DTOs;
using NetCore.Donation.Application.Contact.GetContact;
using NetCore.Donation.Application.Contact.QueryContacts;
using NetCore.Donation.Application.Contact.SetActive;
using NetCore.Donation.Application.Contact.Update;
using System.Net;

namespace NetCore.Donation.Api.Controllers;

[Route("~/api/v1/contacts")]
public class ContactController(IMediator mediator) : AuthorizedBaseController
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Create([FromBody] CreateContactCommand request)
    {
        var id = await mediator.Send(request);

        return CreatedAtAction(nameof(GetContact), new { id }, new { id });
    }

    /// <summary>
    /// Return OData query from client
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [EnableQuery(AllowedFunctions = AllowedFunctions.AllFunctions)]
    public async Task<ActionResult<IQueryable<QueryContactDto>>> GetContacts()
    {
        var response = await mediator.Send(new QueryContacts());

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<QueryContactDto>> GetContact(Guid id)
    {
        var response = await mediator.Send(new GetContactQuery(id));

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateContactCommand request)
    {
        if (id != request.Id)
        {
            return BadRequest("The identifier in the route does not match the identifier in the payload.");
        }

        var updated = await mediator.Send(request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPatch("{id:guid}/active")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> SetActive(Guid id, [FromBody] SetContactActiveCommand request)
    {
        if (id != request.Id)
        {
            return BadRequest("The identifier in the route does not match the identifier in the payload.");
        }

        var updated = await mediator.Send(request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}