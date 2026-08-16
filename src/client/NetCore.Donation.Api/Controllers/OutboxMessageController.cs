using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCore.Donation.Application.Outbox.DTOs;
using NetCore.Donation.Application.Outbox.QueryOutboxMessages;
using System.Net;

namespace NetCore.Donation.Api.Controllers;

[Route("~/api/v1/outbox-messages")]
public class OutboxMessageController(IMediator mediator) : AuthorizedBaseController
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<QueryOutboxMessageDto>>> GetOutboxMessages(
        [FromQuery] string? correlationId,
        [FromQuery] string? idempotencyKey)
    {
        var response = await mediator.Send(new QueryOutboxMessages(correlationId, idempotencyKey));
        return Ok(response);
    }
}
