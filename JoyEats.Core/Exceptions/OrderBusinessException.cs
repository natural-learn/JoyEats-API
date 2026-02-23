namespace JoyEats.Core.Exceptions
{
    public class OrderBusinessException : Exception
    {
        public OrderBusinessException(string message)
            : base(message)
        {
        }
    }
}
