using AuthMicroservice.Authorization.Utils.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace AuthMicroservice.Authorization.Utils.KeyProviders
{
    public class SecurityKeyProvider
    {
        public SymmetricSecurityKey GetSymmetricSecurityKey(string KEY) =>
       new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));

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
