namespace ContactsApi.Exceptions
{
    public class NotFoundException : HandlerExceptions
    {
        public NotFoundException(string message = "Contact Doesn't Exist", int statusCode = 404) : base(message, statusCode)
        {

        }
    }
}
