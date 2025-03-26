using System.Collections.Concurrent;
using WalletApp.Core.Interfaces.Repositories;
using WalletApp.Core.Models;

namespace WalletApp.DataLayerMock.Repositories
{
    /// <summary>
    /// There is no sense to make the in-memory structures completely asynchronous so for this mock it is enough to add the stucture Task.FromResult(....)
    /// </summary>
    public class TransactionRepositoryMock : ITransactionRepository
    {
        private static ConcurrentDictionary<Guid, List<Transaction>> _transactions = new();
        public TransactionRepositoryMock() { }

        public Task<List<Transaction>> GetAllAsync(Guid walletId)
        {
            var res = _transactions.TryGetValue(walletId, out var result) ?
                result :
                new List<Transaction>();
            return Task.FromResult(res);
        }

        public Task<Transaction> GetTransactionAsync(Guid walletId, Guid trId)
        {
            var playerTransactions = _transactions.GetOrAdd(walletId, _ => new());
            lock (playerTransactions)
            {
                return Task.FromResult(playerTransactions.FirstOrDefault(tr => tr.Id == trId));
            }
        }

        public Task<bool> AddAsync(Guid walletId, Transaction transaction)
        {
            try
            {
                var playerTransactions = _transactions.GetOrAdd(walletId, _ => new());
                lock (playerTransactions)
                {
                    playerTransactions.Add(transaction);
                    return Task.FromResult(true);
                }
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}
