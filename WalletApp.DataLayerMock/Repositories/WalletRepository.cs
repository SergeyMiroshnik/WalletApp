using WalletApp.Core.Interfaces.Repositories;
using WalletApp.Core.Models;

namespace WalletApp.DataLayerMock.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private static List<Wallet> wallets = new();

        public WalletRepository() { }

        public async Task<Guid> AddAsync(Wallet wallet)
        {
            wallet.Id = Guid.NewGuid();
            wallets.Add(wallet);
            return wallet.Id;
        }

        public async Task<Wallet> GetAsync(Guid id)
        {
            return wallets.FirstOrDefault(w => w.Id == id);
        }

        public async Task<Wallet> FindByPlayerAsync(Guid playerId)
        {
            return wallets.FirstOrDefault(w => w.PlayerId == playerId);
        }

        public async Task<bool> UpdateAsync(Wallet wallet)
        {
            var wal = wallets.FirstOrDefault(w => w.Id == wallet.Id);
            if (wal == null)
                throw new InvalidOperationException("Invalid wallet");
            wal.Balance = wallet.Balance;
            return true;
        }
    }
}
