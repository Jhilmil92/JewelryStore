using JewelryStore.Common.Utilities;
using JewelryStore.Domain.Models;
using System.Threading.Tasks;

namespace JewelryStore.BLL.Interfaces
{
    public interface IUserBLL
    {
        Task<AuthResponse> AuthenticateUser(UserModel userModel);
    }
}
