using Microsoft.AspNetCore.Mvc;
using Pattern.Core.Responses;

namespace Pattern.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult ActionResultInstance<T>(ResponseDto<T> response) where T : class
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode,
            };
        }
    }
}
