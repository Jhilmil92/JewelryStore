using JewelryStore.Common.Interfaces;
using System.Threading.Tasks;

namespace JewelryStore.DataRepository.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class,IEntity;
        Task<bool> SaveChanges();
    }
}
