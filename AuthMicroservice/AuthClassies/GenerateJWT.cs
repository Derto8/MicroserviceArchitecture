using DBContext.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AuthMicroservice.AuthClassies
{
    public static class GenerateJWT
    {
        public static IResult Generate(Users user, IConfiguration configuration, CancellationToken cancellationToken)
        {
            //настройка клаймов (клайм логина и роли юзера)
            var claims = ClaimSettings.GetClaims(user, cancellationToken);

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: configuration["ISSUER"],
                audience: configuration["AUDIENCE"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                signingCredentials: new SigningCredentials(KeyEncryption.GetSymmetricSecurityKey(configuration["KEY"]), SecurityAlgorithms.HmacSha256)
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
