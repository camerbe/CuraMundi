using System.Net;
using System.Net.Http.Headers;
using CuraMundi.Application.BLL.CustomExceptions;
namespace CuraMundi.Middlewares
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
            catch (Exception ex)
            {
                int statusCode;
                switch (ex)
                {
                    case InvalidLoginException:
                        statusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case EntityNotFoundException<object>:
                        statusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                context.Response.StatusCode = statusCode; // Use the statusCode variable
                string responseMessage = context.Response.StatusCode == 500 ? "Server error" : ex.Message;
                await context.Response.WriteAsync(responseMessage);
            }
        }
    }
}
