using JeweleryStore.Common.Enums;
using JewelryStore.BLL.Interfaces;
using JewelryStore.DataRepository.Interfaces;
using JewelryStore.Domain.Entities;
using JewelryStore.Domain.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JewelryStore.BLL.Classes
{
    /// <summary>
    /// Business Logic for Item related logic
    /// </summary>
    public class ItemsBLL : IItemsBLL
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Creates an instance of <see cref="ItemsBLL"/>
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ItemsBLL(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets Item by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ItemModel> GetItemById(int id)
        {
            var item = await _unitOfWork.Repository<Item>().GetByIdAsync(id);
            if (item == null)
            {
                return null;
            }

            return new ItemModel
            {
                Id = item.Id,
                ItemName = item.ItemName
            };
        }

        /// <summary>
        /// Gets all the Items.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ItemModel>> GetItems()
        {
            var result= await _unitOfWork.Repository<Item>().Get();

            return result.ToList().Select(x => new ItemModel() { Id = x.Id, ItemName = x.ItemName }); 
        }

        /// <summary>
        /// Computes the total price based on User, Item and relevant discount.
        /// </summary>
        /// <param name="metalPrice"></param>
        /// <param name="weight"></param>
        /// <param name="itemId"></param>
        /// <param name="userType"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<double> ComputeTotalPriceAsync(double metalPrice, double weight, int itemId, UserType userType, string userName)
        {
            var valueOfItem = metalPrice * weight;
            if (userType == UserType.Privileged)
            {
                var user = await _unitOfWork.Repository<User>().Get(x => x.UserName.Equals(userName));
                var result = await _unitOfWork.Repository<UserDiscount>().Get(x => x.ItemId == itemId && x.UserId == user.First().Id);
                if (result != null && result.Any())
                { 
                    valueOfItem = valueOfItem * (100.00 - result.First().Discount)/100; 
                }
            }
            return valueOfItem;
        }

        /// <summary>
        /// Creates an item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
                Log.Fatal(ex, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Updates Item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
                Log.Fatal(ex, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Delete Item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
                Log.Fatal(ex, ex.Message);
                return false;
            }
        }
    }
}
