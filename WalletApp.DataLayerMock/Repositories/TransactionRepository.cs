using System.Collections.Concurrent;
using WalletApp.Core.Interfaces.Repositories;
using WalletApp.Core.Models;

namespace WalletApp.DataLayerMock.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private static ConcurrentDictionary<Guid, List<Transaction>> _transactions = new();
        public TransactionRepository() { }

        public async Task<List<Transaction>> GetAllAsync(Guid walletId)
        {
            return _transactions.TryGetValue(walletId, out var result) ?
                result :
                new List<Transaction>();
        }

        public async Task<Transaction> GetTransactionAsync(Guid walletId, Guid trId)
        {
            var playerTransactions = _transactions.GetOrAdd(walletId, _ => new());
            lock (playerTransactions)
            {
                return playerTransactions.FirstOrDefault(tr => tr.Id == trId);
            }
        }

        public async Task<bool> AddAsync(Guid walletId, Transaction transaction)
        {
            try
            {
                var playerTransactions = _transactions.GetOrAdd(walletId, _ => new());
                lock (playerTransactions)
                {
                    playerTransactions.Add(transaction);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public Task<bool> UpdateAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
