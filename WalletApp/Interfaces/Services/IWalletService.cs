using WalletApp.Core.Infrastructure;
using WalletApp.Core.Models;
using WalletApp.Models;

namespace WalletApp.Interfaces.Services
{
    public interface IWalletService
    {
        Task<Guid> RegisterNewWallet(Guid id);
        Task<decimal> GetBalance(Guid playerId);
        Task<TransactionState> MakeTransaction(InputTransaction transaction);
        Task<List<Transaction>> GetTransactions(Guid playerId);
    }
}
