
using JewelryStore.Common.Interfaces;

namespace JewelryStore.Domain.Entities
{
    public class Item : IEntity
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
    }
}
