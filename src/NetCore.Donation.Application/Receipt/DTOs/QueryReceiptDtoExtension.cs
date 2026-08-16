namespace NetCore.Donation.Application.Receipt.DTOs;

public static class QueryReceiptDtoExtension
{
    public static IQueryable<QueryReceiptDto> ToQueryDto(this IQueryable<Domain.Entities.Receipt> receipts)
    {
        return receipts.Select(receipt => new QueryReceiptDto
        {
            Id = receipt.Id,
            ContactId = receipt.ContactId,
            TransactionId = receipt.TransactionId,
            PaymentScheduleId = receipt.PaymentScheduleId,
            DocumentObjectKey = receipt.DocumentObjectKey,
            DocumentFileName = receipt.DocumentFileName,
            DocumentContentType = receipt.DocumentContentType,
            DocumentGeneratedAtUtc = receipt.DocumentGeneratedAtUtc,
            DocumentSizeBytes = receipt.DocumentSizeBytes,
            HasDocument = receipt.DocumentObjectKey != null && receipt.DocumentObjectKey != string.Empty,
        });
    }
}
