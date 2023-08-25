using AuthMicroservice.Authorization.Utils.DTOs;
using AuthMicroservice.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace AuthMicroservice.Authorization.Utils.KeyProviders
{
    public class PublicKeyProvider : IPublicKeyProvider
    {
        private AuthOptions _authOptions { get; }
        public PublicKeyProvider(IOptions<AuthOptions> authOptions)
        {
            _authOptions = authOptions.Value;
        }

        public SecurityKey GetKey()
        {
            var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(Convert.FromBase64String(_authOptions.PublicKey), out int _);

            return new RsaSecurityKey(rsa);
        }
    }
}
