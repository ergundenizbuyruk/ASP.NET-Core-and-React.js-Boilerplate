using Pattern.Persistence.Context;

namespace Pattern.Persistence.UnitOfWork;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public void SaveChanges()
    {
        context.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}