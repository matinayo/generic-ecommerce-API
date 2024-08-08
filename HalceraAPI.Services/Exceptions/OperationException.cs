using System.Net;

namespace HalceraAPI.Services.Exceptions
{
    public class OperationException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public OperationException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class NotFoundException : OperationException
    {
        public NotFoundException(string message = "Not Found")
            : base(message, HttpStatusCode.NotFound)
        {
        }
    }
}
