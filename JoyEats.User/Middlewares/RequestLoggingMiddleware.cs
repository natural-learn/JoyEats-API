using System.Diagnostics;

namespace JoyEats.User.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                _logger.LogInformation("HTTP {Method} {Path} started.", context.Request.Method, context.Request.Path);
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var statusCode = context.Response.StatusCode;
                var level = statusCode >= 500 ? LogLevel.Error : LogLevel.Information;
                _logger.Log(
                    level,
                    "HTTP {Method} {Path} completed with {StatusCode} in {ElapsedMilliseconds}ms",
                    context.Request.Method,
                    context.Request.Path,
                    statusCode,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
