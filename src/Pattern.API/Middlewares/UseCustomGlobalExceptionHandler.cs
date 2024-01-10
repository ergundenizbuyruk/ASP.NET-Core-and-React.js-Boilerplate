using Microsoft.AspNetCore.Diagnostics;
using Pattern.Core.Exceptions;
using Pattern.Core.Responses;

namespace Pattern.API.Middlewares
{
	public static class UseCustomGlobalExceptionHandler
	{
		public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
		{
			app.UseExceptionHandler(options =>
			{
				options.Run(async context =>
				{
					context.Response.ContentType = "application/json";
					var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

					var statusCode = exceptionFeature.Error switch
					{
						EntityNotFoundException => 404,
						_ => 500
					};

					context.Response.StatusCode = statusCode;
					var response = ResponseDto<NoContentDto>.Fail(exceptionFeature.Error.Message, statusCode);
					await context.Response.WriteAsJsonAsync(response);
				});
			});
		}
	}
}
