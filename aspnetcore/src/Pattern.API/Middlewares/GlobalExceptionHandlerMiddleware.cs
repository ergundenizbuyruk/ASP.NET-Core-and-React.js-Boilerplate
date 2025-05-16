using Pattern.Core.Exceptions;
using Pattern.Core.Responses;

namespace Pattern.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware(
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                string errorMessage = $"Bir hata oluştu. Hata mesajı :  {exception.Message}";
                logger.LogError(exception, errorMessage);

                context.Response.ContentType = "application/json";

                var exceptionResponseModel = exception switch
                {
                    EntityNotFoundException => new ExceptionResponseModel(404, exception.Message),
                    _ => new ExceptionResponseModel(500, "Bir hata oluştu."),
                };

                context.Response.StatusCode = exceptionResponseModel.StatusCode;
                var response =
                    ResponseDto<NoContentDto>.Fail(exceptionResponseModel.Message, exceptionResponseModel.StatusCode);
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

    public class ExceptionResponseModel(int statusCode, string message)
    {
        public int StatusCode { get; } = statusCode;
        public string Message { get; } = message;
    }
}