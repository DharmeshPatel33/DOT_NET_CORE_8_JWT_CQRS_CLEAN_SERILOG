using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Extensions.Hosting;

namespace APIService.Middlerwares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly DiagnosticContext _diagnosticContext;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger, DiagnosticContext diagnosticContext)
        {
            _next = next;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception, "Exception occurred: {Message}", exception.Message);

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = exception.GetBaseException().Message,
                    Type = exception.Source
                };

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;
                _diagnosticContext.SetException(exception);
                await context.Response.WriteAsJsonAsync(problemDetails);

                
            }
        }
    }
}
