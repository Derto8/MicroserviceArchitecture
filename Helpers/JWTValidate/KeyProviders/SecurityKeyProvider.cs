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
    /// <summary>
    /// Провайдер приватного и публичного ключа
    /// </summary>
    public class SecurityKeyProvider
    {
        private SecurityKey PrivateKey { get; }
        private PublicKeyDTO PublicKey { get; }


        /// <summary>
        /// Шифруем публичный и приватный ключи
        /// </summary>
        /// <param name="options"></param>
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

        /// <summary>
        /// Возвращает приватный ключ
        /// </summary>
        /// <returns>Приватный ключ</returns>
        public SecurityKey GetPrivateKey() => PrivateKey;
        /// <summary>
        /// Возвращает публчный ключ
        /// </summary>
        /// <returns>Публичный ключ</returns>
        public PublicKeyDTO GetPublicKey() => PublicKey;
    }
}
