using AutoMapper;

namespace Pattern.Application.Services.Base
{
    public interface IBaseService
    {
        public Task SaveChangesAsync();
        public void SaveChanges();
    }
}