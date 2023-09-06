using DBContext.Models;
using DTOs.AuthDTOs;
using Helpers.JWTValidate;
using Helpers.JWTValidate.KeyProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AuthMicroservice.Authorization.Utils
{
    /// <summary>
    /// Класс генерации jwt
    /// </summary>
    public static class GenerateJWT
    {
        /// <summary>
        /// Генерирует jwt
        /// </summary>
        /// <param name="user">Модель юзера</param>
        /// <param name="_authOptions">Параметры авторизации на сервере</param>
        /// <returns>JSON, в котором находится jwt и id записи с данными юзера, 
        /// которому принадлежит данный jwt</returns>
        public static IResult Generate(Users user, IOptions<AuthOptions> _authOptions)
        {
            //настройка клаймов (клайм логина и роли юзера)
            var claims = ClaimSettings.GetClaims(user);

            SecurityKeyProvider securityKeyProvider = new SecurityKeyProvider(_authOptions);

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _authOptions.Value.ISSUER,
                audience: _authOptions.Value.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                signingCredentials: new SigningCredentials(securityKeyProvider.GetPrivateKey(), 
                SecurityAlgorithms.RsaSha512)
            );

            //подписываю токен
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            JWT responce = new JWT
            {
                access_token = token,
                userId = user.Id,
            };

            return Results.Json(responce);
        }
    }
}
