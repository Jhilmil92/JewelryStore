using JeweleryStore.Common.Enums;
using JewelryStore.Common.Interfaces;

namespace JewelryStore.Domain.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserType UserCategory { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
