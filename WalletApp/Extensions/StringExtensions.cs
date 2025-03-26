namespace WalletApp.Extensions
{
    public static class StringExtensions
    {
        public static Guid ToGuid(this string value) =>
            Guid.Parse(value);

        public static bool TryToGuid(this string value, out Guid guid) =>
            Guid.TryParse(value, out guid);
    }
}
