using System.Linq.Expressions;

namespace PortalGenius.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class 
    {
        public Task<TEntity> GetByIdAsync(string id);

        public Task<IEnumerable<TEntity>> GetAllAsync();

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression);

        public void Add(TEntity entity);

        public void AddRange(IEnumerable<TEntity> entities);

        public void Remove(TEntity entity);

        public void RemoveRange(IEnumerable<TEntity> entities);

        public Task<int> SaveChangesAsync();
    }
}
