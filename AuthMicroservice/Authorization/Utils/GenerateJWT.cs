using DBContext.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AuthMicroservice.Authorization.Utils
{
    public static class GenerateJWT
    {
        public static IResult Generate(Users user, AuthOptions _authOptions, CancellationToken cancellationToken)
        {
            //настройка клаймов (клайм логина и роли юзера)
            var claims = ClaimSettings.GetClaims(user, cancellationToken);

            SecurityKeyProvider securityKeyProvider = new SecurityKeyProvider(_authOptions.KEY);

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _authOptions.ISSUER,
                audience: _authOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                signingCredentials: new SigningCredentials(securityKeyProvider.GetSymmetricSecurityKey(_authOptions.KEY), SecurityAlgorithms.HmacSha256)
            );

            //подписываю токен
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            var responce = new
            {
                access_token = token,
                userId = user.Id,
            };

            return Results.Json(responce);
        }
    }
}
