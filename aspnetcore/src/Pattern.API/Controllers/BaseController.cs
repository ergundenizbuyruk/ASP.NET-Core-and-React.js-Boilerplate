using Microsoft.AspNetCore.Mvc;
using Pattern.Core.Responses;

namespace Pattern.API.Controllers;

public class BaseController : ControllerBase
{
    protected IActionResult Success(object? response = null, int statusCode = 200)
    {
        var res = ResponseDto.Success(response, statusCode);

        return new ObjectResult(res)
        {
            StatusCode = res.StatusCode,
        };
    }
}