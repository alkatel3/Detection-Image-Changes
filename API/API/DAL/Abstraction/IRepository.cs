using System.Linq.Expressions;

namespace API.DAL.Abstraction
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null);
        TEntity Get(Expression<Func<TEntity, bool>> filter);
        void Add(TEntity entity);
        void UpDate(TEntity item);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
