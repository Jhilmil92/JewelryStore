using JewelryStore.DataRepository.Interfaces;
using System;
using System.Threading.Tasks;
using JewelryStore.Common.Interfaces;
using JewelryStore.DataRepository.Contexts;

namespace JewelryStore.DataRepository.Classes
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly JewelryStoreDbContext _context;
        public UnitOfWork(JewelryStoreDbContext context)
        {
            _context = context;
        }
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class,IEntity
        {
            //var entityType = typeof(TEntity).Name;
            return new GenericRepository<TEntity>(_context);
            //return (TEntity)Activator.CreateInstance(typeof(TEntity), _context);
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
                var saveResult = await _context.SaveChangesAsync();
                return true;
                ///TODO: Log the save result
            }
            catch (Exception ex)
            {
                ///TODO: Log the exception

                return false;
            }
        }
    }
}
