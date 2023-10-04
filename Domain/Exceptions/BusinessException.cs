namespace Domain.Exceptions
{
    /// <summary>
    /// Program rule exception
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}
