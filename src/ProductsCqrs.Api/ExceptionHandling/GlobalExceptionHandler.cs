using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductsCqrs.Application.Common;
using ProductsCqrs.Domain.Common;

namespace ProductsCqrs.Api.ExceptionHandling;

public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        var problem = CreateProblem(exception);

        if (problem.Status == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Excepción no controlada");

        httpContext.Response.StatusCode = problem.Status!.Value;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problem
        });
    }

    private static ProblemDetails CreateProblem(Exception exception) => exception switch
    {
        ValidationException validationException => new HttpValidationProblemDetails(GroupErrorsByProperty(validationException))
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Datos de entrada inválidos"
        },
        BadHttpRequestException => new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Solicitud inválida",
            Detail = "El cuerpo o los parámetros de la solicitud no tienen un formato válido."
        },
        NotFoundException notFound => new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Recurso no encontrado",
            Detail = notFound.Message
        },
        DomainException domainException => new ProblemDetails
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "Regla de negocio violada",
            Detail = domainException.Message
        },
        _ => new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Error interno del servidor"
        }
    };

    private static Dictionary<string, string[]> GroupErrorsByProperty(ValidationException exception)
        => exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(group => group.Key, group => group.Select(error => error.ErrorMessage).ToArray());
}
