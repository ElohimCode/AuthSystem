using Application.Exceptions;
using Common.Responses.Wrappers;
using System.Net;
using System.Text.Json;

namespace WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }catch (Exception ex)
            {
                await HandleException(ex, context);
            }
        }

        private static async Task HandleException(Exception ex, HttpContext context)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseWrapper = await ResponseWrapper.FailAsync(ex.Message);
            switch (ex)
            {
                case CustomValidationException exception:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonSerializer.Serialize(responseWrapper);
            await response.WriteAsync(result);
        }
    }
}
