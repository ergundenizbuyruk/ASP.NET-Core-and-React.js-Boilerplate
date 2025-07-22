using Pattern.Core;
using Pattern.Core.Exceptions;
using Pattern.Core.Responses;

namespace Pattern.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (BaseHttpException exception)
            {
                logger.LogError(exception, exception.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)exception.StatusCode;
                var response = ResponseDto.Fail(exception.Message, (int)exception.StatusCode);
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, exception.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                var localizer = context.RequestServices.GetRequiredService<IResourceLocalizer>();
                var message = localizer.Localize("InternalServerError");

                var response = ResponseDto.Fail(message, 500);
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}