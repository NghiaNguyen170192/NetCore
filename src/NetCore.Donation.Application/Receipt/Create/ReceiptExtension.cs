namespace NetCore.Donation.Application.Receipt.Create;

public static class ReceiptExtension
{
    public static Domain.Entities.Receipt ToDbEntity(this CreateReceiptCommand request)
    {
        return Domain.Entities.Receipt.Create(request.ContactId, request.TransactionId);
    }
}