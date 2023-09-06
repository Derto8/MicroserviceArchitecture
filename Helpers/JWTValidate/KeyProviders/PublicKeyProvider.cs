using Helpers.JWTValidate.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.JWTValidate.KeyProviders
{
    /// <summary>
    /// Провайдер публичного ключа, имплементирующий интерфейс IPublicKeyProvider
    /// </summary>
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
