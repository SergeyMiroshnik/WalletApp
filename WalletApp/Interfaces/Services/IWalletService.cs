using WalletApp.Core.Infrastructure;
using WalletApp.Models;

namespace WalletApp.Interfaces.Services
{
    public interface IWalletService
    {
        Task<Guid> RegisterNewWallet(Guid id);
        Task<decimal> GetBalance(Guid playerId);
        Task<TransactionState> MakeTransaction(InputTransaction transaction);
        Task<List<OutputTransaction>> GetTransactions(Guid playerId);
    }
}
