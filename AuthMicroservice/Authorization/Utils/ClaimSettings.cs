using DBContext.Models;
using System.Security.Claims;

namespace AuthMicroservice.Authorization.Utils
{
    public static class ClaimSettings
    {
        public static List<Claim> GetClaims(Users user, CancellationToken cancellationToken)
        {
            var claims = new List<Claim> {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };

            return claims;
        }
    }
}
