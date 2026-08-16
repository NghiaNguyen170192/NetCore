namespace NetCore.Donation.Application.Receipt.Update;

public static class UpdateReceiptExtension
{
    public static void UpdateEntity(
        this UpdateReceiptCommand request,
        Domain.Entities.Receipt receipt,
        Guid? paymentScheduleId)
    {
        if (request.TransactionId is { } transactionId)
        {
            if (paymentScheduleId is null)
            {
                throw new ArgumentException("Payment schedule ID is required when a transaction is assigned.", nameof(paymentScheduleId));
            }

            receipt.AssignTransaction(transactionId, paymentScheduleId.Value);
        }
        else
        {
            receipt.ClearTransaction();
        }
    }
}
