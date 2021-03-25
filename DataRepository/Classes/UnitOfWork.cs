using JewelryStore.DataRepository.Interfaces;
using System;
using System.Threading.Tasks;
using JewelryStore.Common.Interfaces;
using JewelryStore.DataRepository.Contexts;
using Serilog;

namespace JewelryStore.DataRepository.Classes
{
    /// <summary>
    /// Unit of Work
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {

        private readonly JewelryStoreDbContext _context;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(JewelryStoreDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class,IEntity
        {
            return new GenericRepository<TEntity>(_context);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChanges()
        {
            try
            {
                var saveResult = await _context.SaveChangesAsync();
                Log.Debug($"Saved successfully {saveResult} items.");
                return true;
                ///TODO: Log the save result
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, ex.Message);
                return false;
            }
        }
    }
}
