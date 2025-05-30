using System.Net;

namespace Pattern.Core.Exceptions;

public abstract class BaseHttpException(string message) : Exception(message)
{
    public HttpStatusCode StatusCode { get; protected init; }
}