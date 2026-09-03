using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Business.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception occurred. Path: {Path}",
                context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            ValidationException =>
                (HttpStatusCode.BadRequest, exception.Message),

            ArgumentException =>
                (HttpStatusCode.BadRequest, exception.Message),


            KeyNotFoundException =>
                (HttpStatusCode.NotFound, exception.Message),

            UnauthorizedAccessException =>
                (HttpStatusCode.Unauthorized, exception.Message),

            InvalidOperationException =>
                (HttpStatusCode.UnprocessableEntity, exception.Message),

            ConflictException =>
                (HttpStatusCode.Conflict, exception.Message),

            _ =>
                (HttpStatusCode.InternalServerError,
                 "An unexpected error occurred.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Instance = context.Request.Path
        };

        if (_environment.IsDevelopment())
        {
            problemDetails.Extensions["exception"] =
                exception.GetType().Name;

            problemDetails.Extensions["detail"] =
                exception.Message;
        }

        problemDetails.Extensions["traceId"] =
            context.TraceIdentifier;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problemDetails, options));
    }
}