using MediatR;
using NetCore.Donation.Domain.IRepositories;
using NetCore.Donation.Domain.SharedKernel;

namespace NetCore.Donation.Application.Receipt.Delete;

public class DeleteReceiptCommandHandler(
    IUnitOfWork unitOfWork,
    IReceiptRepository receiptRepository)
    : IRequestHandler<DeleteReceiptCommand, bool>
{
    public async Task<bool> Handle(DeleteReceiptCommand request, CancellationToken cancellationToken)
    {
        var receipt = await receiptRepository.FindByIdAsync(request.Id, cancellationToken);
        if (receipt is null)
        {
            return false;
        }

        receiptRepository.Delete(receipt);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}