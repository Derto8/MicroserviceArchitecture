using AuthMicroservice.Authorization.Utils.KeyProviders;
using DBContext.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AuthMicroservice.Authorization.Utils
{
    public static class GenerateJWT
    {
        public static IResult Generate(Users user, IOptions<AuthOptions> _authOptions, CancellationToken cancellationToken)
        {
            //настройка клаймов (клайм логина и роли юзера)
            var claims = ClaimSettings.GetClaims(user, cancellationToken);

            SecurityKeyProvider securityKeyProvider = new SecurityKeyProvider(_authOptions);

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _authOptions.Value.ISSUER,
                audience: _authOptions.Value.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                signingCredentials: new SigningCredentials(securityKeyProvider.GetPrivateKey(), SecurityAlgorithms.RsaSha512)
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
