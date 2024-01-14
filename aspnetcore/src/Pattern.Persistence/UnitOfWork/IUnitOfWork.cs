namespace Pattern.Persistence.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        void SaveChanges();
    }
}
