using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JewelryStore.BLL.Interfaces;
using JewelryStore.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelryStoreApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserBLL _userBLL;
        public UsersController(IUserBLL userBLL)
        {
            _userBLL = userBLL;
        }

        [AllowAnonymous]
        [Route("users/getString")]
        [HttpGet]
        public string Login()
        {
            return "Hi";
        }

        [HttpPost]
        public async Task<bool> Login(UserModel userModel)
        {
            return await _userBLL.AuthenticateUser(userModel);
        }
    }
}