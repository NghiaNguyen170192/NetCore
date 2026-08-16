using NetCore.Donation.Application.Donation.DTOs;
using NetCore.Donation.Domain.Entities;
using System.Globalization;
using System.Text.Json;

namespace NetCore.Donation.Application.Donation.QueryDonationFlows;

public static class DonationFlowAssembler
{
    private static readonly HashSet<string> MoneyFlowEvents =
    [
        "DonationCreated",
        "ContactCreated",
        "DonationPaymentMethodCreated",
        "TransactionPending",
        "TransactionSucceeded",
        "TransactionFailed",
        "DonationReceiptGenerated",
        "JournalEntryCreated",
    ];

    public static IReadOnlyList<QueryDonationFlowDto> Assemble(IEnumerable<OutboxMessage> messages)
    {
        var parsed = messages
            .Select(TryParse)
            .Where(item => item is not null)
            .Cast<ParsedOutboxEvent>()
            .OrderBy(item => item.OccurredAtUtc)
            .ToList();

        var scheduleByTransaction = new Dictionary<Guid, Guid>();
        var scheduleByCorrelation = new Dictionary<string, Guid>(StringComparer.Ordinal);

        foreach (var item in parsed)
        {
            if (item.PaymentScheduleId is not { } scheduleId)
            {
                continue;
            }

            if (item.TransactionId is { } transactionId)
            {
                scheduleByTransaction[transactionId] = scheduleId;
            }

            if (!string.IsNullOrWhiteSpace(item.CorrelationId))
            {
                scheduleByCorrelation[item.CorrelationId] = scheduleId;
            }
        }

        foreach (var item in parsed)
        {
            if (item.PaymentScheduleId is not null)
            {
                continue;
            }

            if (item.TransactionId is { } transactionId &&
                scheduleByTransaction.TryGetValue(transactionId, out var fromTransaction))
            {
                item.PaymentScheduleId = fromTransaction;
                continue;
            }

            if (!string.IsNullOrWhiteSpace(item.CorrelationId) &&
                scheduleByCorrelation.TryGetValue(item.CorrelationId, out var fromCorrelation))
            {
                item.PaymentScheduleId = fromCorrelation;
            }
        }

        var groups = new Dictionary<string, List<ParsedOutboxEvent>>(StringComparer.Ordinal);
        foreach (var item in parsed)
        {
            var key = item.PaymentScheduleId?.ToString("N")
                ?? item.TransactionId?.ToString("N")
                ?? item.CorrelationId;
            if (string.IsNullOrWhiteSpace(key))
            {
                continue;
            }

            if (!groups.TryGetValue(key, out var bucket))
            {
                bucket = [];
                groups[key] = bucket;
            }

            bucket.Add(item);
        }

        return groups.Values
            .Select(BuildFlow)
            .Where(flow => flow is not null)
            .Cast<QueryDonationFlowDto>()
            .OrderByDescending(flow => flow.LastEventAtUtc)
            .ToList();
    }

    private static QueryDonationFlowDto? BuildFlow(List<ParsedOutboxEvent> events)
    {
        if (!events.Any(item => item.EventName is
            "DonationCreated" or
            "TransactionPending" or
            "TransactionSucceeded" or
            "TransactionFailed" or
            "JournalEntryCreated" or
            "DonationReceiptGenerated"))
        {
            return null;
        }

        var ordered = events.OrderBy(item => item.OccurredAtUtc).ToList();
        var failed = ordered.Any(item => item.EventName == "TransactionFailed");
        var succeeded = ordered.Any(item => item.EventName == "TransactionSucceeded");
        var pending = ordered.Any(item => item.EventName == "TransactionPending");
        var status = failed ? "Failed" : succeeded ? "Succeeded" : pending ? "Pending" : "InProgress";

        var scheduleId = ordered.Select(item => item.PaymentScheduleId).FirstOrDefault(id => id is not null);
        var transactionId = ordered.Select(item => item.TransactionId).FirstOrDefault(id => id is not null);
        var flowId = scheduleId ?? transactionId ?? ordered[0].MessageId;

        return new QueryDonationFlowDto
        {
            Id = flowId,
            PaymentScheduleId = scheduleId,
            ContactId = ordered.Select(item => item.ContactId).FirstOrDefault(id => id is not null),
            PaymentMethodId = ordered.Select(item => item.PaymentMethodId).FirstOrDefault(id => id is not null),
            TransactionId = transactionId,
            JournalId = ordered.Select(item => item.JournalId).FirstOrDefault(id => id is not null),
            ReceiptId = ordered.Select(item => item.ReceiptId).FirstOrDefault(id => id is not null),
            Amount = ordered.Select(item => item.Amount).LastOrDefault(amount => amount is not null),
            Status = status,
            MoneyPath = BuildMoneyPath(
                scheduleId is not null,
                transactionId is not null,
                failed,
                succeeded,
                pending,
                ordered.Any(item => item.JournalId is not null),
                ordered.Any(item => item.ReceiptId is not null)),
            StartedAtUtc = ordered[0].OccurredAtUtc,
            LastEventAtUtc = ordered[^1].OccurredAtUtc,
            Steps = ordered.Select(item => new QueryDonationFlowStepDto
            {
                EventName = item.EventName,
                OccurredAtUtc = item.OccurredAtUtc,
                ProcessedAtUtc = item.ProcessedAtUtc,
                Summary = item.Summary,
                CorrelationId = item.CorrelationId,
            }).ToList(),
        };
    }

    private static string BuildMoneyPath(
        bool hasSchedule,
        bool hasTransaction,
        bool failed,
        bool succeeded,
        bool pending,
        bool hasJournal,
        bool hasReceipt)
    {
        var parts = new List<string> { "Donor" };
        if (hasSchedule)
        {
            parts.Add("Payment schedule");
        }

        if (hasTransaction)
        {
            parts.Add(failed ? "Transaction (failed)" : succeeded ? "Transaction (posted)" : pending ? "Transaction (pending)" : "Transaction");
        }

        if (hasJournal)
        {
            parts.Add("Journal");
        }

        if (hasReceipt)
        {
            parts.Add("Receipt");
        }

        return string.Join(" → ", parts);
    }

    private static ParsedOutboxEvent? TryParse(OutboxMessage message)
    {
        var eventName = ToEventName(message.MessageType);
        if (!MoneyFlowEvents.Contains(eventName))
        {
            return null;
        }

        using var document = JsonDocument.Parse(message.Payload);
        var root = document.RootElement;
        var amount = GetDecimal(root, "amount");
        var parsed = new ParsedOutboxEvent
        {
            MessageId = message.Id,
            EventName = eventName,
            CorrelationId = message.CorrelationId,
            OccurredAtUtc = message.OccurredAtUtc,
            ProcessedAtUtc = message.ProcessedAtUtc,
            PaymentScheduleId = GetGuid(root, "paymentScheduleId"),
            ContactId = GetGuid(root, "contactId"),
            PaymentMethodId = GetGuid(root, "paymentMethodId"),
            TransactionId = GetGuid(root, "transactionId"),
            JournalId = GetGuid(root, "journalId"),
            ReceiptId = GetGuid(root, "receiptId"),
            Amount = amount,
            Summary = BuildSummary(eventName, amount, root),
        };

        return parsed;
    }

    private static string BuildSummary(string eventName, decimal? amount, JsonElement root)
    {
        var amountText = amount is { } value
            ? value.ToString("0.00", CultureInfo.InvariantCulture)
            : null;

        return eventName switch
        {
            "ContactCreated" => $"Donor profile {GetString(root, "email")}".Trim(),
            "DonationPaymentMethodCreated" => $"Payment method {GetString(root, "displayName")}".Trim(),
            "DonationCreated" => amountText is null ? "Donation pledged" : $"Pledged {amountText} on a payment schedule",
            "TransactionPending" => amountText is null ? "Transaction pending" : $"Transaction pending for {amountText}",
            "TransactionSucceeded" => amountText is null ? "Transaction posted" : $"Posted {amountText} to the ledger path",
            "TransactionFailed" => amountText is null ? "Transaction failed" : $"Payment failed for {amountText}",
            "JournalEntryCreated" => "Journal line recorded for the posted gift",
            "DonationReceiptGenerated" => "Digital receipt stored for the donor",
            _ => eventName,
        };
    }

    private static string ToEventName(string messageType)
    {
        var typeName = messageType.Split(',')[0].Trim();
        var shortName = typeName.Contains('.', StringComparison.Ordinal)
            ? typeName[(typeName.LastIndexOf('.') + 1)..]
            : typeName;
        return shortName.EndsWith("DomainEvent", StringComparison.Ordinal)
            ? shortName[..^"DomainEvent".Length]
            : shortName;
    }

    private static Guid? GetGuid(JsonElement element, string name)
    {
        if (!element.TryGetProperty(name, out var property) ||
            property.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return null;
        }

        if (property.ValueKind == JsonValueKind.String && Guid.TryParse(property.GetString(), out var parsed))
        {
            return parsed;
        }

        return property.TryGetGuid(out var guid) ? guid : null;
    }

    private static decimal? GetDecimal(JsonElement element, string name)
    {
        if (!element.TryGetProperty(name, out var property) ||
            property.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return null;
        }

        return property.TryGetDecimal(out var value) ? value : null;
    }

    private static string GetString(JsonElement element, string name)
    {
        return element.TryGetProperty(name, out var property) && property.ValueKind == JsonValueKind.String
            ? property.GetString() ?? string.Empty
            : string.Empty;
    }

    private sealed class ParsedOutboxEvent
    {
        public Guid MessageId { get; init; }

        public string EventName { get; init; } = string.Empty;

        public string CorrelationId { get; init; } = string.Empty;

        public DateTime OccurredAtUtc { get; init; }

        public DateTime? ProcessedAtUtc { get; init; }

        public Guid? PaymentScheduleId { get; set; }

        public Guid? ContactId { get; init; }

        public Guid? PaymentMethodId { get; init; }

        public Guid? TransactionId { get; init; }

        public Guid? JournalId { get; init; }

        public Guid? ReceiptId { get; init; }

        public decimal? Amount { get; init; }

        public string Summary { get; init; } = string.Empty;
    }
}
