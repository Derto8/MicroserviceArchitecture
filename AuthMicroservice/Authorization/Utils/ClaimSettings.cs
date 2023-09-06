using DBContext.Models;
using System.Security.Claims;

namespace AuthMicroservice.Authorization.Utils
{
    /// <summary>
    /// Класс настройки клаймов
    /// </summary>
    public static class ClaimSettings
    {
        /// <summary>
        /// Метод настраивающий клаймы
        /// </summary>
        /// <param name="user">Модель юзера</param>
        /// <returns></returns>
        public static List<Claim> GetClaims(Users user)
        {
            var claims = new List<Claim> {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };

            return claims;
        }
    }
}
