namespace JoyEats.Common
{
    public class BusinessException : Exception
    {
        /// <summary>
        /// 自定义状态码
        /// </summary>
        public int Code { get; set; }

        public BusinessException(string message, int code = 0)
            : base(message)
        {
            Code = code;
        }
    }
}
