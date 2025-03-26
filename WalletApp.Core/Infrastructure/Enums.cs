namespace WalletApp.Core.Infrastructure
{
    public enum TransactionType
    {
        Deposit = 0,
        Stake,
        Win
    }

    public enum TransactionState
    {
        Accepted = 0,
        Declined
    }
}
