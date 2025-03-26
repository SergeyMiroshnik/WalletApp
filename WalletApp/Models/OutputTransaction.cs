namespace WalletApp.Models
{
    public class OutputTransaction
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string State { get; set; }
        public string? ErrorText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
