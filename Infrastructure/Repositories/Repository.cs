using Core.IRepositories;
using Dapper;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class Repository<TEntity, TKey> : DbContextBase, IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly DbSet<TEntity> _entity;
        private readonly string _connectionString;
        public Repository(AppDbContext context, string connectionString) : base(context)
        {
            _entity = context.Set<TEntity>();
            _connectionString = connectionString;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);
        }

        public async Task DeleteAsync(TKey id)
        {
            var item = await GetByIdAsync(id);
            if (item != null)
            {
                _entity.Remove(item);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entity.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllDapperAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var query = $"SELECT * FROM {typeof(TEntity).Name}s";
            return await connection.QueryAsync<TEntity>(query);
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _entity.FindAsync(id);
        }

        public async Task<TEntity?> GetByIdWithIncludeAsync(TKey id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _entity;

            foreach (var include in includes)
                query = query.Include(include);

            var keyName = _context.Model
                .FindEntityType(typeof(TEntity))!
                .FindPrimaryKey()!
                .Properties[0].Name;

            return await query.FirstOrDefaultAsync(e =>
                EF.Property<TKey>(e, keyName)!.Equals(id));
        }
        public async Task UpdateAsync(TEntity entity)
        {
            _entity.Update(entity);
        }
        public async Task UpdatePartialAsync<TViewModel>(TKey id, TViewModel viewModel, TEntity? entity)
            where TViewModel : class
        {
            if (entity == null)
                throw new Exception("Entity not found");

            var entityType = typeof(TEntity);
            var viewModelType = typeof(TViewModel);

            foreach (var vmProp in viewModelType.GetProperties())
            {
                var entityProp = entityType.GetProperty(vmProp.Name);

                if (entityProp != null && entityProp.CanWrite)
                {
                    var newValue = vmProp.GetValue(viewModel);

                    if (newValue != null)
                    {
                        entityProp.SetValue(entity, newValue);
                        _context.Entry(entity).Property(vmProp.Name).IsModified = true;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        protected override void ConfigureEntities(ModelBuilder modelBuilder)
        {
        }

        public IQueryable<TEntity> AsNoTracking()
        {
            return _entity.AsNoTracking();
        }

        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entity.AnyAsync(predicate);
        }
    }
}
