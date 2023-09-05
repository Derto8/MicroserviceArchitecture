using Helpers.JWTValidate.DTOs;
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
    public class SecurityKeyProvider
    {
        private SecurityKey PrivateKey { get; }
        private PublicKeyDTO PublicKey { get; }


        public SecurityKeyProvider(IOptions<AuthOptions> options)
        {
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(
                source: Convert.FromBase64String(options.Value.PrivateKey),
                bytesRead: out int _);

            PrivateKey = new RsaSecurityKey(rsa);

            PublicKey = new PublicKeyDTO
            {
                PublicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey())
            };
        }

        public SecurityKey GetPrivateKey() => PrivateKey;
        public PublicKeyDTO GetPublicKey() => PublicKey;
    }
}
