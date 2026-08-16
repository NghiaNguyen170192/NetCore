using MediatR;
using NetCore.Donation.Application.Outbox.DTOs;
using NetCore.Donation.Domain.IRepositories;

namespace NetCore.Donation.Application.Outbox.QueryOutboxMessages;

public class QueryOutboxMessagesHandler(IOutboxMessageRepository outboxMessageRepository)
    : IRequestHandler<QueryOutboxMessages, IReadOnlyList<QueryOutboxMessageDto>>
{
    public async Task<IReadOnlyList<QueryOutboxMessageDto>> Handle(
        QueryOutboxMessages request,
        CancellationToken cancellationToken)
    {
        var messages = await outboxMessageRepository.FindByTraceAsync(
            request.CorrelationId,
            request.IdempotencyKey,
            cancellationToken);

        return messages
            .Select(message => new QueryOutboxMessageDto
            {
                Id = message.Id,
                MessageType = message.MessageType,
                Payload = message.Payload,
                CorrelationId = message.CorrelationId,
                IdempotencyKey = message.IdempotencyKey,
                OccurredAtUtc = message.OccurredAtUtc,
                ProcessedAtUtc = message.ProcessedAtUtc,
                AttemptCount = message.AttemptCount,
                LastError = message.LastError,
            })
            .ToList();
    }
}
