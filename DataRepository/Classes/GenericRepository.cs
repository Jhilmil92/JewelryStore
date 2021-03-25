using JewelryStore.Common.Interfaces;
using JewelryStore.DataRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JewelryStore.DataRepository.Classes
{
    /// <summary>
    /// Generic Repository for CRUD operation on any entity of type <see cref="IEntity"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class,IEntity
    {
        protected DbContext _context;
        protected DbSet<TEntity> _dbSet;

        /// <summary>
        /// Creates an instance of <see cref="GenericRepository"/>.
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        /// <summary>
        /// Creates an entry in the DBContext
        /// </summary>
        /// <param name="entity"></param>
        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// Gets query result based on conditions.
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filterCondition = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string[] include = null)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable<TEntity>();
            if(filterCondition != null)
            {
                query = query.Where(filterCondition);
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

        /// <summary>
        /// Gets by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes the entity
        /// </summary>
        /// <param name="entity"></param>
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
