using JewelryStore.Common.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelryStore.Domain.Entities
{
    /// <summary>
    /// Mapping between User and Discount against an Item
    /// </summary>
    public class UserDiscount : IEntity
    {
        /// <summary>
        /// Gets or Sets the UserID  Foreign Key
        /// </summary>
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or Sets the User
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or Sets the ItemId as Foreign Key
        /// </summary>
        [ForeignKey("Item")]
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or Sets the Item
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Gets or Sets the Discount for the user and Item
        /// </summary>
        public double Discount { get; set; }

        /// <summary>
        /// Gets the Id
        /// </summary>
        public int Id { get => UserId; set => UserId = value; }
    }
}
