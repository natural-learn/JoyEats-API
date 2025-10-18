namespace SkyTakeOut.Core.Exceptions
{
    /// <summary>
    /// 账号不存在异常
    /// </summary>
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException(string message)
            : base(message)
        {
        }
    }
}
