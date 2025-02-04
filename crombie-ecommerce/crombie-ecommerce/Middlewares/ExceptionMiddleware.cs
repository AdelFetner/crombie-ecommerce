using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CrombieEcommerce.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogErrorWithContext(ex, context);
                await HandleExceptionAsync(context);
            }
        }

        private void LogErrorWithContext(Exception ex, HttpContext context)
        {
            using (_logger.BeginScope(new
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                User = context.User.Identity?.Name ?? "Anonymous"
            }))
            {
                _logger.LogError(ex, "Unhandled exception occurred");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.StatusCode = 500;
            return context.Response.WriteAsync("An internal error occurred");
        }
    }
}