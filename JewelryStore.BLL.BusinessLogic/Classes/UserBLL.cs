using JewelryStore.BLL.Interfaces;
using JewelryStore.DataRepository.Interfaces;
using JewelryStore.Domain.Entities;
using JewelryStore.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace JewelryStore.BLL.Classes
{
    public class UserBLL : IUserBLL
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserBLL(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AuthenticateUser(UserModel userModel)
        {
            var filteredUsers = await _unitOfWork.Repository<User>().Get(a => a.UserName == userModel.UserName);
            var user = filteredUsers.ToArray().First();
            bool isValid = true;
            if(userModel.UserName == user.UserName)
            {
                isValid = CheckPassword(user.Password, userModel.Password);
            }
            return isValid;
        }

        private bool CheckPassword(string originalPassword, string enteredPassword)
        {
            return string.Equals(originalPassword, enteredPassword);
        }
    }
}
