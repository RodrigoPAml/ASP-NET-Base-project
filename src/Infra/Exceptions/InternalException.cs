namespace API.Infra.Exceptions
{
    /// <summary>
    /// A non excepted exception
    /// </summary>
    public class InternalException : Exception
    {
        public InternalException(string message) : 
            base(
#if DEBUG
                message
#else
                "Erro interno. Por favor tente mais tarde ou contate o suporte"
#endif
                )
        {
        }
    }
}
