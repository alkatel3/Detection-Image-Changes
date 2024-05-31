using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using API.Model;
using API.DAL.Abstraction;

namespace API.DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext _db;
        internal DbSet<TEntity> dbSet;

        public Repository(ApplicationDbContext context)
        {
            _db = context;
            dbSet = context.Set<TEntity>();
        }
        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> quety = dbSet;
            quety = quety.Where(filter);
            return quety.FirstOrDefault();
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList();
        }

        public virtual void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public virtual void UpDate(TEntity item)
        {
            dbSet.Update(item);
        }
    }
}
