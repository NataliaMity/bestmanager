using Application.Common;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Web.ErrorHandling
{
    /// <summary>
    /// Превращает исключения приложения в ProblemDetails с правильным HTTP-кодом.
    /// Всё остальное пропускает дальше, и оно станет 500.
    /// </summary>
    internal class ExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (status, title) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Не найдено"),
                DomainException => (StatusCodes.Status400BadRequest, "Некорректный запрос"),
                // Битый JSON, неверный тип поля и т.п. — ошибка клиента, а не сервера
                BadHttpRequestException bad => (bad.StatusCode, "Некорректный запрос"),
                _ => (0, null)
            };
            if (status == 0)
                return false;

            httpContext.Response.StatusCode = status;
            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails =
                {
                    Status = status,
                    Title = title,
                    Detail = exception.Message
                }
            });
        }
    }
}
