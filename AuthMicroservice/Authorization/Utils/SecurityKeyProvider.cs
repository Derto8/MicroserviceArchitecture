using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace AuthMicroservice.Authorization.Utils
{
    public class SecurityKeyProvider
    {
        public SymmetricSecurityKey GetSymmetricSecurityKey(string KEY) =>
       new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));

        private SecurityKey PrivateKey { get; }
        private string PublicKey { get; }
        

        public SecurityKeyProvider(string key)
        {
            var privateKey = $"-----BEGIN RSA PRIVATE KEY-----{key}-----END RSA PRIVATE KEY-----";
            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKey);

            PrivateKey = new RsaSecurityKey(rsa);

            PublicKey = "-----BEGIN PUBLIC KEY-----" + Convert.ToBase64String(rsa.ExportRSAPublicKey()) + "-----END PUBLIC KEY-----";
        }

        public SecurityKey GetPrivateKey() => PrivateKey;
        public string GetPublicKey() => PublicKey;


    }
}
