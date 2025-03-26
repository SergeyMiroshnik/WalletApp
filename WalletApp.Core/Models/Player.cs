namespace WalletApp.Core.Models
{
    public class Player
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
