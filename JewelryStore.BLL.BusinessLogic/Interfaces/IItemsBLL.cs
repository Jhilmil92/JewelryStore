using JewelryStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JewelryStore.BLL.Interfaces
{
    public interface IItemsBLL
    {
        Task<ItemModel> GetItemById(int id);

        Task<IEnumerable<ItemModel>> GetItems();

        Task<double> ComputeTotalPriceAsync(double metalPrice, double weight);

        Task<bool> CreateItem(ItemModel item);

        Task<bool> UpdateItem(ItemModel item);

        Task<bool> DeleteItem(ItemModel item);
    }
}
