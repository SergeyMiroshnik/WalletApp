using WalletApp.Core.Infrastructure;

namespace WalletApp.Core.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public TransactionState State { get; set; }
        public string? ErrorText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
