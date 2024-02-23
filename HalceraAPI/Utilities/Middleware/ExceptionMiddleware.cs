using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Utilities.Exceptions;
using System.Net;

namespace HalceraAPI.Utilities.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Internal Server Error";
            if (exception is OperationException operationException)
            {
                message = operationException.Message;
                statusCode = (int)operationException.StatusCode;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorDetails = new ErrorDetails(
                Errors: new List<string> { exception.Message },
                StatusCode: statusCode,
                Message: message
            );

            await context.Response.WriteAsync(errorDetails.ToString());
        }
    }
}
