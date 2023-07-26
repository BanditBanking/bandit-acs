using Bandit.ACS.Daemon.Exceptions;
using Bandit.ACS.Daemon.Models.DTOs;
using System.Text.Json;

namespace Bandit.ACS.Daemon.Extensions
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ExceptionHandlerMiddlewareOptions _options;

        public ExceptionHandlerMiddleware(RequestDelegate next, ExceptionHandlerMiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var problemDetail = (exception is IExposedException exposedException) ? exposedException.Expose() : ProblemDetailDTO.Default;
            problemDetail.Type = GetErrorUri(problemDetail.ErrorCode);
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problemDetail.HttpCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetail));
        }

        private string GetErrorUri(string exceptionCode) => _options.DocumentationPath + exceptionCode;
    }
}
