namespace NetCore.Donation.Application.Receipt.Update;

public static class UpdateReceiptExtension
{
    public static void UpdateEntity(this UpdateReceiptCommand request, Domain.Entities.Receipt receipt)
    {
        if (request.TransactionId is { } transactionId)
        {
            receipt.AssignTransaction(transactionId);
        }
        else
        {
            receipt.ClearTransaction();
        }
    }
}