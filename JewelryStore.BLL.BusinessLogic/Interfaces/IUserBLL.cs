using JewelryStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JewelryStore.BLL.Interfaces
{
    public interface IUserBLL
    {
        Task<bool> AuthenticateUser(UserModel userModel);
    }
}
