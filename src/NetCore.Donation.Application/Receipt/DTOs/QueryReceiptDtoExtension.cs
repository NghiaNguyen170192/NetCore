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
        });
    }
}