using JeweleryStore.Common.Enums;
using JewelryStore.Common.Interfaces;

namespace JewelryStore.Domain.Entities
{
    /// <summary>
    /// Represents the user of the Jwellery Store
    /// </summary>
    public class User : IEntity
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets the Fullname of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the User Category
        /// </summary>
        public UserType UserCategory { get; set; }

        /// <summary>
        /// Gets or Sets the Username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or Sets the password
        /// </summary>
        public string Password { get; set; }
    }
}
