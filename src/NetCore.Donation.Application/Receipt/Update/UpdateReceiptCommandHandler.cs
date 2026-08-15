using MediatR;
using NetCore.Donation.Domain.IRepositories;
using NetCore.Donation.Domain.SharedKernel;

namespace NetCore.Donation.Application.Receipt.Update;

public class UpdateReceiptCommandHandler(
    IUnitOfWork unitOfWork,
    IReceiptRepository receiptRepository,
    ITransactionRepository transactionRepository)
    : IRequestHandler<UpdateReceiptCommand, bool>
{
    public async Task<bool> Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
    {
        var receipt = await receiptRepository.FindByIdAsync(request.Id, cancellationToken);
        if (receipt is null)
        {
            return false;
        }

        if (request.TransactionId is { } transactionId)
        {
            var transaction = await transactionRepository.FindByIdAsync(transactionId, cancellationToken);
            if (transaction is null)
            {
                throw new ArgumentException($"Transaction '{transactionId}' was not found.", nameof(request));
            }

            if (transaction.ContactId != receipt.ContactId)
            {
                throw new InvalidOperationException("The transaction does not belong to the contact.");
            }
        }

        request.UpdateEntity(receipt);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}