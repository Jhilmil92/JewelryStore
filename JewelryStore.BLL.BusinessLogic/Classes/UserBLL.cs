using JeweleryStore.Common.Enums;
using JewelryStore.BLL.Interfaces;
using JewelryStore.Common.Utilities;
using JewelryStore.DataRepository.Interfaces;
using JewelryStore.Domain.Entities;
using JewelryStore.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JewelryStore.BLL.Classes
{
    public class UserBLL : IUserBLL
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public UserBLL(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponse> AuthenticateUser(UserModel userModel)
        {
            var filteredUsers = await _unitOfWork.Repository<User>().Get(a => a.UserName.ToLower() == userModel.UserName.ToLower());
            var user = filteredUsers.ToArray().First();
            AuthResponse authresponse = null;
            if (user != null)
            {
                bool isValid = true;
                isValid = CheckPassword(user.Password, userModel.Password);
                if (isValid)
                {
                    var jwtToken = GenerateJSONWebToken(userModel, user.UserCategory);
                    authresponse = new AuthResponse(userModel, jwtToken);
                }
            }
           
            return authresponse;
        }


        private bool CheckPassword(string originalPassword, string enteredPassword)
        {
            return string.Equals(originalPassword, enteredPassword);
        }
        private string GenerateJSONWebToken(UserModel userInfo, UserType userType)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userInfo.UserName),
                    new Claim("UserType", Enum.GetName(typeof(UserType), userType))
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
                Issuer = _configuration["JwtSettings:TokenIssuer"],
                Audience= _configuration["JwtSettings:TokenIssuer"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
