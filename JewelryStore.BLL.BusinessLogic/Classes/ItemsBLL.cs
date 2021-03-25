using JewelryStore.BLL.Interfaces;
using JewelryStore.DataRepository.Interfaces;
using JewelryStore.Domain.Entities;
using JewelryStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JewelryStore.BLL.Classes
{
    public class ItemsBLL : IItemsBLL
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemsBLL(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ItemModel> GetItemById(int id)
        {
            var item = await _unitOfWork.Repository<Item>().GetByIdAsync(id);
            return new ItemModel
            {
                Id = item.Id,
                ItemName = item.ItemName
            };
        }

        public async Task<IEnumerable<ItemModel>> GetItems()
        {
            var result= await _unitOfWork.Repository<Item>().Get();

            return result.ToList().Select(x => new ItemModel() { Id = x.Id, ItemName = x.ItemName }); 
        }

        public async Task<double> ComputeTotalPriceAsync(double metalPrice, double weight)
        {
            return await Task.Run(() =>
            {
               return metalPrice* weight;
            });
            
        }


        public async Task<bool> CreateItem(ItemModel item)
        {
            var newItem = new Item
            {
                ItemName = item.ItemName
            };
            try
            {
                _unitOfWork.Repository<Item>().Create(newItem);
                 return await _unitOfWork.SaveChanges();
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateItem(ItemModel item)
        {
            var updateItem = new Item
            {
                ItemName = item.ItemName
            };

            try
            {
                _unitOfWork.Repository<Item>().Update(updateItem);
                return await _unitOfWork.SaveChanges();
            }
            catch(Exception ex) 
            {
                //log data.
                return false;
            }
        }
        public async Task<bool> DeleteItem(ItemModel item)
        {
            var deleteItem = new Item
            {
                ItemName = item.ItemName
            };

            try
            {
                _unitOfWork.Repository<Item>().Delete(deleteItem);
                return await _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                //log data.
                return false;
            }
        }
    }
}
