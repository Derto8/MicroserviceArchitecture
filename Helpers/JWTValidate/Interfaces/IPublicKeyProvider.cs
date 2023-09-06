using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.JWTValidate.Interfaces
{
    /// <summary>
    /// Интерфейс определяющий методы для взаимодействия с публичным ключем 
    /// </summary>
    public interface IPublicKeyProvider
    {
        /// <summary>
        /// Метод возвращающий публичный ключ
        /// </summary>
        /// <returns>Публчиный ключ</returns>
        SecurityKey GetKey();
    }
}
