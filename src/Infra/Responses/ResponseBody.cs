using API.Infra.Exceptions;

namespace API.Infra.Responses
{
    /// <summary>
    /// The layout for responses 
    /// </summary>
    public class ResponseBody
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public object Content { get; set; }

        public static ResponseBody HandledError(BusinessException be) => new ResponseBody() { Message = be.Message, Content = string.Empty, Success = false };
#if DEBUG
        public static ResponseBody UnhandledError(Exception e) => new ResponseBody() { Message = e.Message + e.InnerException?.Message, Content = string.Empty, Success = false };
#else
        public static ResponseBody UnhandledError(Exception _) => new ResponseBody() { Message = "Internal error. Please try later or contact the support", Content = string.Empty, Success = false};
#endif
        public static ResponseBody NoContentSuccess(string message) => new ResponseBody() { Message = message, Content = string.Empty, Success = true };
        public static ResponseBody WithContentSuccess(string message, object content) => new ResponseBody() { Message = message, Content = content, Success = true };
    }
}
