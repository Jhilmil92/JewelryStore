
using JewelryStore.Common.Interfaces;

namespace JewelryStore.Domain.Entities
{
    /// <summary>
    /// Represents an item from the Jwellery Story
    /// </summary>
    public class Item : IEntity
    {
        /// <summary>
        /// Gets or Sets the ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets the name of the item
        /// </summary>
        public string ItemName { get; set; }
    }
}
