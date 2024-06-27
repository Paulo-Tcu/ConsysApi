using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace ConsysApi.Data.Interfaces
{
    public interface IRepositoryEF<TContext>
        where TContext : DbContext
    {
        public TContext Context { get; }

        public int Save();

        public Task<int> SaveAsync();

        public void DeleteEntity<TEntity>(TEntity entity)
            where TEntity : class;

        public void DeleteEntities<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class;

        public void AddEntity<TEntity>(TEntity entity)
            where TEntity : class;

        public void AddRangeEF<TEntity>(IEnumerable<TEntity> entities, bool alterStateEntity = false)
            where TEntity : class;

        public Task AddRangeEFAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class;

        public IEnumerable<TEntity> SelectEntities<TEntity>(Expression<Func<TEntity, bool>> filterExpression)
            where TEntity : class;

        public IEnumerable<TEntity> SelectEntities<TEntity>(
            Expression<Func<TEntity, bool>> filterExpression,
            params Expression<Func<TEntity, object>>[] includeExpressions)
                where TEntity : class;

        public void UpdateEntity<TEntity>(TEntity entity, Action<TEntity> updateAction)
            where TEntity : class;

        public void UpdateEntity<TEntity>(TEntity entity)
            where TEntity : class;

        //public void UpdateEntity<TEntity>(TEntity entity, Expression<Func<TEntity, object>> propertyExpression)
        //    where TEntity : class;
    }
}
