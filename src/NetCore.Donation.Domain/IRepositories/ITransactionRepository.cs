using NetCore.Donation.Domain.Entities;

namespace NetCore.Donation.Domain.IRepositories;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);

    Task<bool> IsExistAsync(Guid id, CancellationToken cancellationToken);

    Task<Transaction?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    void Delete(Transaction transaction);

    IQueryable<Transaction> GetAll();
}