using ConsysApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using System.Linq.Expressions;

namespace ConsysApi.Repository
{
    public abstract class RepositoryEF<TContext> //: IRepositoryEF<TContext>
        where TContext : ConsysContext
    {
        public TContext Context { get; }

        protected bool _disposedContext = false;

        protected IConfiguration Configuration { get; private set; }

        void ContextDisposed(object? sender, EventArgs e)
        {
            _disposedContext = true;
        }

        public RepositoryEF(TContext context, IConfiguration config)
        {
            Context = context;
            Context.Disposed += ContextDisposed;
            Configuration = config;
        }

        public virtual int Save()
        {
            return Context.SaveChanges();
        }

        public virtual async Task<int> SaveAsync()
        {
            var linhasAfetadas = await Context.SaveChangesAsync();
            return linhasAfetadas;
        }

        public virtual void DeleteEntity<TEntity>(TEntity entity)
            where TEntity : class
        {
            Context.Entry(entity).State = EntityState.Deleted;
            Context.Set<TEntity>().Remove(entity);
        }

        public virtual void DeleteEntities<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            foreach (var item in entities)
                Context.Entry(item).State = EntityState.Deleted;

            Context.Set<TEntity>().RemoveRange(entities);
        }

        public virtual void AddEntity<TEntity>(TEntity entity)
            where TEntity : class
        {
            Context.Entry(entity).State = EntityState.Added;

            Context.Add(entity);
        }

        public virtual void AddRangeEF<TEntity>(IEnumerable<TEntity> entities, bool alterStateEntity = false)
            where TEntity : class
        {
            if (alterStateEntity)
                foreach (var item in entities)
                    Context.Entry(item).State = EntityState.Added;

            Context.AddRange(entities);
        }

        public virtual async Task AddRangeEFAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            foreach (var item in entities)
                Context.Entry(item).State = EntityState.Added;

            await Context.AddRangeAsync(entities);
        }

        public virtual IEnumerable<TEntity> SelectEntities<TEntity>(Expression<Func<TEntity, bool>> filterExpression)
            where TEntity : class
        {
            return Context.Set<TEntity>().Where(filterExpression).AsEnumerable();
        }

        public virtual IEnumerable<TEntity> SelectEntities<TEntity>(
            Expression<Func<TEntity, bool>> filterExpression,
            params Expression<Func<TEntity, object>>[] includeExpressions)
                where TEntity : class
        {
            var queryable = Context.Set<TEntity>().Where(filterExpression);

            foreach (var includeExpression in includeExpressions)
            {
                queryable = queryable.Include(includeExpression);
            }

            return queryable.AsEnumerable();
        }

        public virtual void UpdateEntity<TEntity>(TEntity entity, Action<TEntity> updateAction)
            where TEntity : class
        {
            var originalValues = Context.Entry(entity).OriginalValues;

            updateAction(entity);

            var currentValues = Context.Entry(entity).CurrentValues;
            var propsIsModified = GetPropertiesOnModified(originalValues, currentValues).ToList();

            foreach (var item in propsIsModified)
            {
                Context.Entry(entity).Property(item).IsModified = true;
            }
        }

        public virtual void UpdateEntity<TEntity>(TEntity entity)
            where TEntity : class
        {
            var originalValues = Context.Entry(entity).OriginalValues;
            var currentValues = Context.Entry(entity).CurrentValues;
            var propsIsModified = GetPropertiesOnModified(originalValues, currentValues).ToList();

            foreach (var item in propsIsModified)
            {
                Context.Entry(entity).Property(item).IsModified = true;
            }
        }

        protected virtual IEnumerable<string> GetPropertiesOnModified(PropertyValues originalValues, PropertyValues currentValues)
        {
            //Captura as propriedades de cada entidade
            var properties = currentValues.Properties;

            foreach (var property in properties)
            {
                dynamic valueNew = currentValues[property];
                dynamic valueOld = originalValues[property];

                // Verifica se os tipos são compatíveis antes de comparar
                if ((valueNew?.GetType() == valueOld?.GetType()) || (valueNew == null && valueOld != null) || (valueNew != null && valueOld == null))
                {
                    // Comparação dos valores
                    if (valueNew != valueOld)
                    {
                        yield return property.Name;
                    }
                }
            }
        }
    }
}
