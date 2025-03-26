using WalletApp.Core.Models;

namespace WalletApp.Core.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllAsync(Guid walletId);
        Task<Transaction> GetTransactionAsync(Guid walletId, Guid trId);
        Task<bool> AddAsync(Guid walletId, Transaction transaction);
    }
}
