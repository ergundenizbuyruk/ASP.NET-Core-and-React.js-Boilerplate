namespace Pattern.Core.Exceptions
{
    public class EntityNotFoundException(string entityName) : Exception(entityName + " not found.");
}