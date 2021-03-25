using JewelryStore.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace JewelryStoreApp.Services
{
    public interface IHttpServices
    {
        /// <summary>
        /// Gets the Claim Identity of the HttpContext
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        UserClaim GetClaimsIdentity(HttpContext httpContext);
    }
}
