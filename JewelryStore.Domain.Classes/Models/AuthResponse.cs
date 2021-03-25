using JewelryStore.Domain.Models;

namespace JewelryStore.Common.Utilities
{
    /// <summary>
    /// Authotentication Response
    /// </summary>
    public class AuthResponse
    {
        public string UserName { get; set; }
        public string JwtToken { get; set; }

        public AuthResponse(UserModel user, string jwtToken)
        {
            UserName = user.UserName;
            JwtToken = jwtToken;
        }
    }
}
