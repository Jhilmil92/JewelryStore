using System.ComponentModel.DataAnnotations;

namespace JewelryStore.Domain.Models
{
    public class UserModel
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
