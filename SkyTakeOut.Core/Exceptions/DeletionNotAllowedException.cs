namespace SkyTakeOut.Core.Exceptions
{
    public class DeletionNotAllowedException : Exception
    {
        public DeletionNotAllowedException(string message)
            : base(message)
        {
        }
    }
}
