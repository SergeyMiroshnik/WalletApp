using WalletApp.Core.Infrastructure;
using WalletApp.Core.Models;
using WalletApp.Extensions;
using WalletApp.Models;

namespace WalletApp.Mappers
{
    public static class TransactionMapper
    {
        public static Transaction ToDbEntity(InputTransaction inputTransaction, Guid walletId)
        {
            return new Transaction
            {
                Amount = inputTransaction.Amount,
                Id = inputTransaction.Id.ToGuid(),
                WalletId = walletId,
                Type = Enum.Parse<TransactionType>(inputTransaction.Type, true)
            };
        }

        public static OutputTransaction ToOutputModel(Transaction transaction)
        {
            return new OutputTransaction
            {
                Amount = transaction.Amount,
                Id = transaction.Id,
                CreatedAt = transaction.CreatedAt,
                ErrorText = transaction.ErrorText,
                State = transaction.State.ToString(),
                Type = transaction.Type.ToString()
            };
        }
    }
}
