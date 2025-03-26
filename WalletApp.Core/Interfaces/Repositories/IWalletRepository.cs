using WalletApp.Core.Infrastructure;
using WalletApp.Core.Models;

namespace WalletApp.Core.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        Task<Guid> AddAsync(Wallet wallet);
        Task<Wallet> GetAsync(Guid id);
        Task<Wallet> FindByPlayerAsync(Guid playerId);
        Task<bool> UpdateAsync(Wallet wallet);
    }
}
