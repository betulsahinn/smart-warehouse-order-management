using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swoms.Application.Common.Exceptions;
using Swoms.Domain.Common;

namespace Swoms.API.Middleware;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problem = exception switch
        {
            ValidationException validationException => CreateValidationProblem(validationException),
            NotFoundException notFoundException => CreateProblem(StatusCodes.Status404NotFound, "Resource not found", notFoundException.Message),
            DomainException domainException => CreateProblem(StatusCodes.Status400BadRequest, "Business rule violation", domainException.Message),
            UnauthorizedAccessException unauthorizedAccessException => CreateProblem(StatusCodes.Status401Unauthorized, "Unauthorized", unauthorizedAccessException.Message),
            _ => CreateProblem(StatusCodes.Status500InternalServerError, "An unexpected error occurred", "The server encountered an unexpected error.")
        };

        if (problem.Status == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception");
        }
        else
        {
            _logger.LogWarning(exception, "Handled exception: {Title}", problem.Title);
        }

        context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(problem);
    }

    private static ProblemDetails CreateProblem(int status, string title, string detail)
    {
        return new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail
        };
    }

    private static ValidationProblemDetails CreateValidationProblem(ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(group => group.Key, group => group.Select(error => error.ErrorMessage).ToArray());

        return new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed"
        };
    }
}
