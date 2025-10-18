namespace SkyTakeOut.Core.Exceptions
{
    public class AccountLockedException : Exception
    {
        public AccountLockedException(string message)
            : base(message)
        {
        }
    }
}
