namespace Pattern.Core.Exceptions;

public class UserIsLockedOutException(string message) : Exception(message)
{
}