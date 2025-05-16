namespace Pattern.Core.Exceptions;

public class NotFoundException(string message) : Exception(message)
{
}