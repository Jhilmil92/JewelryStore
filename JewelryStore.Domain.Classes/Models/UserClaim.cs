using JeweleryStore.Common.Enums;

namespace JewelryStore.Domain.Models
{
    /// <summary>
    /// Model for the claim of a authorized user.
    /// </summary>
    public class UserClaim
    {
        /// <summary>
        /// Gets or Sets the UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or Sets the UserType
        /// </summary>
        public UserType UserType { get; set; }
    }
}
