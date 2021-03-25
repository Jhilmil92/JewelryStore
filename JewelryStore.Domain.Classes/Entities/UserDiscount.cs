using JewelryStore.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JewelryStore.Domain.Entities
{
    public class UserDiscount : IEntity
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public double Discount { get; set; }
        public int Id { get => UserId; set => UserId = value; }
    }
}
