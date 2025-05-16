namespace Pattern.Core.Exceptions;

public class UserIsNotActiveException(string message) : Exception(message)
{
}