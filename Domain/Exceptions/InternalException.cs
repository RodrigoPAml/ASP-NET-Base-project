namespace Domain.Exceptions
{
    /// <summary>
    /// A non excepted exception
    /// </summary>
    public class InternalException : Exception
    {
        public InternalException(string message) : base(message)
        {
        }
    }
}
