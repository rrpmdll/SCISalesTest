using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SCISalesTest.Domain.Exceptions;

namespace SCISalesTest.WebApi.Base.Filters;

[AttributeUsage(AttributeTargets.All)]
public sealed class AppExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<AppExceptionFilterAttribute> _logger;

    public AppExceptionFilterAttribute(ILogger<AppExceptionFilterAttribute> logger)
    {
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = context.Exception switch
        {
            NotFoundException => (int)HttpStatusCode.NotFound,
            ValidationException => (int)HttpStatusCode.BadRequest,
            ExternalServiceException => (int)HttpStatusCode.ServiceUnavailable,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var message = context.Exception switch
        {
            NotFoundException or ValidationException or ExternalServiceException => context.Exception.Message,
            _ => "An unexpected error occurred. Please try again later."
        };

        _logger.LogError(context.Exception, "Unhandled exception: {Message}", context.Exception.Message);

        context.Result = new ObjectResult(new
        {
            context.HttpContext.Response.StatusCode,
            Message = message,
            Timestamp = DateTime.UtcNow
        });

        context.ExceptionHandled = true;
    }
}
