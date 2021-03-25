using JewelryStore.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JewelryStore.DataRepository.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class,IEntity;
        Task<bool> SaveChanges();
    }
}
