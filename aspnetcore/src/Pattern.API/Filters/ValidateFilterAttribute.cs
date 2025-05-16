using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pattern.Core.Responses;

namespace Pattern.API.Filters
{
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(x => x.Errors.Select(x => x.ErrorMessage)).ToList();

                context.Result = new BadRequestObjectResult(
                    ResponseDto<NoContentDto>.Fail(new ErrorDto(errors), 400));
            }
        }
    }
}
