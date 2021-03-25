using JewelryStore.Common.Interfaces;
using JewelryStore.DataRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JewelryStore.DataRepository.Classes
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class,IEntity
    {
        protected DbContext _context;
        protected DbSet<TEntity> _dbSet;
        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filterCondition = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string[] include = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if(filterCondition != null)
            {
                query.Where(filterCondition);
            }

            if(include != null)
            {
                foreach(var item in include)
                {
                    query = query.Include(item);
                }
            }

            if(orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _context.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(TEntity entity)
        {
            if(_context.Entry(entity).State == EntityState.Detached)
            {
                    _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

    }
}
