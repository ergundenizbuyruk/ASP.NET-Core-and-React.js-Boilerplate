using System.Net;

namespace Pattern.Core.Exceptions;

public sealed class NotFoundException : BaseHttpException
{
    public NotFoundException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.NotFound;
    }
}