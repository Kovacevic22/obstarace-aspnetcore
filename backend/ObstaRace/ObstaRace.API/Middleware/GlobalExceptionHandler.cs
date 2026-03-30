using Amazon.S3;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ObstaRace.API.Middleware;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger):IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception");
        var (statusCode, title, detail) = exception switch
        {
            ValidationException fluentEx => (StatusCodes.Status400BadRequest, "Validation Error",
                fluentEx.Errors.FirstOrDefault()?.ErrorMessage ?? "Validation Error"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized", exception.Message),
            AmazonS3Exception => (StatusCodes.Status502BadGateway, "Storage Service Error", exception.Message), 
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred.")
        };
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Type = exception.GetType().Name,
            Extensions = { ["requestId"] = httpContext.TraceIdentifier }
        }, cancellationToken);
        return true;
    }    
}