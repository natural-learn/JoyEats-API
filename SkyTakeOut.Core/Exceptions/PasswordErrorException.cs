namespace SkyTakeOut.Core.Exceptions
{
    /// <summary>
    /// 密码错误异常
    /// </summary>
    public class PasswordErrorException : Exception
    {
        public PasswordErrorException(string message)
            : base(message)
        {
        }
    }
}
