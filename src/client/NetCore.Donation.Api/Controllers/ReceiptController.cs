using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using NetCore.Donation.Application.Receipt.Create;
using NetCore.Donation.Application.Receipt.Delete;
using NetCore.Donation.Application.Receipt.DTOs;
using NetCore.Donation.Application.Receipt.GetReceipt;
using NetCore.Donation.Application.Receipt.QueryReceipts;
using NetCore.Donation.Application.Receipt.Update;
using System.Net;

namespace NetCore.Donation.Api.Controllers;

[Route("~/api/v1/receipts")]
public class ReceiptController(IMediator mediator) : AuthorizedBaseController
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Create([FromBody] CreateReceiptCommand request)
    {
        var id = await mediator.Send(request);

        return CreatedAtAction(nameof(GetReceipt), new { id }, new { id });
    }

    /// <summary>
    /// Return OData query from client
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [EnableQuery(AllowedFunctions = AllowedFunctions.AllFunctions)]
    public async Task<ActionResult<IQueryable<QueryReceiptDto>>> GetReceipts([FromQuery] Guid? contactId)
    {
        var response = await mediator.Send(new QueryReceipts(contactId));

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<QueryReceiptDto>> GetReceipt(Guid id)
    {
        var response = await mediator.Send(new GetReceiptQuery(id));

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
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateReceiptCommand request)
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

    [HttpDelete("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await mediator.Send(new DeleteReceiptCommand(id));

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}