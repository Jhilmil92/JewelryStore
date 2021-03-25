using JeweleryStore.Common.Enums;
using JewelryStore.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace JewelryStoreApp.Services
{
    public class HttpServices : IHttpServices
    {
        /// <summary>
        /// gets the Claim Identity
        /// </summary>
        /// <returns></returns>
        public UserClaim GetClaimsIdentity(HttpContext httpContext)
        {
            var claimsIdentity = httpContext.User.Identity as ClaimsIdentity;
            
            if (claimsIdentity != null)
            {
                var type = claimsIdentity.FindFirst("UserType").Value;
                UserType userType = Enum.Parse<UserType>(type);
                string userName = claimsIdentity.FindFirst(ClaimTypes.Name).Value;
                return new UserClaim() { UserName = userName, UserType = userType };
            }

            return null;
        }
    }
}
