using WalletApp.Core.Infrastructure;

namespace WalletApp.Models
{
    public class InputTransaction
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
    }
}
