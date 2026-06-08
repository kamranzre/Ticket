using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(TKey id);
        Task<TEntity?> GetByIdWithIncludeAsync(TKey id, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task UpdatePartialAsync<TViewModel>(TKey id, TViewModel viewModel,TEntity? entity)where TViewModel : class;
        Task DeleteAsync(TKey id);
        IQueryable<TEntity> AsNoTracking();
        Task<IEnumerable<TEntity>> GetAllDapperAsync();
        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
