using System;
using System.Threading.Tasks;
using JewelryStore.BLL.Interfaces;
using JewelryStore.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace JewelryStoreApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserBLL _userBLL;
        
        /// <summary>
        /// Creates an instance of <see cref="UsersController"/> 
        /// </summary>
        /// <param name="userBLL"></param>
        public UsersController(IUserBLL userBLL)
        {
            _userBLL = userBLL;
        }

        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]UserModel userModel)
        {
            try
            {
                var response = await _userBLL.AuthenticateUser(userModel);
                if (response != null)
                {
                    return Ok(new { token = response.JwtToken });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, ex.Message);
                throw;
            }
        }
    }
}