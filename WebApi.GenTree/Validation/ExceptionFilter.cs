using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.GenTree.Validation;

/// <summary>
/// Модель результата
/// </summary>
/// <param name="Data">Результат</param>
/// <param name="Success">Флаг успешности выполнения запроса</param>
/// <param name="Errors">Список описаний ошибок</param>
public record ResultModel<T>(T Data, bool Success = true, string[] Errors = null);

/// <summary>
/// Модель ошибки
/// </summary>
/// <param name="Errors">Список описаний ошибок</param>
/// <param name="Success">Флаг успешности выполнения запроса</param>
public record ErrorModel(string[] Errors, bool Success = false);

/// <inheritdoc/>
public class ExceptionFilter(ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    /// <inheritdoc/>
    public void OnException(ExceptionContext context)
    {
        Exception exception = context.Exception;
        string route = string.Join("/", context.RouteData.Values.Values.Reverse());

        var (faultData, status) = UnwrapExcepton(exception, route);

        context.HttpContext.Response.StatusCode = status;
        context.HttpContext.Response.WriteAsJsonAsync(faultData).Wait();

        context.ExceptionHandled = true;
    }

    private (ErrorModel FaultData, int StatusCode) UnwrapExcepton(Exception exception, string route)
    {
        logger.LogError(exception, "Error on handle route '{route}'", route);

        int status = StatusCodes.Status500InternalServerError;
        ErrorModel faultData = new(["Внутреняя ошибка сервера"]);
        return (faultData, status);
    }
}
