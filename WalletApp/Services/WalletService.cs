using System.Collections.Concurrent;
using WalletApp.Core.Infrastructure;
using WalletApp.Core.Interfaces.Repositories;
using WalletApp.Core.Models;
using WalletApp.Interfaces.Services;
using WalletApp.Mappers;
using WalletApp.Models;

namespace WalletApp.Services
{
    public class WalletService : IWalletService
    {
        private IWalletRepository _walletRepository;
        private ITransactionRepository _transactionRepository;
        private static ConcurrentDictionary<Guid, SemaphoreSlim> _playersLocks = new();
        public WalletService(IWalletRepository walletRepository, ITransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<Guid> RegisterNewWallet(Guid playerId)
        {
            Wallet wallet = await _walletRepository.FindByPlayerAsync(playerId);
            if (wallet != null)
                throw new ArgumentException("This wallet exists already");

            wallet = new Wallet
            {
                Balance = 0,
                PlayerId = playerId
            };
            return await _walletRepository.AddAsync(wallet);
        }

        public async Task<decimal> GetBalance(Guid playerId)
        {
            var wallet = await GetWalletSafe(playerId);
            return wallet.Balance;
        }

        public async Task<TransactionState> MakeTransaction(InputTransaction inputTransaction)
        {
            var playerLock = _playersLocks.GetOrAdd(inputTransaction.PlayerId, _ => new SemaphoreSlim(1, 1));

            await playerLock.WaitAsync();

            try
            {
                var wallet = await GetWalletSafe(inputTransaction.PlayerId);
                var transaction = await _transactionRepository.GetTransactionAsync(wallet.Id, inputTransaction.Id);
                if (transaction != null)
                    return transaction.State;

                transaction = TransactionMapper.ToDbEntity(inputTransaction, wallet.Id);

                switch (transaction.Type)
                {
                    case TransactionType.Deposit:
                    case TransactionType.Win:
                        wallet.Balance += transaction.Amount;
                        transaction.State = TransactionState.Accepted;
                        break;
                    case TransactionType.Stake:
                        if (wallet.Balance < transaction.Amount)
                        {
                            transaction.State = TransactionState.Declined;
                            transaction.ErrorText = "Transaction couldn't be done. Stake amount will make the balance negative";
                        }
                        else
                        {
                            wallet.Balance -= transaction.Amount;
                            transaction.State = TransactionState.Accepted;
                        }
                        break;
                }
                if (transaction.State == TransactionState.Accepted)
                    await _walletRepository.UpdateAsync(wallet);
                await _transactionRepository.AddAsync(wallet.Id, transaction);
                return transaction.State;
            }
            catch
            {
                return TransactionState.Declined;
            }
            finally
            {
                playerLock.Release();
            }
        }

        public async Task<List<OutputTransaction>> GetTransactions(Guid playerId)
        {
            var wallet = await GetWalletSafe(playerId);
            var transactions = await _transactionRepository.GetAllAsync(wallet.Id);
            return transactions.Select(TransactionMapper.ToOutputModel).ToList();
        }

        private async Task<Wallet> GetWalletSafe(Guid playerId)
        {
            Wallet wallet = await _walletRepository.FindByPlayerAsync(playerId);
            if (wallet == null)
                throw new ArgumentException("The wallet of this player doesn't exist");
            return wallet;
        }
    }
}
