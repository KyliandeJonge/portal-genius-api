using Microsoft.EntityFrameworkCore;
using PortalGenius.Core.Interfaces;
using System.Linq.Expressions;

namespace PortalGenius.Infrastructure.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _appDbContext;

        private readonly DbSet<TEntity> _table;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _table = _appDbContext.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _table.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _table.AddRange(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return _table.Where(expression);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _table.FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            _table.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _table.RemoveRange(entities);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }
    }
}
