using JeweleryStore.Common.Enums;
using JewelryStore.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JewelryStore.BLL.Interfaces
{
    public interface IItemsBLL
    {
        Task<ItemModel> GetItemById(int id);

        Task<IEnumerable<ItemModel>> GetItems();

        Task<double> ComputeTotalPriceAsync(double metalPrice, double weight,int itemId, UserType userType,string userName);

        Task<bool> CreateItem(ItemModel item);

        Task<bool> UpdateItem(ItemModel item);

        Task<bool> DeleteItem(ItemModel item);
    }
}
