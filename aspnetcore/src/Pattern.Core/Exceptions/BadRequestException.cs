using System.Net;

namespace Pattern.Core.Exceptions;

public sealed class BadRequestException : BaseHttpException
{
    public BadRequestException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.BadRequest;
    }
}