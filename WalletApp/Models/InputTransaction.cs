namespace WalletApp.Models
{
    public class InputTransaction
    {
        public string Id { get; set; }
        public string PlayerId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
