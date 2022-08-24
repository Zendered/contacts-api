namespace ContactsApi.Exceptions
{
    public class HandlerExceptions
    {
        public int StatusCode { get; private set; }
        public string Message { get; private set; } = string.Empty;

        public HandlerExceptions(string message, int statusCode)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
